import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:flutter_application_nestly/network/api_client.dart';
import 'package:flutter_application_nestly/layouts/nestly_calendar.dart';
import 'package:flutter_application_nestly/layouts/nestly_toast.dart';
import 'package:flutter_application_nestly/main.dart';

class MedicationIntakeLog {
  final int intakeLogId;
  final int planId;
  final String medicineName;
  final String dose;
  final DateTime scheduledDate;
  final Duration intakeTime;
  final bool taken;

  MedicationIntakeLog({
    required this.intakeLogId,
    required this.planId,
    required this.medicineName,
    required this.dose,
    required this.scheduledDate,
    required this.intakeTime,
    required this.taken,
  });
}

class TherapyPlan {
  final int id;
  final String name;
  final String dose;
  final DateTime start;
  final DateTime end;
  final List<Duration> intakeTimes;

  const TherapyPlan({
    required this.id,
    required this.name,
    required this.dose,
    required this.start,
    required this.end,
    required this.intakeTimes,
  });

  bool isActiveOn(DateTime day) {
    final d = DateTime(day.year, day.month, day.day);
    final s = DateTime(start.year, start.month, start.day);
    final e = DateTime(end.year, end.month, end.day);

    return !d.isBefore(s) && !d.isAfter(e);
  }
}

/// =============================================================
/// API SERVICE
/// =============================================================

class MedicationPlanApiService {
  final int parentProfileId;

  MedicationPlanApiService(this.parentProfileId);

  Future<List<TherapyPlan>> fetchPlans() async {
    final res = await ApiClient.get(
      '/api/MedicationPlan?ParentProfileId=$parentProfileId',
    );

    if (res.statusCode != 200) {
      throw Exception('Failed to load therapies');
    }

    final List data = jsonDecode(res.body);

    return data.map((e) {
      return TherapyPlan(
        id: e['id'],
        name: e['medicineName'],
        dose: e['dose'],
        start: DateTime.parse(e['startDate']),
        end: DateTime.parse(e['endDate']),
        intakeTimes: ((e['intakeTimes'] ?? []) as List).map((t) {
          final parts = t.toString().split(':');
          return Duration(
            hours: int.parse(parts[0]),
            minutes: int.parse(parts[1]),
          );
        }).toList(),
      );
    }).toList();
  }

  Future<List<MedicationIntakeLog>> fetchLogsForDay(DateTime date) async {
    final formattedDate =
        '${date.year.toString().padLeft(4, '0')}-'
        '${date.month.toString().padLeft(2, '0')}-'
        '${date.day.toString().padLeft(2, '0')}';

    final res = await ApiClient.get(
      '/api/MedicationPlan/day?parentProfileId=$parentProfileId&date=$formattedDate',
    );

    if (res.statusCode != 200) {
      throw Exception('Failed to load day logs');
    }

    final List data = jsonDecode(res.body);

    return data.map((e) {
      final parts = e['intakeTime'].toString().split(':');

      return MedicationIntakeLog(
        intakeLogId: e['intakeLogId'],
        planId: e['planId'],
        medicineName: e['medicineName'],
        dose: e['dose'],
        scheduledDate: DateTime.parse(e['scheduledDate']),
        intakeTime: Duration(
          hours: int.parse(parts[0]),
          minutes: int.parse(parts[1]),
        ),
        taken: e['taken'],
      );
    }).toList();
  }

  Future<void> markTaken(int logId) async {
    final res = await ApiClient.post(
      '/api/MedicationPlan/mark-taken',
      body: {'intakeLogId': logId},
    );

    if (res.statusCode != 204) {
      throw Exception('Failed to mark as taken');
    }
  }

