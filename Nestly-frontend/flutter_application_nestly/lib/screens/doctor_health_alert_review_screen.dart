import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:flutter_application_nestly/layouts/nestly_toast.dart';
import 'package:flutter_application_nestly/main.dart';
import 'package:flutter_application_nestly/network/api_client.dart';
import 'package:flutter_application_nestly/providers/api_response_helper.dart';

/// Doctor-facing review queue for ML-generated health deviation alerts.
/// Lets a doctor confirm or dispute whether an alert was clinically
/// accurate - the explicit "human in the loop" feedback the thesis lists
/// as a future-work item, wired up end to end here rather than left as a
/// backend-only stub.
class DoctorHealthAlertReviewScreen extends StatefulWidget {
  const DoctorHealthAlertReviewScreen({super.key});

  @override
  State<DoctorHealthAlertReviewScreen> createState() =>
      _DoctorHealthAlertReviewScreenState();
}

class _DoctorHealthAlertReviewScreenState
    extends State<DoctorHealthAlertReviewScreen> {
  List<dynamic> _alerts = [];
  bool _loading = true;
  bool _onlyPending = true;

  @override
  void initState() {
    super.initState();
    _load();
  }

  Future<void> _load() async {
    setState(() => _loading = true);

    try {
      final filter = _onlyPending ? '&hasDoctorFeedback=false' : '';
      final res = await ApiClient.get(
        '/api/health-alerts/doctor?pageSize=100$filter',
      );

      if (res.statusCode != 200) {
        final error = jsonDecode(res.body);
        throw Exception(
          error["message"] ?? "Greška pri učitavanju upozorenja",
        );
      }

      final all = ApiResponseHelper.extractList(res.body);

      if (!mounted) return;

      setState(() {
        _alerts = all;
        _loading = false;
      });
    } catch (e) {
      if (!mounted) return;
      setState(() => _loading = false);
      NestlyToast.error(context, 'Greška pri učitavanju upozorenja.');
    }
  }

  Future<void> _submitFeedback(
    int id, {
    required bool isAccurate,
    String? comment,
  }) async {
    try {
      final res = await ApiClient.post(
        '/api/health-alerts/$id/feedback',
        body: {'isAccurate': isAccurate, 'comment': comment},
      );

      if (res.statusCode != 200) {
        final error = jsonDecode(res.body);
        throw Exception(error["message"] ?? "Greška pri slanju povratne informacije");
      }

      if (!mounted) return;

      setState(() {
        if (_onlyPending) {
          _alerts.removeWhere((a) => a["id"] == id);
        } else {
          final index = _alerts.indexWhere((a) => a["id"] == id);
          if (index != -1) {
            _alerts[index] = jsonDecode(res.body);
          }
        }
      });

      NestlyToast.success(context, 'Povratna informacija je zabilježena');
    } catch (e) {
      NestlyToast.error(context, 'Greška pri slanju povratne informacije');
    }
  }

  static const _severityMeta = {
    1: (label: 'Informativno', color: Color(0xFF3B82F6)),
    2: (label: 'Upozorenje', color: Color(0xFFF59E0B)),
    3: (label: 'Kritično', color: Color(0xFFEF4444)),
  };

  static const _parameterMeta = {
    1: (label: 'Rast', icon: Icons.trending_up_rounded),
    2: (label: 'Hranjenje', icon: Icons.restaurant_rounded),
    3: (label: 'San', icon: Icons.bedtime_rounded),
    4: (label: 'Pelene', icon: Icons.baby_changing_station_rounded),
    5: (label: 'Temperatura', icon: Icons.thermostat_rounded),
  };

  String _formatDate(String? iso) {
    if (iso == null) return '';
    final d = DateTime.tryParse(iso);
    if (d == null) return '';
    return '${d.day.toString().padLeft(2, '0')}.${d.month.toString().padLeft(2, '0')}.${d.year}.';
  }

  Future<void> _promptFeedback(dynamic alert, {required bool isAccurate}) async {
    final commentCtrl = TextEditingController();

    final confirmed = await showDialog<bool>(
      context: context,
      builder: (context) => AlertDialog(
        title: Text(isAccurate ? 'Potvrdi tačnost' : 'Označi kao netačno'),
        content: TextField(
          controller: commentCtrl,
          maxLines: 3,
          decoration: const InputDecoration(
            hintText: 'Opcionalan komentar...',
            border: OutlineInputBorder(),
          ),
        ),
        actions: [
          TextButton(
            onPressed: () => Navigator.pop(context, false),
            child: const Text('Otkaži'),
          ),
          ElevatedButton(
            onPressed: () => Navigator.pop(context, true),
            child: const Text('Potvrdi'),
          ),
        ],
      ),
    );

    if (confirmed == true) {
      await _submitFeedback(
        alert["id"],
        isAccurate: isAccurate,
        comment: commentCtrl.text.trim().isEmpty ? null : commentCtrl.text.trim(),
      );
    }
  }

  @override
  Widget build(BuildContext context) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Row(
          mainAxisAlignment: MainAxisAlignment.spaceBetween,
          children: [
            Text(
              'Zdravstvena upozorenja',
              style: Theme.of(context).textTheme.titleLarge?.copyWith(
                fontWeight: FontWeight.w800,
                color: AppColors.seed,
              ),
            ),
            FilterChip(
              label: Text(
                'Samo neocijenjena',
                style: TextStyle(
                  color: _onlyPending ? Colors.white : AppColors.textSecondary,
                  fontWeight: FontWeight.w600,
                ),
              ),
              selected: _onlyPending,
              selectedColor: AppColors.seed,
              backgroundColor: AppColors.card,
              checkmarkColor: Colors.white,
              shape: RoundedRectangleBorder(
                borderRadius: BorderRadius.circular(20),
                side: BorderSide(
                  color: _onlyPending
                      ? AppColors.seed
                      : Colors.black.withOpacity(.1),
                ),
              ),
              onSelected: (v) {
                setState(() => _onlyPending = v);
                _load();
              },
            ),
          ],
        ),
        const SizedBox(height: 6),
        Text(
          'Označite da li je automatski generisano upozorenje bilo klinički '
          'tačno. Ova povratna informacija čuva se uz upozorenje i služi kao '
          'osnova za buduću evaluaciju/re-treniranje modela.',
          style: Theme.of(
            context,
          ).textTheme.bodySmall?.copyWith(color: AppColors.textSecondary),
        ),
        const SizedBox(height: AppSpacing.lg),
        Expanded(
          child: _loading
              ? const Center(child: CircularProgressIndicator())
              : _alerts.isEmpty
              ? const Center(child: Text('Nema upozorenja za prikaz.'))
              : RefreshIndicator(
                  onRefresh: _load,
                  child: ListView.separated(
                    itemCount: _alerts.length,
                    separatorBuilder: (_, __) =>
                        const SizedBox(height: AppSpacing.md),
                    itemBuilder: (context, i) {
                      final alert = _alerts[i];
                      final hasFeedback = alert["doctorFeedbackIsAccurate"] != null;
                      final isAccurate = alert["doctorFeedbackIsAccurate"] == true;

                      final severity = _severityMeta[alert["severity"]] ??
                          _severityMeta[2]!;
                      final parameter = _parameterMeta[alert["parameterType"]] ??
                          (label: 'Parametar', icon: Icons.info_outline_rounded);

                      return Container(
                        decoration: BoxDecoration(
                          borderRadius: BorderRadius.circular(AppRadius.lg),
                          boxShadow: [
                            BoxShadow(
                              color: Colors.black.withOpacity(.03),
                              blurRadius: 10,
                              offset: const Offset(0, 4),
                            ),
                          ],
                        ),
                        child: ClipRRect(
                          borderRadius: BorderRadius.circular(AppRadius.lg),
                          child: Container(
                            decoration: BoxDecoration(
                              color: Colors.white,
                              border: Border.all(
                                color: Colors.black.withOpacity(.06),
                              ),
                            ),
                            child: IntrinsicHeight(
                              child: Row(
                                crossAxisAlignment: CrossAxisAlignment.stretch,
                                children: [
                                  Container(width: 4, color: severity.color),
                                  Expanded(
                                    child: Padding(
                                      padding: const EdgeInsets.all(
                                        AppSpacing.lg,
                                      ),
                                      child: Column(
                          crossAxisAlignment: CrossAxisAlignment.start,
                          children: [
                            Row(
                              children: [
                                _Pill(
                                  label: severity.label,
                                  color: severity.color,
                                ),
                                const SizedBox(width: 8),
                                _Pill(
                                  label: parameter.label,
                                  icon: parameter.icon,
                                  color: AppColors.seed,
                                ),
                                const Spacer(),
                                if (alert["detectedAt"] != null)
                                  Text(
                                    _formatDate(alert["detectedAt"]),
                                    style: Theme.of(context).textTheme.bodySmall
                                        ?.copyWith(color: AppColors.textSecondary),
                                  ),
                              ],
                            ),
                            const SizedBox(height: 10),

                            Text(
                              alert["title"] ?? "",
                              style: Theme.of(context).textTheme.titleMedium
                                  ?.copyWith(fontWeight: FontWeight.w800),
                            ),
                            if (alert["babyName"] != null) ...[
                              const SizedBox(height: 2),
                              Row(
                                children: [
                                  const Icon(
                                    Icons.child_care_rounded,
                                    size: 14,
                                    color: AppColors.textSecondary,
                                  ),
                                  const SizedBox(width: 4),
                                  Text(
                                    alert["babyName"],
                                    style: Theme.of(context).textTheme.bodySmall
                                        ?.copyWith(color: AppColors.textSecondary),
                                  ),
                                ],
                              ),
                            ],
                            const SizedBox(height: 10),

                            Text(
                              alert["message"] ?? "",
                              style: const TextStyle(height: 1.4),
                            ),

                            const SizedBox(height: 10),
                            Container(
                              width: double.infinity,
                              padding: const EdgeInsets.all(AppSpacing.md),
                              decoration: BoxDecoration(
                                color: AppColors.babyBlue.withOpacity(.12),
                                borderRadius: BorderRadius.circular(AppRadius.md),
                              ),
                              child: Row(
                                crossAxisAlignment: CrossAxisAlignment.start,
                                children: [
                                  const Icon(
                                    Icons.lightbulb_outline_rounded,
                                    size: 18,
                                    color: AppColors.seed,
                                  ),
                                  const SizedBox(width: 8),
                                  Expanded(
                                    child: Text(
                                      alert["recommendation"] ?? "",
                                      style: const TextStyle(
                                        fontStyle: FontStyle.italic,
                                        height: 1.4,
                                      ),
                                    ),
                                  ),
                                ],
                              ),
                            ),

                            const SizedBox(height: AppSpacing.md),
                            if (hasFeedback)
                              Container(
                                width: double.infinity,
                                padding: const EdgeInsets.symmetric(
                                  horizontal: AppSpacing.md,
                                  vertical: AppSpacing.sm,
                                ),
                                decoration: BoxDecoration(
                                  color: (isAccurate ? Colors.green : Colors.redAccent)
                                      .withOpacity(.08),
                                  borderRadius: BorderRadius.circular(AppRadius.md),
                                  border: Border.all(
                                    color: (isAccurate
                                            ? Colors.green
                                            : Colors.redAccent)
                                        .withOpacity(.3),
                                  ),
                                ),
                                child: Row(
                                  children: [
                                    Icon(
                                      isAccurate
                                          ? Icons.check_circle_rounded
                                          : Icons.cancel_rounded,
                                      color: isAccurate
                                          ? Colors.green
                                          : Colors.redAccent,
                                      size: 20,
                                    ),
                                    const SizedBox(width: 8),
                                    Expanded(
                                      child: Column(
                                        crossAxisAlignment:
                                            CrossAxisAlignment.start,
                                        children: [
                                          Text(
                                            isAccurate
                                                ? 'Označeno kao tačno'
                                                : 'Označeno kao netačno',
                                            style: TextStyle(
                                              fontWeight: FontWeight.w700,
                                              color: isAccurate
                                                  ? Colors.green[800]
                                                  : Colors.red[800],
                                            ),
                                          ),
                                          Text(
                                            'Ocijenio: ${alert["doctorFeedbackByDoctorName"] ?? "doktor"}'
                                            '${alert["doctorFeedbackComment"] != null ? " — ${alert["doctorFeedbackComment"]}" : ""}',
                                            style: Theme.of(context)
                                                .textTheme
                                                .bodySmall
                                                ?.copyWith(
                                                  color:
                                                      AppColors.textSecondary,
                                                ),
                                          ),
                                        ],
                                      ),
                                    ),
                                  ],
                                ),
                              )
                            else
                              Row(
                                mainAxisAlignment: MainAxisAlignment.end,
                                children: [
                                  TextButton.icon(
                                    onPressed: () => _promptFeedback(
                                      alert,
                                      isAccurate: true,
                                    ),
                                    icon: const Icon(
                                      Icons.check_rounded,
                                      size: 16,
                                    ),
                                    label: const Text('Tačno'),
                                    style: TextButton.styleFrom(
                                      foregroundColor: Colors.green[700],
                                      backgroundColor: Colors.green
                                          .withOpacity(.1),
                                      padding: const EdgeInsets.symmetric(
                                        horizontal: 14,
                                        vertical: 8,
                                      ),
                                      minimumSize: Size.zero,
                                      tapTargetSize:
                                          MaterialTapTargetSize.shrinkWrap,
                                      textStyle: const TextStyle(
                                        fontSize: 13,
                                        fontWeight: FontWeight.w700,
                                      ),
                                      shape: RoundedRectangleBorder(
                                        borderRadius: BorderRadius.circular(
                                          20,
                                        ),
                                      ),
                                    ),
                                  ),
                                  const SizedBox(width: 8),
                                  TextButton.icon(
                                    onPressed: () => _promptFeedback(
                                      alert,
                                      isAccurate: false,
                                    ),
                                    icon: const Icon(
                                      Icons.close_rounded,
                                      size: 16,
                                    ),
                                    label: const Text('Netačno'),
                                    style: TextButton.styleFrom(
                                      foregroundColor: Colors.redAccent,
                                      backgroundColor: Colors.redAccent
                                          .withOpacity(.08),
                                      padding: const EdgeInsets.symmetric(
                                        horizontal: 14,
                                        vertical: 8,
                                      ),
                                      minimumSize: Size.zero,
                                      tapTargetSize:
                                          MaterialTapTargetSize.shrinkWrap,
                                      textStyle: const TextStyle(
                                        fontSize: 13,
                                        fontWeight: FontWeight.w700,
                                      ),
                                      shape: RoundedRectangleBorder(
                                        borderRadius: BorderRadius.circular(
                                          20,
                                        ),
                                      ),
                                    ),
                                  ),
                                ],
                              ),
                          ],
                                      ),
                                    ),
                                  ),
                                ],
                              ),
                            ),
                          ),
                        ),
                      );
                    },
                  ),
                ),
        ),
      ],
    );
  }
}

class _Pill extends StatelessWidget {
  final String label;
  final Color color;
  final IconData? icon;

  const _Pill({required this.label, required this.color, this.icon});

  @override
  Widget build(BuildContext context) {
    return Container(
      padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 4),
      decoration: BoxDecoration(
        color: color.withOpacity(.12),
        borderRadius: BorderRadius.circular(20),
      ),
      child: Row(
        mainAxisSize: MainAxisSize.min,
        children: [
          if (icon != null) ...[
            Icon(icon, size: 13, color: color),
            const SizedBox(width: 4),
          ],
          Text(
            label,
            style: TextStyle(
              fontSize: 12,
              fontWeight: FontWeight.w700,
              color: color,
            ),
          ),
        ],
      ),
    );
  }
}
