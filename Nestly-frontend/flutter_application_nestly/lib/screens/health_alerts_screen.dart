import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:flutter_application_nestly/layouts/nestly_toast.dart';
import 'package:flutter_application_nestly/main.dart';
import 'package:flutter_application_nestly/network/api_client.dart';
import 'package:flutter_application_nestly/providers/api_response_helper.dart';

/// Lists WHO/pedijatrijska odstupanja detektovana za jednu bebu (rast,
/// hranjenje, spavanje, pelene, temperatura) - vidi
/// Nestly.Services.Repository.BabyHealthMonitoringService na backendu.
class HealthAlertsScreen extends StatefulWidget {
  final int babyId;
  final String babyName;

  const HealthAlertsScreen({
    super.key,
    required this.babyId,
    required this.babyName,
  });

  @override
  State<HealthAlertsScreen> createState() => _HealthAlertsScreenState();
}

class _HealthAlertsScreenState extends State<HealthAlertsScreen> {
  List<dynamic> _alerts = [];
  bool _loading = true;
  bool _checking = false;

  @override
  void initState() {
    super.initState();
    _load();
  }

  Future<void> _load() async {
    try {
      final res = await ApiClient.get(
        '/api/health-alerts?babyId=${widget.babyId}&pageSize=100',
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
      NestlyToast.error(
        context,
        'Greška pri učitavanju zdravstvenih upozorenja. Pokušajte ponovo.',
      );
    }
  }

  Future<void> _resolve(int id) async {
    try {
      final res = await ApiClient.post('/api/health-alerts/$id/resolve');

      if (res.statusCode != 200 && res.statusCode != 204) {
        final error = jsonDecode(res.body);
        throw Exception(error["message"] ?? "Greška pri ažuriranju");
      }

      if (!mounted) return;

      setState(() {
        final index = _alerts.indexWhere((a) => a["id"] == id);
        if (index != -1) {
          _alerts[index] = {..._alerts[index], "isResolved": true};
        }
      });

      NestlyToast.success(context, 'Upozorenje označeno kao riješeno');
    } catch (e) {
      NestlyToast.error(context, 'Greška pri označavanju upozorenja');
    }
  }

  Future<void> _checkNow() async {
    setState(() => _checking = true);

    try {
      final res = await ApiClient.post(
        '/api/health-alerts/run/${widget.babyId}',
      );

      if (res.statusCode != 200 && res.statusCode != 204) {
        final error = jsonDecode(res.body);
        throw Exception(error["message"] ?? "Greška pri provjeri");
      }

      final created = ApiResponseHelper.extractList(res.body);

      if (!mounted) return;

      if (created.isEmpty) {
        NestlyToast.info(context, 'Nema novih odstupanja u ovom trenutku.');
      } else {
        NestlyToast.warning(
          context,
          'Pronađeno ${created.length} novo(a) odstupanje(a).',
        );
      }

      await _load();
    } catch (e) {
      if (!mounted) return;
      NestlyToast.error(context, 'Greška pri pokretanju provjere.');
    } finally {
      if (mounted) setState(() => _checking = false);
    }
  }

  int get _activeCount =>
      _alerts.where((a) => a["isResolved"] == false).length;

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppColors.bg,
      appBar: AppBar(
        backgroundColor: Colors.transparent,
        elevation: 0,
        iconTheme: const IconThemeData(color: AppColors.roseDark),
        centerTitle: true,
        title: Text(
          'Zdravstvena upozorenja',
          style: Theme.of(context).textTheme.titleLarge?.copyWith(
            fontWeight: FontWeight.w700,
            color: AppColors.roseDark,
          ),
        ),
      ),
      body: _loading
          ? const Center(
              child: CircularProgressIndicator(color: AppColors.roseDark),
            )
          : RefreshIndicator(
              color: AppColors.roseDark,
              onRefresh: _load,
              child: ListView(
                padding: const EdgeInsets.all(AppSpacing.lg),
                children: [
                  _IntroCard(
                    babyName: widget.babyName,
                    activeCount: _activeCount,
                    checking: _checking,
                    onCheckNow: _checkNow,
                  ),
                  const SizedBox(height: AppSpacing.lg),
                  if (_alerts.isEmpty)
                    const _EmptyState()
                  else
                    for (final alert in _alerts)
                      Padding(
                        padding: const EdgeInsets.only(
                          bottom: AppSpacing.md,
                        ),
                        child: _AlertCard(
                          alert: alert,
                          onResolve: alert["isResolved"] == true
                              ? null
                              : () => _resolve(alert["id"]),
                        ),
                      ),
                ],
              ),
            ),
    );
  }
}

class _IntroCard extends StatelessWidget {
  final String babyName;
  final int activeCount;
  final bool checking;
  final VoidCallback onCheckNow;

  const _IntroCard({
    required this.babyName,
    required this.activeCount,
    required this.checking,
    required this.onCheckNow,
  });

