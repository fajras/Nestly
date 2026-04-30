import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:flutter_application_nestly/network/api_client.dart';
import 'package:flutter_application_nestly/layouts/nestly_calendar.dart';
import 'package:flutter_application_nestly/layouts/nestly_toast.dart';
import 'package:flutter_application_nestly/main.dart';

class DiaperLog {
  final int id;
  final DateTime date;
  final Duration time;
  final String state;
  final String? notes;

  DiaperLog({
    required this.id,
    required this.date,
    required this.time,
    required this.state,
    this.notes,
  });

  factory DiaperLog.fromJson(Map<String, dynamic> json) {
    final timeParts = json['changeTime'].toString().split(':');

    final rawState = json['diaperState']?.toString().toLowerCase();

    String formattedState;
    switch (rawState) {
      case 'mokra':
        formattedState = 'Mokra';
        break;
      case 'stolica':
        formattedState = 'Stolica';
        break;
      case 'kombinovano':
        formattedState = 'Kombinovano';
        break;
      default:
        formattedState = 'Mokra';
    }

    return DiaperLog(
      id: json['id'],
      date: DateTime.parse(json['changeDate']),
      time: Duration(
        hours: int.parse(timeParts[0]),
        minutes: int.parse(timeParts[1]),
      ),
      state: formattedState,
      notes: json['notes'],
    );
  }
}

class DiaperLogApiService {
  final int babyId;

  DiaperLogApiService(this.babyId);
  Future<void> update({
    required int id,
    required DateTime date,
    required TimeOfDay time,
    required String state,
    String? notes,
  }) async {
    final body = {
      'changeDate': date.toIso8601String(),
      'changeTime':
          '${time.hour.toString().padLeft(2, '0')}:${time.minute.toString().padLeft(2, '0')}:00',
      'diaperState': state.toLowerCase(),
      'notes': notes ?? '',
    };

    final res = await ApiClient.patch('/api/DiaperLog/$id', body: body);

    if (res.statusCode != 200) {
      throw Exception('Update failed');
    }
  }

  Future<void> delete(int id) async {
    final res = await ApiClient.delete('/api/DiaperLog/$id');

    if (res.statusCode != 204) {
      throw Exception('Delete failed');
    }
  }

  Future<List<DiaperLog>> getForRange({
    required DateTime from,
    required DateTime to,
  }) async {
    int page = 1;
    const pageSize = 100;

    List<DiaperLog> result = [];

    while (true) {
      final res = await ApiClient.get(
        '/api/DiaperLog'
        '?BabyId=$babyId'
        '&DateFrom=${from.toIso8601String()}'
        '&DateTo=${to.toIso8601String()}'
        '&page=$page&pageSize=$pageSize',
      );

      if (res.statusCode != 200) {
        throw Exception('Failed to fetch diaper logs');
      }

      final data = jsonDecode(res.body);
      final List items = data['items'];

      if (items.isEmpty) break;

      final parsed = items
          .map<Map<String, dynamic>>((e) => e as Map<String, dynamic>)
          .map(DiaperLog.fromJson)
          .toList();

      result.addAll(parsed);

      if (items.length < pageSize) break;

      page++;
    }

    return result;
  }

