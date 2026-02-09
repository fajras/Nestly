import 'dart:convert';
import 'dart:io' show Platform;

import 'package:flutter/foundation.dart' show kIsWeb;
import 'package:flutter/material.dart';
import 'package:flutter_application_nestly/layouts/nestly_toast.dart';
import 'package:flutter_application_nestly/main.dart';
import 'package:http/http.dart' as http;

String _devBase() {
  if (kIsWeb) return 'http://localhost:5167';
  if (Platform.isAndroid) return 'http://10.0.2.2:5167';
  return 'http://localhost:5167';
}

String get _apiBase => _devBase();

Map<String, String> _headers(String token) => {
  'Content-Type': 'application/json',
  'Accept': 'application/json',
  'Authorization': 'Bearer $token',
};

class UserMedicationPlanScreen extends StatefulWidget {
  final int userId;
  final String token;

  const UserMedicationPlanScreen({
    super.key,
    required this.userId,
    required this.token,
  });

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
    try {
      final res = await http.get(
        Uri.parse('$_apiBase/api/medicationplan?UserId=${widget.userId}'),
        headers: _headers(widget.token),
      );

      if (res.statusCode != 200) {
        throw Exception();
      }

      final List data = jsonDecode(res.body);
      setState(() {
        _plans = data.map((e) => MedicationPlanRow.fromJson(e)).toList();
      });
    } catch (_) {
      NestlyToast.error(context, 'Greška pri učitavanju terapije');
    } finally {
      setState(() => _loading = false);
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppColors.bg,
      body: Padding(
        padding: const EdgeInsets.all(AppSpacing.xl),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            /// HEADER
            Row(
              children: [
                IconButton(
                  icon: const Icon(Icons.arrow_back),
                  onPressed: () => Navigator.of(context).pop(),
                ),
                const SizedBox(width: 8),
                const Text(
                  'Terapija',
                  style: TextStyle(fontSize: 24, fontWeight: FontWeight.w800),
                ),
              ],
            ),

            const SizedBox(height: AppSpacing.xl),

            if (_loading)
              const Center(child: CircularProgressIndicator())
            else if (_plans.isEmpty)
              _EmptyState()
            else
              Expanded(
                child: ListView.separated(
                  itemCount: _plans.length,
                  separatorBuilder: (_, __) =>
                      const SizedBox(height: AppSpacing.md),
                  itemBuilder: (_, i) => _MedicationCard(plan: _plans[i]),
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

  MedicationPlanRow({
    required this.medicineName,
    required this.dose,
    required this.startDate,
    required this.endDate,
  });

  factory MedicationPlanRow.fromJson(Map<String, dynamic> json) {
    return MedicationPlanRow(
      medicineName: json['medicineName'],
      dose: json['dose'],
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
      child: Padding(
        padding: const EdgeInsets.all(AppSpacing.lg),
        child: Row(
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
                    style: const TextStyle(fontSize: 13),
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
  @override
  Widget build(BuildContext context) {
    return Center(
      child: Column(
        mainAxisAlignment: MainAxisAlignment.center,
        children: const [
          Icon(Icons.medication_outlined, size: 48),
          SizedBox(height: 12),
          Text(
            'Nema evidentirane terapije',
            style: TextStyle(fontWeight: FontWeight.w600),
          ),
        ],
      ),
    );
  }
}

String _fmt(DateTime d) => '${d.day}.${d.month}.${d.year}.';
