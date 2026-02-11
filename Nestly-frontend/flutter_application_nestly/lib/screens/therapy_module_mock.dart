import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:flutter_application_nestly/network/api_client.dart';
import 'package:flutter_application_nestly/layouts/nestly_calendar.dart';
import 'package:flutter_application_nestly/layouts/nestly_toast.dart';
import 'package:flutter_application_nestly/main.dart';

class Therapy {
  final int id;
  final String name;
  final String dose;
  final DateTime start;
  final DateTime end;

  const Therapy({
    required this.id,
    required this.name,
    required this.dose,
    required this.start,
    required this.end,
  });

  bool contains(DateTime date) {
    final d = DateTime(date.year, date.month, date.day);
    final s = DateTime(start.year, start.month, start.day);
    final e = DateTime(end.year, end.month, end.day);

    return (d.isAtSameMomentAs(s) || d.isAfter(s)) &&
        (d.isAtSameMomentAs(e) || d.isBefore(e));
  }
}

/// =============================================================
/// API SERVICE
/// =============================================================

class MedicationPlanApiService {
  final int userId;

  MedicationPlanApiService(this.userId);

  Future<List<Therapy>> fetchTherapies() async {
    final res = await ApiClient.get('/api/MedicationPlan?UserId=$userId');

    if (res.statusCode != 200) {
      throw Exception('Failed to load therapies');
    }

    final List data = jsonDecode(res.body);
    return data.map((e) {
      return Therapy(
        id: e['id'],
        name: e['medicineName'],
        dose: e['dose'],
        start: DateTime.parse(e['startDate']),
        end: DateTime.parse(e['endDate']),
      );
    }).toList();
  }

  Future<void> create({
    required String name,
    required String dose,
    required DateTime start,
    required DateTime end,
  }) async {
    final res = await ApiClient.post(
      '/api/MedicationPlan',
      body: {
        'userId': userId,
        'medicineName': name,
        'dose': dose,
        'startDate': start.toIso8601String(),
        'endDate': end.toIso8601String(),
      },
    );

    if (res.statusCode != 200 && res.statusCode != 201) {
      throw Exception('Failed to create therapy');
    }
  }

  Future<void> update(
    int id, {
    String? name,
    String? dose,
    DateTime? start,
    DateTime? end,
  }) async {
    final res = await ApiClient.patch(
      '/api/MedicationPlan/$id',
      body: {
        if (name != null) 'medicineName': name,
        if (dose != null) 'dose': dose,
        if (start != null) 'startDate': start.toIso8601String(),
        if (end != null) 'endDate': end.toIso8601String(),
      },
    );

    if (res.statusCode != 200) {
      throw Exception('Failed to update therapy');
    }
  }

  Future<void> delete(int id) async {
    final res = await ApiClient.delete('/api/MedicationPlan/$id');

    if (res.statusCode != 204) {
      throw Exception('Failed to delete therapy');
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

  bool _loading = true;
  List<Therapy> _therapies = [];

  @override
  void initState() {
    super.initState();
    _service = MedicationPlanApiService(widget.parentProfileId);
    _load();
  }

  Future<void> _load() async {
    setState(() => _loading = true);
    try {
      _therapies = await _service.fetchTherapies();
    } catch (_) {
      NestlyToast.error(context, 'Greška pri učitavanju terapija');
    }
    if (mounted) setState(() => _loading = false);
  }

  List<Therapy> _forDay(DateTime day) =>
      _therapies.where((t) => t.contains(day)).toList();

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
          ? const Center(child: CircularProgressIndicator())
          : Column(
              children: [
                NestlyCalendar(
                  focusedDay: _focusedDay,
                  selectedDay: _selectedDay,
                  markerIcon: Icons.medication_rounded,
                  eventLoader: (day) => _forDay(day),
                  onDaySelected: (selected, focused) {
                    setState(() {
                      _selectedDay = _dayOnly(selected);
                      _focusedDay = _dayOnly(focused);
                    });
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
      return const Center(
        child: Text(
          'Odaberite dan u kalendaru.',
          style: TextStyle(color: AppColors.textSecondary),
        ),
      );
    }

    final list = _forDay(_selectedDay!);
    if (list.isEmpty) {
      return const Center(
        child: Text(
          'Nema terapije za odabrani dan.',
          style: TextStyle(color: AppColors.textSecondary),
        ),
      );
    }

    return ListView(
      padding: const EdgeInsets.all(AppSpacing.lg),
      children: list.map(_therapyCard).toList(),
    );
  }

  Widget _therapyCard(Therapy t) {
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
            const SizedBox(width: AppSpacing.md),
            Expanded(
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Text(
                    t.name,
                    style: const TextStyle(
                      fontWeight: FontWeight.w800,
                      color: AppColors.roseDark,
                    ),
                  ),
                  Text(
                    'Doza: ${t.dose}',
                    style: const TextStyle(color: AppColors.textSecondary),
                  ),
                  Text(
                    '${_fmt(t.start)} – ${_fmt(t.end)}',
                    style: const TextStyle(
                      fontSize: 12,
                      color: AppColors.textSecondary,
                    ),
                  ),
                ],
              ),
            ),
            PopupMenuButton<String>(
              onSelected: (v) async {
                if (v == 'delete') {
                  await _service.delete(t.id);
                  _load();
                }
              },
              itemBuilder: (_) => const [
                PopupMenuItem(value: 'delete', child: Text('Obriši')),
              ],
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
            _load();
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

  Future<void> _save() async {
    if (_name.text.trim().isEmpty || _dose.text.trim().isEmpty) {
      NestlyToast.error(context, 'Popunite sva polja');
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