  @override
  Widget build(BuildContext context) {
    return Card(
      elevation: 0,
      shape: RoundedRectangleBorder(
        borderRadius: BorderRadius.circular(AppRadius.xl),
      ),
      child: Padding(
        padding: const EdgeInsets.all(AppSpacing.lg),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Text(
              activeCount == 0
                  ? '$babyName nema aktivnih upozorenja.'
                  : '$babyName ima $activeCount aktivno upozorenje.',
              style: Theme.of(context).textTheme.titleMedium?.copyWith(
                fontWeight: FontWeight.w700,
                color: AppColors.textPrimary,
              ),
            ),
            const SizedBox(height: 6),
            Text(
              'Ovdje se prikazuju odstupanja unosa (rast, hranjenje, spavanje, '
              'pelene, temperatura) u odnosu na preporučene pedijatrijske '
              'standarde, otkrivena tokom nekoliko dana praćenja.',
              style: Theme.of(context).textTheme.bodySmall?.copyWith(
                color: AppColors.textSecondary,
              ),
            ),
            const SizedBox(height: AppSpacing.md),
            SizedBox(
              width: double.infinity,
              child: OutlinedButton.icon(
                onPressed: checking ? null : onCheckNow,
                style: OutlinedButton.styleFrom(
                  foregroundColor: AppColors.roseDark,
                  side: const BorderSide(color: AppColors.roseDark),
                  shape: RoundedRectangleBorder(
                    borderRadius: BorderRadius.circular(AppRadius.lg),
                  ),
                  padding: const EdgeInsets.symmetric(vertical: 12),
                ),
                icon: checking
                    ? const SizedBox(
                        width: 16,
                        height: 16,
                        child: CircularProgressIndicator(strokeWidth: 2),
                      )
                    : const Icon(Icons.fact_check_rounded),
                label: Text(checking ? 'Provjera u toku...' : 'Provjeri sada'),
              ),
            ),
          ],
        ),
      ),
    );
  }
}

class _AlertCard extends StatelessWidget {
  final dynamic alert;
  final VoidCallback? onResolve;

  const _AlertCard({required this.alert, required this.onResolve});

  Color get _severityColor {
    switch (alert["severity"]) {
      case 3:
        return Colors.redAccent;
      case 2:
        return Colors.orange;
      default:
        return AppColors.babyBlue;
    }
  }

  IconData get _parameterIcon {
    switch (alert["parameterType"]) {
      case 1:
        return Icons.show_chart_rounded;
      case 2:
        return Icons.local_drink_rounded;
      case 3:
        return Icons.nights_stay_rounded;
      case 4:
        return Icons.baby_changing_station_rounded;
      case 5:
        return Icons.thermostat_rounded;
      default:
        return Icons.favorite_border_rounded;
    }
  }

  @override
  Widget build(BuildContext context) {
    final isResolved = alert["isResolved"] == true;
    final color = _severityColor;

    return Card(
      elevation: 0,
      shape: RoundedRectangleBorder(
        borderRadius: BorderRadius.circular(AppRadius.xl),
        side: BorderSide(color: color.withOpacity(.3)),
      ),
      child: Padding(
        padding: const EdgeInsets.all(AppSpacing.lg),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Row(
              children: [
                Container(
                  width: 40,
                  height: 40,
                  decoration: BoxDecoration(
                    color: color.withOpacity(isResolved ? .25 : 1),
                    shape: BoxShape.circle,
                  ),
                  child: Icon(_parameterIcon, color: Colors.white, size: 20),
                ),
                const SizedBox(width: AppSpacing.md),
                Expanded(
                  child: Text(
                    alert["title"] ?? "",
                    style: Theme.of(context).textTheme.titleMedium?.copyWith(
                      fontWeight: FontWeight.w700,
                      color: AppColors.textPrimary,
                    ),
                  ),
                ),
                if (isResolved)
                  const Icon(Icons.check_circle_rounded, color: Colors.green),
              ],
            ),
            const SizedBox(height: AppSpacing.sm),
            Text(
              alert["message"] ?? "",
              style: Theme.of(
                context,
              ).textTheme.bodyMedium?.copyWith(color: AppColors.textPrimary),
            ),
            const SizedBox(height: 6),
            Text(
              alert["recommendation"] ?? "",
              style: Theme.of(context).textTheme.bodyMedium?.copyWith(
                color: AppColors.textSecondary,
                fontStyle: FontStyle.italic,
              ),
            ),
            const SizedBox(height: AppSpacing.sm),
            Text(
              _formatDate(alert["detectedAt"]),
              style: Theme.of(context).textTheme.bodySmall?.copyWith(
                color: AppColors.textSecondary,
                fontSize: 11,
              ),
            ),
            if (onResolve != null) ...[
              const SizedBox(height: AppSpacing.md),
              SizedBox(
                width: double.infinity,
                child: TextButton(
                  onPressed: onResolve,
                  style: TextButton.styleFrom(foregroundColor: color),
                  child: const Text('Označi kao riješeno'),
                ),
              ),
            ],
          ],
        ),
      ),
    );
  }
}

String _formatDate(String? raw) {
  if (raw == null) return '';
  final date = DateTime.tryParse(raw);
  if (date == null) return '';

  return "${date.day}.${date.month}.${date.year} ${date.hour}:${date.minute.toString().padLeft(2, '0')}";
}

class _EmptyState extends StatelessWidget {
  const _EmptyState();

  @override
  Widget build(BuildContext context) {
    return Center(
      child: Padding(
        padding: const EdgeInsets.all(AppSpacing.xl),
        child: Card(
          elevation: 0,
          shape: RoundedRectangleBorder(
            borderRadius: BorderRadius.circular(AppRadius.xl),
          ),
          child: Padding(
            padding: const EdgeInsets.all(AppSpacing.xl),
            child: Column(
              mainAxisSize: MainAxisSize.min,
              children: const [
                Icon(
                  Icons.verified_rounded,
                  size: 60,
                  color: AppColors.babyBlue,
                ),
                SizedBox(height: 16),
                Text(
                  'Nema zabilježenih odstupanja',
                  style: TextStyle(
                    fontWeight: FontWeight.w700,
                    color: AppColors.roseDark,
                  ),
                ),
              ],
            ),
          ),
        ),
      ),
    );
  }
}
