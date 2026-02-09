import 'dart:convert';
import 'dart:io' show Platform;

import 'package:flutter/foundation.dart' show kIsWeb;
import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;

import 'package:flutter_application_nestly/layouts/nestly_calendar.dart';
import 'package:flutter_application_nestly/layouts/nestly_toast.dart';
import 'package:flutter_application_nestly/main.dart';

String _devBase() {
  if (kIsWeb) return 'http://localhost:5167';
  if (Platform.isAndroid) return 'http://10.0.2.2:5167';
  return 'http://localhost:5167';
}

String get _apiBase =>
    const String.fromEnvironment('API_BASE', defaultValue: '').isNotEmpty
    ? const String.fromEnvironment('API_BASE')
    : _devBase();

Map<String, String> _headers() => const {
  'Content-Type': 'application/json',
  'Accept': 'application/json',
};

class DiaperLog {
  final int id;
  final DateTime date;
  final String state;
  final String? notes;

  DiaperLog({
    required this.id,
    required this.date,
    required this.state,
    this.notes,
  });

  factory DiaperLog.fromJson(Map<String, dynamic> json) {
    return DiaperLog(
      id: json['id'],
      date: DateTime.parse(json['changeDate']),
      state: json['diaperState'],
      notes: json['notes'],
    );
  }
}

class DiaperLogApiService {
  final int babyId;

  DiaperLogApiService(this.babyId);

  String get _base => '$_apiBase/api/DiaperLog';

  Future<List<DiaperLog>> getForRange({
    required DateTime from,
    required DateTime to,
  }) async {
    final uri = Uri.parse(
      '$_base?BabyId=$babyId'
      '&DateFrom=${from.toIso8601String()}'
      '&DateTo=${to.toIso8601String()}',
    );

    final res = await http.get(uri, headers: _headers());
    if (res.statusCode != 200) throw Exception();

    final List data = jsonDecode(res.body);
    return data.map((e) => DiaperLog.fromJson(e)).toList();
  }

  Future<void> create({
    required DateTime date,
    required TimeOfDay time,
    required String state,
    String? notes,
  }) async {
    final res = await http.post(
      Uri.parse(_base),
      headers: _headers(),
      body: jsonEncode({
        'babyId': babyId,
        'changeDate': date.toIso8601String(),
        'changeTime':
            '${time.hour.toString().padLeft(2, '0')}:'
            '${time.minute.toString().padLeft(2, '0')}:00',
        'diaperState': state.toLowerCase(),
        'notes': notes,
      }),
    );

    if (res.statusCode != 200 && res.statusCode != 201) {
      throw Exception('Neuspješno spremanje');
    }
  }
}

class DiaperLogCalendarScreen extends StatefulWidget {
  final int babyId;

  const DiaperLogCalendarScreen({super.key, required this.babyId});

  @override
  State<DiaperLogCalendarScreen> createState() =>
      _DiaperLogCalendarScreenState();
}

class _DiaperLogCalendarScreenState extends State<DiaperLogCalendarScreen> {
  late final DiaperLogApiService _service;
  final _notesCtrl = TextEditingController();

  DateTime _focusedDay = DateTime.now();
  DateTime? _selectedDay;

  bool _loading = true;
  bool _saving = false;

  final Map<DateTime, List<DiaperLog>> _logsByDay = {};

  TimeOfDay _time = TimeOfDay.now();
  String _state = 'Mokra';

  @override
  void initState() {
    super.initState();
    _service = DiaperLogApiService(widget.babyId);
    _focusedDay = _dayOnly(DateTime.now());
    _loadMonth(_focusedDay);
  }

  DateTime _dayOnly(DateTime d) => DateTime(d.year, d.month, d.day);
  DateTime _monthStart(DateTime d) => DateTime(d.year, d.month, 1);
  DateTime _monthEnd(DateTime d) =>
      DateTime(d.year, d.month + 1, 0, 23, 59, 59);

  Future<void> _loadMonth(DateTime month) async {
    setState(() => _loading = true);

    try {
      final list = await _service.getForRange(
        from: _monthStart(month),
        to: _monthEnd(month),
      );

      _logsByDay.clear();
      for (final e in list) {
        final key = _dayOnly(e.date);
        _logsByDay.putIfAbsent(key, () => []).add(e);
      }
    } catch (_) {
      NestlyToast.error(context, 'Greška pri učitavanju');
    }

    if (mounted) setState(() => _loading = false);
  }

  List<DiaperLog> _forDay(DateTime d) => _logsByDay[_dayOnly(d)] ?? const [];