  Future<void> create({
    required DateTime date,
    required TimeOfDay time,
    required String state,
    String? notes,
  }) async {
    final body = <String, dynamic>{
      'babyId': babyId,
      'changeDate': date.toIso8601String(),
      'changeTime':
          '${time.hour.toString().padLeft(2, '0')}:'
          '${time.minute.toString().padLeft(2, '0')}:00',
      'diaperState': state.toLowerCase(),
      'notes': (notes == null || notes.trim().isEmpty) ? '' : notes.trim(),
    };

    final res = await ApiClient.post('/api/DiaperLog', body: body);

    if (res.statusCode != 200 && res.statusCode != 201) {
      throw Exception('Failed to create diaper log');
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
  DiaperLog? _editingLog;
  DateTime _focusedDay = DateTime.now();
  DateTime? _selectedDay;

  bool _loading = true;
  bool _saving = false;

  final Map<DateTime, List<DiaperLog>> _logsByDay = {};

  TimeOfDay _time = TimeOfDay.now();
  String _state = 'Mokra';
  bool _validate() {
    if (_selectedDay == null) {
      NestlyToast.info(context, 'Odaberite dan');
      return false;
    }

    return true;
  }

  @override
  void dispose() {
    _notesCtrl.dispose();
    super.dispose();
  }

  @override
  void initState() {
    super.initState();
    _service = DiaperLogApiService(widget.babyId);
    _focusedDay = _dayOnly(DateTime.now());
    _selectedDay = _focusedDay;
    _loadMonth(_focusedDay);
  }

  Widget _buildLogsForSelectedDay() {
    if (_selectedDay == null) {
      return const Text(
        'Odaberite datum',
        style: TextStyle(color: AppColors.textSecondary),
      );
    }

    final logs = _forDay(_selectedDay!);

    final label =
        '${_selectedDay!.day.toString().padLeft(2, '0')}.'
        '${_selectedDay!.month.toString().padLeft(2, '0')}.'
        '${_selectedDay!.year}.';

    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          'Zapisi za $label',
          style: const TextStyle(
            fontWeight: FontWeight.w700,
            color: AppColors.roseDark,
          ),
        ),
        const SizedBox(height: 8),
        if (logs.isEmpty)
          const Text(
            'Nema zapisa za ovaj datum.',
            style: TextStyle(color: AppColors.textSecondary),
          )
        else
          Column(children: logs.map(_logTile).toList()),
      ],
    );
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