  Future<void> create({
    required String name,
    required String dose,
    required DateTime start,
    required DateTime end,
    required List<TimeOfDay> times,
  }) async {
    final res = await ApiClient.post(
      '/api/MedicationPlan',
      body: {
        'parentProfileId': parentProfileId,
        'medicineName': name,
        'dose': dose,
        'startDate': start.toIso8601String(),
        'endDate': end.toIso8601String(),
        'intakeTimes': times
            .map(
              (t) =>
                  '${t.hour.toString().padLeft(2, '0')}:${t.minute.toString().padLeft(2, '0')}:00',
            )
            .toList(),
      },
    );

    if (res.statusCode != 200 && res.statusCode != 201) {
      throw Exception('Failed to create therapy');
    }
  }
}

/// =============================================================
/// SCREEN
/// =============================================================

class TherapyCalendarScreen extends StatefulWidget {
  const TherapyCalendarScreen({super.key, required this.parentProfileId});

  final int parentProfileId;

  @override
  State<TherapyCalendarScreen> createState() => _TherapyCalendarScreenState();
}

class _TherapyCalendarScreenState extends State<TherapyCalendarScreen> {
  late final MedicationPlanApiService _service;

  DateTime _focusedDay = DateTime.now();
  DateTime? _selectedDay;
  List<TherapyPlan> _plans = [];
  List<MedicationIntakeLog> _dayLogs = [];

  bool _loading = true;

  @override
  void initState() {
    super.initState();
    _service = MedicationPlanApiService(widget.parentProfileId);

    final today = DateTime.now();
    _selectedDay = DateTime(today.year, today.month, today.day);
    _focusedDay = _selectedDay!;

    _loadPlans();
  }

  Future<void> _loadLogsForDay(DateTime day) async {
    try {
      _dayLogs = await _service.fetchLogsForDay(day);
      setState(() {});
    } catch (_) {
      NestlyToast.error(context, 'Greška pri učitavanju terapije');
    }
  }

  Future<void> _loadPlans() async {
    setState(() => _loading = true);
    try {
      _plans = await _service.fetchPlans();
    } catch (_) {
      NestlyToast.error(context, 'Greška pri učitavanju terapija');
    }
    if (mounted) setState(() => _loading = false);
  }