  Future<void> _save() async {
    if (_selectedDay == null) {
      NestlyToast.info(context, 'Odaberite dan');
      return;
    }

    setState(() => _saving = true);

    try {
      await _service.create(
        date: _selectedDay!,
        time: _time,
        state: _state,
        notes: _notesCtrl.text.trim().isEmpty ? '' : _notesCtrl.text.trim(),
      );

      _notesCtrl.clear();

      await _loadMonth(_focusedDay);
      NestlyToast.success(context, 'Zapis sačuvan');
    } catch (_) {
      NestlyToast.error(context, 'Greška pri spremanju');
    }

    if (mounted) setState(() => _saving = false);
  }

  InputDecoration _fieldDecoration({
    required String label,
    required IconData icon,
  }) {
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

  Widget _form() {
    return Card(
      shape: RoundedRectangleBorder(
        borderRadius: BorderRadius.circular(AppRadius.xl),
      ),
      child: Padding(
        padding: const EdgeInsets.all(AppSpacing.lg),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Text(
              'Novi zapis pelena',
              style: Theme.of(context).textTheme.bodyLarge?.copyWith(
                fontWeight: FontWeight.w800,
                color: AppColors.roseDark,
              ),
            ),

            const SizedBox(height: AppSpacing.md),

            ListTile(
              contentPadding: EdgeInsets.zero,
              title: const Text('Vrijeme'),
              trailing: TextButton(
                onPressed: () async {
                  final t = await showTimePicker(
                    context: context,
                    initialTime: _time,
                  );
                  if (t != null) setState(() => _time = t);
                },
                child: Text(
                  _time.format(context),
                  style: const TextStyle(
                    color: AppColors.roseDark,
                    fontWeight: FontWeight.w600,
                  ),
                ),
              ),
            ),

            const SizedBox(height: AppSpacing.md),

            DropdownButtonFormField<String>(
              value: _state,
              decoration: _fieldDecoration(
                label: 'Stanje',
                icon: Icons.baby_changing_station,
              ),
              items: const [
                DropdownMenuItem(value: 'Mokra', child: Text('Mokra')),
                DropdownMenuItem(value: 'Stolica', child: Text('Stolica')),
                DropdownMenuItem(
                  value: 'Kombinovano',
                  child: Text('Kombinovano'),
                ),
              ],
              onChanged: (v) => setState(() => _state = v!),
            ),
            const SizedBox(height: AppSpacing.md),

            TextField(
              controller: _notesCtrl,
              maxLines: 2,
              decoration: _fieldDecoration(
                label: 'Napomena',
                icon: Icons.notes_rounded,
              ),
            ),

            const SizedBox(height: AppSpacing.lg),

            SizedBox(
              width: double.infinity,
              child: ElevatedButton(
                onPressed: _saving ? null : _save,
                style: ElevatedButton.styleFrom(
                  backgroundColor: AppColors.roseDark,
                  foregroundColor: Colors.white,
                  padding: const EdgeInsets.symmetric(vertical: 14),
                  shape: RoundedRectangleBorder(
                    borderRadius: BorderRadius.circular(AppRadius.lg),
                  ),
                ),
                child: _saving
                    ? const SizedBox(
                        width: 18,
                        height: 18,
                        child: CircularProgressIndicator(
                          strokeWidth: 2,
                          valueColor: AlwaysStoppedAnimation<Color>(
                            Colors.white,
                          ),
                        ),
                      )
                    : const Text(
                        'Sačuvaj',
                        style: TextStyle(
                          fontWeight: FontWeight.w700,
                          fontSize: 16,
                        ),
                      ),
              ),
            ),
          ],
        ),
      ),
    );
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
          'Praćenje pelena',
          style: Theme.of(context).textTheme.titleLarge?.copyWith(
            fontWeight: FontWeight.w800,
            color: AppColors.roseDark,
          ),
        ),
        centerTitle: true,
      ),
      body: _loading
          ? const Center(
              child: CircularProgressIndicator(
                valueColor: AlwaysStoppedAnimation<Color>(AppColors.roseDark),
              ),
            )
          : Column(
              children: [
                NestlyCalendar(
                  focusedDay: _focusedDay,
                  selectedDay: _selectedDay,
                  accentColor: AppColors.roseDark,
                  markerIcon: Icons.baby_changing_station,
                  eventLoader: (day) => _forDay(day),
                  onDaySelected: (selected, focused) {
                    setState(() {
                      _selectedDay = _dayOnly(selected);
                      _focusedDay = _dayOnly(focused);
                    });
                  },
                ),
                const SizedBox(height: AppSpacing.lg),
                Expanded(
                  child: ListView(
                    padding: const EdgeInsets.symmetric(
                      horizontal: AppSpacing.lg,
                    ),
                    children: [
                      _form(),
                      const SizedBox(height: AppSpacing.lg),
                    ],
                  ),
                ),
              ],
            ),
    );
  }
}