      if (mounted) {
        setState(() {
          _logsByDay.clear();
          for (final e in list) {
            final key = _dayOnly(e.date);
            _logsByDay.putIfAbsent(key, () => []).add(e);
          }
        });
      }
    } catch (_) {
      if (!mounted) return;
      NestlyToast.error(context, 'Greška pri učitavanju');
    }

    if (mounted) setState(() => _loading = false);
  }

  List<DiaperLog> _forDay(DateTime d) => _logsByDay[_dayOnly(d)] ?? const [];

  Future<void> _save() async {
    if (!_validate()) return;

    setState(() => _saving = true);

    try {
      if (_editingLog == null) {
        await _service.create(
          date: _selectedDay!,
          time: _time,
          state: _state,
          notes: _notesCtrl.text,
        );

        NestlyToast.success(context, 'Zapis sačuvan');
      } else {
        await _service.update(
          id: _editingLog!.id,
          date: _selectedDay!,
          time: _time,
          state: _state,
          notes: _notesCtrl.text,
        );

        NestlyToast.success(context, 'Zapis ažuriran');
      }

      _cancelEdit();
      await _loadMonth(_focusedDay);
    } catch (_) {
      if (!mounted) return;
      NestlyToast.error(context, 'Greška pri spremanju');
    }

    if (mounted) setState(() => _saving = false);
  }

  void _cancelEdit() {
    setState(() {
      _editingLog = null;
      _notesCtrl.clear();
      _state = 'Mokra';
      _time = TimeOfDay.now();
    });
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
              _editingLog == null ? 'Novi zapis pelena' : 'Uređivanje zapisa',
              style: Theme.of(context).textTheme.bodyLarge?.copyWith(
                fontWeight: FontWeight.w700,
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
                    builder: (context, child) {
                      return Theme(
                        data: Theme.of(context).copyWith(
                          colorScheme: const ColorScheme.light(
                            primary: AppColors.roseDark,
                            onPrimary: Colors.white,
                            onSurface: AppColors.roseDark,
                          ),
                          timePickerTheme: const TimePickerThemeData(
                            hourMinuteTextColor: AppColors.roseDark,
                            dialHandColor: AppColors.roseDark,
                            dialTextColor: AppColors.roseDark,
                          ),
                        ),
                        child: child!,
                      );
                    },
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
              cursorColor: AppColors.roseDark,
              maxLines: 2,
              decoration: _fieldDecoration(
                label: 'Napomena (opcionalno)',
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
                    : Text(
                        _editingLog == null ? 'Sačuvaj' : 'Spremi promjene',
                        style: const TextStyle(
                          fontWeight: FontWeight.w700,
                          fontSize: 16,
                        ),
                      ),
              ),
            ),
            if (_editingLog != null) ...[
              const SizedBox(height: 12),
              SizedBox(
                width: double.infinity,
                child: OutlinedButton(
                  onPressed: _cancelEdit,
                  style: OutlinedButton.styleFrom(
                    foregroundColor: AppColors.roseDark,
                    side: const BorderSide(color: AppColors.roseDark),
                  ),
                  child: const Text(
                    'Odustani',
                    style: TextStyle(fontWeight: FontWeight.w700),
                  ),
                ),
              ),
            ],
            const SizedBox(height: AppSpacing.md),
          ],
        ),
      ),
    );
  }

  Future<void> _confirmDelete(DiaperLog log) async {
    final confirm = await showDialog<bool>(
      context: context,
      builder: (_) => AlertDialog(
        title: const Text('Obrisati zapis?'),
        content: const Text('Ova akcija je nepovratna.'),
        actions: [
          TextButton(
            onPressed: () => Navigator.pop(context, false),
            child: const Text('Odustani'),
          ),
          TextButton(
            onPressed: () => Navigator.pop(context, true),
            child: const Text('Obriši', style: TextStyle(color: Colors.red)),
          ),
        ],
      ),
    );

    if (confirm != true) return;

    try {
      await _service.delete(log.id);
      await _loadMonth(_focusedDay);
      NestlyToast.success(context, 'Zapis obrisan');
    } catch (_) {
      if (!mounted) return;
      NestlyToast.error(context, 'Greška pri brisanju');
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
          'Praćenje pelena',
          style: Theme.of(context).textTheme.titleLarge?.copyWith(
            fontWeight: FontWeight.w700,
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
                  onPageChanged: (focused) {
                    setState(() {
                      _focusedDay = _dayOnly(focused);
                    });
                    _loadMonth(_focusedDay);
                  },
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
                      _buildLogsForSelectedDay(),
                      const SizedBox(height: AppSpacing.lg),
                      _form(),
                      const SizedBox(height: AppSpacing.lg),
                    ],
                  ),
                ),
              ],
            ),
    );
  }

  Widget _logTile(DiaperLog log) {
    final timeStr =
        '${log.time.inHours.toString().padLeft(2, '0')}:${(log.time.inMinutes % 60).toString().padLeft(2, '0')}';

    return Card(
      margin: const EdgeInsets.only(bottom: 8),
      shape: RoundedRectangleBorder(
        borderRadius: BorderRadius.circular(AppRadius.lg),
      ),
      child: Padding(
        padding: const EdgeInsets.all(AppSpacing.lg),
        child: Row(
          children: [
            const Icon(Icons.baby_changing_station, color: AppColors.roseDark),
            const SizedBox(width: AppSpacing.md),
            Expanded(
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Text(
                    '$timeStr  •  ${log.state}',
                    style: const TextStyle(fontWeight: FontWeight.w700),
                  ),
                  if (log.notes != null && log.notes!.isNotEmpty)
                    Padding(
                      padding: const EdgeInsets.only(top: 4),
                      child: Text(log.notes!),
                    ),
                ],
              ),
            ),
            IconButton(
              icon: const Icon(Icons.edit, color: AppColors.roseDark),
              onPressed: () {
                setState(() {
                  _editingLog = log;
                  _selectedDay = _dayOnly(log.date);
                  _time = TimeOfDay(
                    hour: log.time.inHours,
                    minute: log.time.inMinutes % 60,
                  );
                  _state = log.state;
                  _notesCtrl.text = log.notes ?? '';
                });
              },
            ),
            IconButton(
              icon: const Icon(Icons.delete, color: Colors.red),
              onPressed: () => _confirmDelete(log),
            ),
          ],
        ),
      ),
    );
  }
}