  DateTime _dayOnly(DateTime d) => DateTime(d.year, d.month, d.day);

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
            fontWeight: FontWeight.w800,
            color: AppColors.roseDark,
          ),
        ),
        centerTitle: true,
      ),
      body: _loading
          ? const Center(
              child: CircularProgressIndicator(color: AppColors.roseDark),
            )
          : Column(
              children: [
                NestlyCalendar(
                  focusedDay: _focusedDay,
                  selectedDay: _selectedDay,
                  markerIcon: Icons.medication_rounded,
                  lastDay: DateTime.utc(2100, 12, 31),
                  eventLoader: (day) {
                    final hasEvent = _plans.any((p) => p.isActiveOn(day));
                    return hasEvent ? [1] : [];
                  },

                  onDaySelected: (selected, focused) async {
                    final d = DateTime(
                      selected.year,
                      selected.month,
                      selected.day,
                    );

                    setState(() {
                      _selectedDay = d;
                      _focusedDay = d;
                    });

                    await _loadLogsForDay(d);
                  },
                ),
                const SizedBox(height: AppSpacing.lg),
                Expanded(child: _buildDayDetails()),
              ],
            ),
      bottomNavigationBar: _buildAddButton(),
    );
  }

  Widget _buildDayDetails() {
    if (_selectedDay == null) {
      return const Center(child: Text('Odaberite dan'));
    }

    if (_dayLogs.isEmpty) {
      return const Center(child: Text('Nema terapije za ovaj dan'));
    }

    return ListView.separated(
      padding: const EdgeInsets.all(AppSpacing.lg),
      itemCount: _dayLogs.length,
      separatorBuilder: (_, __) => const SizedBox(height: 14),
      itemBuilder: (context, index) {
        final log = _dayLogs[index];

        return Container(
          decoration: BoxDecoration(
            color: Colors.white,
            borderRadius: BorderRadius.circular(AppRadius.lg),
            boxShadow: [
              BoxShadow(
                color: Colors.black.withOpacity(.06),
                blurRadius: 10,
                offset: const Offset(0, 4),
              ),
            ],
          ),
          child: Padding(
            padding: const EdgeInsets.all(AppSpacing.lg),
            child: Row(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Checkbox(
                  value: log.taken,
                  activeColor: AppColors.roseDark,
                  onChanged: log.taken
                      ? null
                      : (_) async {
                          await _service.markTaken(log.intakeLogId);

                          NestlyToast.success(
                            context,
                            'Terapija označena kao popijena',
                          );

                          await _loadLogsForDay(_selectedDay!);
                        },
                ),
                const SizedBox(width: 10),
                Expanded(
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Text(
                        log.medicineName,
                        style: const TextStyle(
                          fontWeight: FontWeight.w800,
                          fontSize: 16,
                          color: AppColors.roseDark,
                        ),
                      ),
                      const SizedBox(height: 4),
                      Text(
                        'Doza: ${log.dose}',
                        style: const TextStyle(color: AppColors.textSecondary),
                      ),
                      const SizedBox(height: 4),
                      Text(
                        'Vrijeme: ${_formatTime(log.intakeTime)}',
                        style: const TextStyle(color: AppColors.textSecondary),
                      ),
                    ],
                  ),
                ),
              ],
            ),
          ),
        );
      },
    );
  }

  Widget _therapyCard(TherapyPlan plan, Duration time) {
    return Card(
      margin: const EdgeInsets.only(bottom: 12),
      shape: RoundedRectangleBorder(
        borderRadius: BorderRadius.circular(AppRadius.lg),
      ),
      child: Padding(
        padding: const EdgeInsets.all(AppSpacing.lg),
        child: Row(
          children: [
            const Icon(Icons.local_pharmacy_rounded, color: AppColors.roseDark),
            const SizedBox(width: 8),
            Expanded(
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Text(
                    plan.name,
                    style: const TextStyle(
                      fontWeight: FontWeight.w800,
                      color: AppColors.roseDark,
                    ),
                  ),
                  Text(
                    'Doza: ${plan.dose}',
                    style: const TextStyle(color: AppColors.textSecondary),
                  ),
                  Text(
                    'Vrijeme: ${_formatTime(time)}',
                    style: const TextStyle(color: AppColors.textSecondary),
                  ),
                ],
              ),
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildAddButton() {
    return SafeArea(
      child: Padding(
        padding: const EdgeInsets.all(AppSpacing.lg),
        child: ElevatedButton(
          style: ElevatedButton.styleFrom(
            backgroundColor: AppColors.roseDark,
            foregroundColor: Colors.white,
            minimumSize: const Size.fromHeight(52),
          ),
          onPressed: () async {
            await Navigator.of(context).push(
              MaterialPageRoute(
                builder: (_) => AddTherapyScreen(service: _service),
              ),
            );

            _loadPlans();
          },

          child: const Text('Dodaj terapiju'),
        ),
      ),
    );
  }

  String _fmt(DateTime d) =>
      '${d.day.toString().padLeft(2, '0')}.'
      '${d.month.toString().padLeft(2, '0')}.'
      '${d.year}.';
}

/// =============================================================
/// ADD / EDIT SCREEN
/// =============================================================

class AddTherapyScreen extends StatefulWidget {
  const AddTherapyScreen({super.key, required this.service});
  final MedicationPlanApiService service;

  @override
  State<AddTherapyScreen> createState() => _AddTherapyScreenState();
}

class _AddTherapyScreenState extends State<AddTherapyScreen> {
  final _name = TextEditingController();
  final _dose = TextEditingController();
  DateTime? _start;
  DateTime? _end;
  bool _saving = false;
  List<TimeOfDay> _times = [];
  Future<void> _addTime() async {
    final picked = await showTimePicker(
      context: context,
      initialTime: TimeOfDay.now(),
      builder: (context, child) {
        return Theme(
          data: Theme.of(context).copyWith(
            colorScheme: const ColorScheme.light(
              primary: AppColors.roseDark,
              onPrimary: Colors.white,
              onSurface: AppColors.textPrimary,
            ),
            timePickerTheme: TimePickerThemeData(
              backgroundColor: Colors.white,
              hourMinuteColor: AppColors.roseDark.withOpacity(.15),
              hourMinuteTextColor: AppColors.roseDark,
              dayPeriodTextColor: AppColors.roseDark,

              dialHandColor: AppColors.roseDark,
              dialBackgroundColor: AppColors.babyPink.withOpacity(.2),

              dialTextColor: MaterialStateColor.resolveWith((states) {
                if (states.contains(MaterialState.selected)) {
                  return Colors.white;
                }
                return AppColors.textPrimary;
              }),

              entryModeIconColor: AppColors.roseDark,
              confirmButtonStyle: TextButton.styleFrom(
                foregroundColor: AppColors.roseDark,
                textStyle: const TextStyle(fontWeight: FontWeight.w700),
              ),
              cancelButtonStyle: TextButton.styleFrom(
                foregroundColor: AppColors.roseDark,
              ),
            ),
          ),
          child: child!,
        );
      },
    );

    if (picked != null) {
      setState(() => _times.add(picked));
    }
  }

  Future<void> _save() async {
    if (_name.text.trim().isEmpty || _dose.text.trim().isEmpty) {
      NestlyToast.error(context, 'Popunite sva polja');
      return;
    }
    if (_times.isEmpty) {
      NestlyToast.error(context, 'Dodajte barem jedno vrijeme uzimanja');
      return;
    }

    if (_start == null || _end == null) {
      NestlyToast.error(context, 'Odaberite period terapije');
      return;
    }

    if (_end!.isBefore(_start!)) {
      NestlyToast.error(context, 'Datum završetka ne može biti prije početka');
      return;
    }

    setState(() => _saving = true);
    try {
      await widget.service.create(
        name: _name.text.trim(),
        dose: _dose.text.trim(),
        start: _start!,
        end: _end!,
        times: _times,
      );

      Navigator.pop(context);
      NestlyToast.success(context, 'Terapija dodana');
    } catch (_) {
      NestlyToast.error(context, 'Greška');
    }
    if (mounted) setState(() => _saving = false);
  }

  Future<void> _pickDate({
    required DateTime? current,
    required ValueChanged<DateTime> onPicked,
  }) async {
    final now = DateTime.now();

    final picked = await showDatePicker(
      context: context,
      initialDate: current ?? now,
      firstDate: DateTime(now.year - 1),
      lastDate: DateTime(now.year + 5),
      builder: (context, child) {
        return Theme(
          data: Theme.of(context).copyWith(
            colorScheme: ColorScheme.light(
              primary: AppColors.roseDark,
              onPrimary: Colors.white,
              onSurface: AppColors.textPrimary,
            ),
          ),
          child: child!,
        );
      },
    );

    if (picked != null) {
      onPicked(picked);
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
          'Dodaj terapiju',
          style: Theme.of(context).textTheme.titleLarge?.copyWith(
            fontWeight: FontWeight.w800,
            color: AppColors.roseDark,
          ),
        ),
      ),
      body: Padding(
        padding: const EdgeInsets.all(AppSpacing.lg),
        child: Column(
          children: [
            TextField(
              controller: _name,
              decoration: _decoration(
                label: 'Naziv terapije',
                icon: Icons.medication_rounded,
              ),
            ),
            const SizedBox(height: AppSpacing.md),

            TextField(
              controller: _dose,
              decoration: _decoration(label: 'Doza', icon: Icons.scale_rounded),
            ),
            const SizedBox(height: AppSpacing.md),

            InkWell(
              onTap: () => _pickDate(
                current: _start,
                onPicked: (d) => setState(() => _start = d),
              ),
              child: InputDecorator(
                decoration: _decoration(
                  label: 'Datum početka',
                  icon: Icons.calendar_today_rounded,
                ),
                child: Text(
                  _start == null ? 'Odaberite datum' : _fmt(_start!),
                  style: TextStyle(
                    color: _start == null
                        ? AppColors.textSecondary
                        : AppColors.textPrimary,
                  ),
                ),
              ),
            ),

            const SizedBox(height: AppSpacing.md),

            InkWell(
              onTap: () => _pickDate(
                current: _end,
                onPicked: (d) => setState(() => _end = d),
              ),
              child: InputDecorator(
                decoration: _decoration(
                  label: 'Datum završetka',
                  icon: Icons.event_rounded,
                ),
                child: Text(
                  _end == null ? 'Odaberite datum' : _fmt(_end!),
                  style: TextStyle(
                    color: _end == null
                        ? AppColors.textSecondary
                        : AppColors.textPrimary,
                  ),
                ),
              ),
            ),
            const SizedBox(height: 16),

            Align(
              alignment: Alignment.centerLeft,
              child: Text(
                'Vrijeme uzimanja',
                style: TextStyle(
                  fontWeight: FontWeight.w700,
                  color: AppColors.roseDark,
                ),
              ),
            ),

            Wrap(
              spacing: 8,
              children: _times
                  .map(
                    (t) => Chip(
                      label: Text(
                        '${t.hour.toString().padLeft(2, '0')}:${t.minute.toString().padLeft(2, '0')}',
                      ),
                      onDeleted: () => setState(() => _times.remove(t)),
                    ),
                  )
                  .toList(),
            ),

            TextButton(
              style: TextButton.styleFrom(
                foregroundColor: AppColors.roseDark,
                textStyle: const TextStyle(fontWeight: FontWeight.w700),
              ),
              onPressed: _addTime,
              child: const Text('Dodaj termin'),
            ),

            const SizedBox(height: 16),

            SizedBox(
              height: 52,
              width: double.infinity,
              child: ElevatedButton(
                onPressed: _saving ? null : _save,
                style: ElevatedButton.styleFrom(
                  backgroundColor: AppColors.roseDark,
                  foregroundColor: Colors.white,
                  elevation: 0,
                  shape: RoundedRectangleBorder(
                    borderRadius: BorderRadius.circular(AppRadius.lg),
                  ),
                  textStyle: const TextStyle(
                    fontWeight: FontWeight.w700,
                    fontSize: 16,
                  ),
                ),
                child: _saving
                    ? const SizedBox(
                        width: 20,
                        height: 20,
                        child: CircularProgressIndicator(
                          strokeWidth: 2,
                          valueColor: AlwaysStoppedAnimation<Color>(
                            Colors.white,
                          ),
                        ),
                      )
                    : const Text('Sačuvaj'),
              ),
            ),
          ],
        ),
      ),
    );
  }
}

String _fmt(DateTime d) {
  return '${d.day.toString().padLeft(2, '0')}.'
      '${d.month.toString().padLeft(2, '0')}.'
      '${d.year}.';
}

InputDecoration _decoration({required String label, required IconData icon}) {
  return InputDecoration(
    labelText: label,
    prefixIcon: Icon(icon),
    filled: true,
    fillColor: AppColors.babyPink.withOpacity(.15),
    border: OutlineInputBorder(
      borderRadius: BorderRadius.circular(AppRadius.lg),
      borderSide: BorderSide.none,
    ),
    focusedBorder: OutlineInputBorder(
      borderRadius: BorderRadius.circular(AppRadius.lg),
      borderSide: const BorderSide(color: AppColors.roseDark, width: 1.6),
    ),
    floatingLabelStyle: const TextStyle(
      color: AppColors.roseDark,
      fontWeight: FontWeight.w600,
    ),
    prefixIconColor: AppColors.roseDark,
  );
}

String _formatTime(Duration d) {
  final h = d.inHours.toString().padLeft(2, '0');
  final m = (d.inMinutes % 60).toString().padLeft(2, '0');
  return '$h:$m';
}
