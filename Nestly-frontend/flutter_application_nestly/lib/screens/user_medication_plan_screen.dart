import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:flutter_application_nestly/layouts/nestly_toast.dart';
import 'package:flutter_application_nestly/main.dart';
import 'package:flutter_application_nestly/network/api_client.dart';

class UserMedicationPlanScreen extends StatefulWidget {
  final int userId;

  const UserMedicationPlanScreen({super.key, required this.userId});

  @override
  State<UserMedicationPlanScreen> createState() =>
      _UserMedicationPlanScreenState();
}

class _UserMedicationPlanScreenState extends State<UserMedicationPlanScreen> {
  bool _loading = true;
  List<MedicationPlanRow> _plans = [];

  @override
  void initState() {
    super.initState();
    _load();
  }

  Future<void> _load() async {
    if (!mounted) return;

    setState(() => _loading = true);

    try {
      final res = await ApiClient.get(
        '/api/MedicationPlan?ParentProfileId=${widget.userId}',
      );

      if (res.statusCode != 200) {
        throw Exception('Failed to load medication plans');
      }

      final List data = jsonDecode(res.body);

      final plans = data
          .map((e) => MedicationPlanRow.fromJson(e as Map<String, dynamic>))
          .toList();

      if (!mounted) return;

      setState(() {
        _plans = plans;
      });
    } catch (_) {
      if (!mounted) return;
      NestlyToast.error(context, 'Greška pri učitavanju terapije');
    } finally {
      if (mounted) {
        setState(() => _loading = false);
      }
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppColors.bg,
      appBar: AppBar(
        backgroundColor: Colors.transparent,
        elevation: 0,
        iconTheme: const IconThemeData(color: AppColors.roseDark),
        title: Text(
          'Terapija',
          style: Theme.of(context).textTheme.titleLarge?.copyWith(
            fontWeight: FontWeight.w700,
            color: AppColors.roseDark,
          ),
        ),
        centerTitle: true,
      ),
      body: Padding(
        padding: const EdgeInsets.all(AppSpacing.xl),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Expanded(
              child: _loading
                  ? const Center(
                      child: CircularProgressIndicator(
                        color: AppColors.roseDark,
                      ),
                    )
                  : _plans.isEmpty
                  ? const _EmptyState()
                  : RefreshIndicator(
                      color: AppColors.roseDark,
                      onRefresh: _load,
                      child: ListView.separated(
                        itemCount: _plans.length,
                        separatorBuilder: (_, __) =>
                            const SizedBox(height: AppSpacing.md),
                        itemBuilder: (_, i) => _MedicationCard(plan: _plans[i]),
                      ),
                    ),
            ),
          ],
        ),
      ),
    );
  }
}

class MedicationPlanRow {
  final String medicineName;
  final String dose;
  final DateTime startDate;
  final DateTime endDate;

  const MedicationPlanRow({
    required this.medicineName,
    required this.dose,
    required this.startDate,
    required this.endDate,
  });

  factory MedicationPlanRow.fromJson(Map<String, dynamic> json) {
    return MedicationPlanRow(
      medicineName: (json['medicineName'] ?? '').toString(),
      dose: (json['dose'] ?? '').toString(),
      startDate: DateTime.parse(json['startDate']),
      endDate: DateTime.parse(json['endDate']),
    );
  }
}

class _MedicationCard extends StatelessWidget {
  final MedicationPlanRow plan;

  const _MedicationCard({required this.plan});

  @override
  Widget build(BuildContext context) {
    return Card(
      elevation: 0,
      shape: RoundedRectangleBorder(
        borderRadius: BorderRadius.circular(AppRadius.lg),
      ),
      child: Padding(
        padding: const EdgeInsets.all(AppSpacing.lg),
        child: Row(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            CircleAvatar(
              backgroundColor: AppColors.roseDark.withOpacity(.15),
              child: const Icon(
                Icons.medication_outlined,
                color: AppColors.roseDark,
              ),
            ),
            const SizedBox(width: AppSpacing.lg),
            Expanded(
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Text(
                    plan.medicineName,
                    style: const TextStyle(
                      fontSize: 16,
                      fontWeight: FontWeight.w700,
                      color: AppColors.roseDark,
                    ),
                  ),
                  const SizedBox(height: 4),
                  Text(
                    'Doza: ${plan.dose}',
                    style: const TextStyle(color: AppColors.textSecondary),
                  ),
                  const SizedBox(height: 4),
                  Text(
                    'Period: ${_fmt(plan.startDate)} – ${_fmt(plan.endDate)}',
                    style: const TextStyle(
                      fontSize: 13,
                      color: AppColors.textSecondary,
                    ),
                  ),
                ],
              ),
            ),
          ],
        ),
      ),
    );
  }
}

class _EmptyState extends StatelessWidget {
  const _EmptyState();

  @override
  Widget build(BuildContext context) {
    return Center(
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
                Icons.medication_outlined,
                size: 48,
                color: AppColors.roseDark,
              ),
              SizedBox(height: 12),
              Text(
                'Nema evidentirane terapije',
                style: TextStyle(
                  fontWeight: FontWeight.w600,
                  color: AppColors.textSecondary,
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }
}

String _fmt(DateTime d) =>
    '${d.day.toString().padLeft(2, '0')}.'
    '${d.month.toString().padLeft(2, '0')}.'
    '${d.year}.';
