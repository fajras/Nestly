import 'dart:convert';
import 'dart:io' show Platform;

import 'package:flutter/foundation.dart' show kIsWeb;
import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;

import 'package:flutter_application_nestly/main.dart';
import 'package:table_calendar/table_calendar.dart';

String _devBase() {
  if (kIsWeb) return 'http://localhost:5167';
  if (Platform.isAndroid) return 'http://10.0.2.2:5167';
  if (Platform.isIOS || Platform.isMacOS) return 'http://localhost:5167';
  return 'http://localhost:5167';
}

String get _apiBase =>
    const String.fromEnvironment('API_BASE', defaultValue: '').isNotEmpty
    ? const String.fromEnvironment('API_BASE')
    : _devBase();

Map<String, String> _headers() => {
  'Content-Type': 'application/json',
  'Accept': 'application/json',
};

class Therapy {
  final int id;
  final String name;
  final String dose;
  final DateTime start;
  final DateTime end;

  Therapy({
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

abstract class TherapyService {
  Future<List<Therapy>> fetchTherapies();
  Future<void> addTherapy({
    required String name,
    required String dose,
    required DateTime start,
    required DateTime end,
  });
}

class MedicationPlanApiService implements TherapyService {
  final int userId;
  final String baseUrl;

  MedicationPlanApiService({required this.userId, String? baseUrl})
    : baseUrl = baseUrl ?? _apiBase;

  String get _base => '$baseUrl/api/MedicationPlan';

  @override
  Future<List<Therapy>> fetchTherapies() async {
    final uri = Uri.parse('$_base?UserId=$userId');

    final res = await http
        .get(uri, headers: _headers())
        .timeout(const Duration(seconds: 10));

    if (res.statusCode != 200) {
      throw Exception('Greška pri učitavanju terapija (${res.statusCode}).');
    }

    final body = jsonDecode(res.body);
    if (body is! List) return [];

    return body.map<Therapy>((raw) {
      final map = raw as Map<String, dynamic>;

      final id = (map['id'] ?? map['Id']) as int;
      final medicineName = (map['medicineName'] ?? map['MedicineName'] ?? '')
          .toString();
      final dose = (map['dose'] ?? map['Dose'] ?? '').toString();

      final startStr = (map['startDate'] ?? map['StartDate'])?.toString() ?? '';
      final endStr = (map['endDate'] ?? map['EndDate'])?.toString() ?? '';

      final start = DateTime.tryParse(startStr) ?? DateTime.now();
      final end = DateTime.tryParse(endStr) ?? start;

      return Therapy(
        id: id,
        name: medicineName,
        dose: dose,
        start: start,
        end: end,
      );
    }).toList();
  }

  @override
  Future<void> addTherapy({
    required String name,
    required String dose,
    required DateTime start,
    required DateTime end,
  }) async {
    final uri = Uri.parse(_base);

    final body = jsonEncode({
      'userId': userId,
      'startDate': start.toIso8601String(),
      'endDate': end.toIso8601String(),
      'medicineName': name,
      'dose': dose,
    });

    final res = await http
        .post(uri, headers: _headers(), body: body)
        .timeout(const Duration(seconds: 10));

    if (res.statusCode != 201 && res.statusCode != 200) {
      throw Exception('Greška pri spremanju terapije (${res.statusCode}).');
    }
  }
}

class TherapyCalendarScreen extends StatefulWidget {
  const TherapyCalendarScreen({
    super.key,
    this.service,
    required this.parentProfileId,
  });

  final TherapyService? service;
  final int parentProfileId;

  @override
  State<TherapyCalendarScreen> createState() => _TherapyCalendarScreenState();
}

class _TherapyCalendarScreenState extends State<TherapyCalendarScreen> {
  late final TherapyService _service;

  DateTime _focusedDay = DateTime.now();
  DateTime? _selectedDay;
  List<Therapy> _therapies = [];
  bool _loading = true;
  String? _error;

  @override
  void initState() {
    super.initState();
    _service =
        widget.service ??
        MedicationPlanApiService(userId: widget.parentProfileId);
    _load();
  }

  Future<void> _load() async {
    setState(() {
      _loading = true;
      _error = null;
    });

    try {
      _therapies = await _service.fetchTherapies();
    } catch (e) {
      _error = e.toString();
    }

    if (mounted) {
      setState(() => _loading = false);
    }
  }

  List<Therapy> _getTherapiesForDay(DateTime day) {
    return _therapies.where((t) => t.contains(day)).toList();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppColors.bg,
      appBar: AppBar(
        backgroundColor: Colors.transparent,
        elevation: 0,
        leading: IconButton(
          icon: const Icon(
            Icons.arrow_back_ios_new_rounded,
            color: AppColors.roseDark,
          ),
          onPressed: () => Navigator.pop(context),
        ),
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
          : _error != null
          ? Center(
              child: Padding(
                padding: const EdgeInsets.all(AppSpacing.lg),
                child: Text(
                  'Greška: $_error',
                  style: const TextStyle(
                    color: Colors.red,
                    fontWeight: FontWeight.w600,
                  ),
                  textAlign: TextAlign.center,
                ),
              ),
            )
          : Column(
              children: [
                _buildCalendar(),
                _buildCalendarHint(),
                const SizedBox(height: AppSpacing.lg),
                Expanded(child: _buildDayDetails()),
              ],
            ),

      bottomNavigationBar: SafeArea(
        child: Padding(
          padding: const EdgeInsets.fromLTRB(
            AppSpacing.xl,
            0,
            AppSpacing.xl,
            AppSpacing.lg,
          ),
          child: SizedBox(
            height: 52,
            child: ElevatedButton.icon(
              onPressed: () async {
                await Navigator.of(context).push(
                  MaterialPageRoute(
                    builder: (_) => AddTherapyScreen(service: _service),
                  ),
                );
                _load();
              },
              style: ElevatedButton.styleFrom(
                foregroundColor: AppColors.card,
                backgroundColor: AppColors.roseDark,
                shape: RoundedRectangleBorder(
                  borderRadius: BorderRadius.circular(AppRadius.lg),
                ),
                elevation: 0,
                textStyle: const TextStyle(fontWeight: FontWeight.w700),
              ),
              icon: const Icon(Icons.add),
              label: const Text('Dodaj terapiju'),
            ),
          ),
        ),
      ),
    );
  }

  Widget _buildCalendar() {
    return Center(
      child: ConstrainedBox(
        constraints: const BoxConstraints(maxWidth: 520),
        child: Padding(
          padding: const EdgeInsets.all(AppSpacing.lg),
          child: TableCalendar(
            focusedDay: _focusedDay,
            firstDay: DateTime.utc(2024, 1, 1),
            lastDay: DateTime.utc(2026, 12, 31),
            locale: 'bs_BA',
            selectedDayPredicate: (day) =>
                _selectedDay != null &&
                day.year == _selectedDay!.year &&
                day.month == _selectedDay!.month &&
                day.day == _selectedDay!.day,
            calendarFormat: CalendarFormat.month,
            headerStyle: const HeaderStyle(
              formatButtonVisible: false,
              titleCentered: true,
              titleTextStyle: TextStyle(
                fontWeight: FontWeight.bold,
                color: AppColors.roseDark,
              ),
              leftChevronIcon: const Icon(
                Icons.chevron_left_rounded,
                color: AppColors.roseDark,
              ),
              rightChevronIcon: const Icon(
                Icons.chevron_right_rounded,
                color: AppColors.roseDark,
              ),
            ),
            calendarStyle: CalendarStyle(
              outsideDaysVisible: false,
              defaultDecoration: const BoxDecoration(shape: BoxShape.rectangle),
              selectedDecoration: BoxDecoration(
                color: AppColors.seed.withOpacity(0.2),
                borderRadius: BorderRadius.circular(8),
              ),
              todayDecoration: BoxDecoration(
                color: AppColors.seed.withOpacity(0.3),
                borderRadius: BorderRadius.circular(8),
              ),
            ),
            eventLoader: (day) => _getTherapiesForDay(day),
            onDaySelected: (selected, focused) {
              setState(() {
                _selectedDay = selected;
                _focusedDay = focused;
              });
            },
            calendarBuilders: CalendarBuilders(
              markerBuilder: (context, date, events) {
                if (events.isEmpty) return null;
                return const Padding(
                  padding: EdgeInsets.only(top: 30),
                  child: Icon(
                    Icons.medication_outlined,
                    color: AppColors.seed,
                    size: 18,
                  ),
                );
              },
            ),
          ),
        ),
      ),
    );
  }

  Widget _buildDayDetails() {
    if (_selectedDay == null) {
      return const Center(
        child: Text(
          'Odaberite dan u kalendaru da vidite terapije.',
          style: TextStyle(color: AppColors.textSecondary),
        ),
      );
    }

    final list = _getTherapiesForDay(_selectedDay!);

    if (list.isEmpty) {
      return Center(
        child: Column(
          mainAxisSize: MainAxisSize.min,
          children: [
            const Icon(
              Icons.medication_liquid_rounded,
              size: 32,
              color: AppColors.roseDark,
            ),
            const SizedBox(height: 8),
            const Text(
              'Nema terapije za odabrani dan.',
              style: TextStyle(color: AppColors.textSecondary),
            ),
          ],
        ),
      );
    }

    final prettyDate =
        '${_selectedDay!.day.toString().padLeft(2, '0')}.${_selectedDay!.month.toString().padLeft(2, '0')}.${_selectedDay!.year}.';

    return ListView.builder(
      padding: const EdgeInsets.symmetric(
        horizontal: AppSpacing.xl,
        vertical: AppSpacing.lg,
      ),
      itemCount: list.length + 2,
      itemBuilder: (context, i) {
        if (i == 0) {
          return Center(
            child: ConstrainedBox(
              constraints: const BoxConstraints(maxWidth: 520),
              child: Padding(
                padding: const EdgeInsets.only(bottom: AppSpacing.md),
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Row(
                      children: [
                        const Icon(
                          Icons.medication_outlined,
                          color: AppColors.roseDark,
                          size: 20,
                        ),
                        const SizedBox(width: 8),
                        Text(
                          'Terapije za $prettyDate',
                          style: Theme.of(context).textTheme.titleMedium
                              ?.copyWith(
                                color: AppColors.roseDark,
                                fontWeight: FontWeight.w800,
                              ),
                        ),
                      ],
                    ),
                    const SizedBox(height: 4),
                    const SizedBox(height: AppSpacing.md),
                  ],
                ),
              ),
            ),
          );
        }

        if (i == list.length + 1) {
          return const SizedBox(height: 90);
        }

        final t = list[i - 1];

        return Center(
          child: ConstrainedBox(
            constraints: const BoxConstraints(maxWidth: 520),
            child: Container(
              margin: const EdgeInsets.only(bottom: 12),
              padding: const EdgeInsets.all(AppSpacing.lg),
              decoration: BoxDecoration(
                color: AppColors.card,
                borderRadius: BorderRadius.circular(AppRadius.lg),
                boxShadow: [
                  BoxShadow(
                    color: Colors.black.withOpacity(0.05),
                    blurRadius: 8,
                    offset: const Offset(0, 3),
                  ),
                ],
              ),
              child: Row(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Container(
                    padding: const EdgeInsets.all(10),
                    decoration: BoxDecoration(
                      color: AppColors.babyBlue.withOpacity(.22),
                      borderRadius: BorderRadius.circular(AppRadius.md),
                    ),
                    child: const Icon(
                      Icons.local_pharmacy_rounded,
                      color: AppColors.roseDark,
                      size: 22,
                    ),
                  ),
                  const SizedBox(width: AppSpacing.md),
                  Expanded(
                    child: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        Text(
                          t.name,
                          style: Theme.of(context).textTheme.titleMedium
                              ?.copyWith(
                                color: AppColors.roseDark,
                                fontWeight: FontWeight.w800,
                              ),
                        ),
                        if (t.dose.isNotEmpty) ...[
                          const SizedBox(height: 4),
                          Text(
                            'Doza: ${t.dose}',
                            style: Theme.of(context).textTheme.bodyMedium
                                ?.copyWith(color: AppColors.textSecondary),
                          ),
                        ],
                        const SizedBox(height: 6),
                        Text(
                          'Trajanje: ${_fmt(t.start)} – ${_fmt(t.end)}',
                          style: Theme.of(context).textTheme.bodySmall
                              ?.copyWith(color: AppColors.textSecondary),
                        ),
                      ],
                    ),
                  ),
                ],
              ),
            ),
          ),
        );
      },
    );
  }

  String _fmt(DateTime d) =>
      '${d.day.toString().padLeft(2, '0')}.${d.month.toString().padLeft(2, '0')}.${d.year}.';
}

class AddTherapyScreen extends StatefulWidget {
  const AddTherapyScreen({super.key, required this.service});

  final TherapyService service;

  @override
  State<AddTherapyScreen> createState() => _AddTherapyScreenState();
}

class _AddTherapyScreenState extends State<AddTherapyScreen> {
  final _formKey = GlobalKey<FormState>();
  final _nameController = TextEditingController();
  final _doseController = TextEditingController();

  DateTime? _start;
  DateTime? _end;
  bool _saving = false;

  Future<void> _save() async {
    if (!_formKey.currentState!.validate() || _start == null || _end == null) {
      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(content: Text('Molimo unesite sve podatke.')),
      );
      return;
    }

    setState(() => _saving = true);

    try {
      await widget.service.addTherapy(
        name: _nameController.text.trim(),
        dose: _doseController.text.trim(),
        start: _start!,
        end: _end!,
      );
      if (!mounted) return;
      Navigator.of(context).pop();
    } catch (e) {
      if (!mounted) return;
      ScaffoldMessenger.of(
        context,
      ).showSnackBar(SnackBar(content: Text('Greška pri spremanju: $e')));
    } finally {
      if (mounted) setState(() => _saving = false);
    }
  }

  Future<void> _pickDate(bool start) async {
    final now = DateTime.now();
    final picked = await showDatePicker(
      context: context,
      initialDate: now,
      firstDate: DateTime(now.year - 1),
      lastDate: DateTime(now.year + 2),
      builder: (context, child) {
        return Theme(
          data: Theme.of(context).copyWith(
            colorScheme: Theme.of(context).colorScheme.copyWith(
              primary: AppColors.roseDark,
              surface: Colors.white,
            ),
          ),
          child: child!,
        );
      },
    );
    if (picked != null) {
      setState(() {
        if (start) {
          _start = picked;
          if (_end != null && _end!.isBefore(_start!)) {
            _end = _start;
          }
        } else {
          _end = picked;
        }
      });
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppColors.bg,
      appBar: AppBar(
        backgroundColor: Colors.transparent,
        elevation: 0,
        leading: IconButton(
          icon: const Icon(
            Icons.arrow_back_ios_new_rounded,
            color: AppColors.roseDark,
          ),
          onPressed: () => Navigator.pop(context),
        ),
        title: Text(
          'Dodaj terapiju',
          style: Theme.of(context).textTheme.titleLarge?.copyWith(
            fontWeight: FontWeight.w800,
            color: AppColors.roseDark,
          ),
        ),
        centerTitle: true,
      ),
      body: SingleChildScrollView(
        padding: const EdgeInsets.all(AppSpacing.xl),
        child: ConstrainedBox(
          constraints: const BoxConstraints(maxWidth: 520),
          child: Card(
            elevation: 3,
            color: AppColors.card,
            shadowColor: AppColors.babyPink.withOpacity(0.35),
            shape: RoundedRectangleBorder(
              borderRadius: BorderRadius.circular(AppRadius.xl),
            ),
            child: Padding(
              padding: const EdgeInsets.all(AppSpacing.xl),
              child: Form(
                key: _formKey,
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.stretch,
                  children: [
                    Row(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        Container(
                          padding: const EdgeInsets.all(10),
                          decoration: BoxDecoration(
                            color: AppColors.babyBlue.withOpacity(.2),
                            borderRadius: BorderRadius.circular(AppRadius.md),
                          ),
                          child: const Icon(
                            Icons.medication_rounded,
                            color: AppColors.roseDark,
                            size: 22,
                          ),
                        ),
                        const SizedBox(width: AppSpacing.md),
                        Expanded(
                          child: Column(
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: [
                              Text(
                                'Dodajte terapiju',
                                style: Theme.of(context).textTheme.titleMedium
                                    ?.copyWith(
                                      color: AppColors.roseDark,
                                      fontWeight: FontWeight.w800,
                                    ),
                              ),
                              const SizedBox(height: 4),
                              Text(
                                'Unesite naziv lijeka, dozu i period u kojem treba da se uzima.',
                                style: Theme.of(context).textTheme.bodySmall
                                    ?.copyWith(
                                      color: AppColors.textSecondary,
                                      height: 1.4,
                                    ),
                              ),
                            ],
                          ),
                        ),
                      ],
                    ),

                    const SizedBox(height: AppSpacing.xl),

                    TextFormField(
                      controller: _nameController,
                      decoration: const InputDecoration(
                        labelText: 'Naziv terapije (npr. Vitamin D)',
                        prefixIcon: Icon(Icons.medication_liquid_rounded),
                      ),
                      validator: (v) =>
                          v == null || v.isEmpty ? 'Unesite naziv.' : null,
                    ),
                    const SizedBox(height: AppSpacing.lg),
                    TextFormField(
                      controller: _doseController,
                      decoration: const InputDecoration(
                        labelText: 'Doza (npr. 1x dnevno, 400 IU)',
                        prefixIcon: Icon(Icons.numbers_rounded),
                      ),
                      validator: (v) =>
                          v == null || v.isEmpty ? 'Unesite dozu.' : null,
                    ),

                    const SizedBox(height: AppSpacing.lg),

                    Row(
                      children: [
                        Expanded(
                          child: OutlinedButton(
                            onPressed: () => _pickDate(true),
                            child: Text(
                              _start == null
                                  ? 'Početni datum'
                                  : 'Od: ${_fmt(_start!)}',
                            ),
                          ),
                        ),
                        const SizedBox(width: 12),
                        Expanded(
                          child: OutlinedButton(
                            onPressed: () => _pickDate(false),
                            child: Text(
                              _end == null
                                  ? 'Krajnji datum'
                                  : 'Do: ${_fmt(_end!)}',
                            ),
                          ),
                        ),
                      ],
                    ),

                    const SizedBox(height: AppSpacing.xl),

                    SizedBox(
                      height: 52,
                      child: ElevatedButton(
                        onPressed: _saving ? null : _save,
                        style: ElevatedButton.styleFrom(
                          backgroundColor: AppColors.roseDark,
                          shape: RoundedRectangleBorder(
                            borderRadius: BorderRadius.circular(AppRadius.lg),
                          ),
                          elevation: 0,
                          textStyle: const TextStyle(
                            fontWeight: FontWeight.w700,
                          ),
                        ),
                        child: _saving
                            ? const SizedBox(
                                height: 22,
                                width: 22,
                                child: CircularProgressIndicator(
                                  strokeWidth: 2,
                                  valueColor: AlwaysStoppedAnimation(
                                    Colors.white,
                                  ),
                                ),
                              )
                            : const Text(
                                'Unesi terapiju',
                                style: TextStyle(color: Colors.white),
                              ),
                      ),
                    ),
                  ],
                ),
              ),
            ),
          ),
        ),
      ),
    );
  }

  String _fmt(DateTime d) =>
      '${d.day.toString().padLeft(2, '0')}.${d.month.toString().padLeft(2, '0')}.${d.year}.';
}

Widget _buildCalendarHint() {
  return Center(
    child: ConstrainedBox(
      constraints: const BoxConstraints(maxWidth: 520),
      child: Padding(
        padding: const EdgeInsets.symmetric(horizontal: AppSpacing.xl),
        child: Container(
          padding: const EdgeInsets.symmetric(horizontal: 12, vertical: 10),
          decoration: BoxDecoration(
            color: AppColors.babyBlue.withOpacity(.15),
            borderRadius: BorderRadius.circular(AppRadius.md),
          ),
          child: Row(
            children: [
              const Icon(
                Icons.info_outline_rounded,
                size: 18,
                color: AppColors.roseDark,
              ),
              const SizedBox(width: 8),
              Expanded(
                child: Text(
                  'Dodirnite dan u kalendaru da vidite terapije koje važe za taj datum.',
                  style: const TextStyle(
                    color: AppColors.textSecondary,
                    fontSize: 13,
                    height: 1.3,
                    fontWeight: FontWeight.w500,
                  ),
                ),
              ),
            ],
          ),
        ),
      ),
    ),
  );
}
