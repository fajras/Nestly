import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:flutter_application_nestly/layouts/nestly_calendar.dart';
import 'package:flutter_application_nestly/layouts/nestly_toast.dart'
    show NestlyToast;
import 'package:flutter_application_nestly/network/api_client.dart';
import 'package:flutter_application_nestly/main.dart';

class CalendarEventEntry {
  final int id;
  final int babyId;
  final int? userId;
  final String title;
  final String? description;
  final DateTime startAt;
  final DateTime? endAt;

  const CalendarEventEntry({
    required this.id,
    required this.babyId,
    this.userId,
    required this.title,
    this.description,
    required this.startAt,
    this.endAt,
  });

  factory CalendarEventEntry.fromJson(Map<String, dynamic> json) {
    return CalendarEventEntry(
      id: json['id'] as int,
      babyId: json['babyId'] as int,
      userId: json['userId'] as int?,
      title: json['title'] as String,
      description: json['description'] as String?,
      startAt: DateTime.parse(json['startAt'] as String),
      endAt: json['endAt'] != null ? DateTime.parse(json['endAt']) : null,
    );
  }
}

class CreateCalendarEventRequest {
  final int babyId;
  final int? userId;
  final String title;
  final String? description;
  final DateTime startAt;
  final DateTime? endAt;

  const CreateCalendarEventRequest({
    required this.babyId,
    this.userId,
    required this.title,
    this.description,
    required this.startAt,
    this.endAt,
  });

  Map<String, dynamic> toJson() => {
    'babyId': babyId,
    'userId': userId,
    'title': title,
    'description': description,
    'startAt': startAt.toIso8601String(),
    'endAt': endAt?.toIso8601String(),
  };
}

class CalendarEventApiService {
  Future<List<CalendarEventEntry>> getForBabyInRange({
    required int babyId,
    required DateTime from,
    required DateTime to,
  }) async {
    final resp = await ApiClient.get(
      '/api/CalendarEvent'
      '?BabyId=$babyId'
      '&From=${from.toIso8601String()}'
      '&To=${to.toIso8601String()}',
    );

    if (resp.statusCode != 200) {
      throw Exception('Failed to fetch calendar events');
    }

    final List data = jsonDecode(resp.body) as List;
    return data.map((e) => CalendarEventEntry.fromJson(e)).toList();
  }

  Future<CalendarEventEntry> update({
    required int id,
    required String title,
    String? description,
    required DateTime startAt,
  }) async {
    final res = await ApiClient.patch(
      '/api/CalendarEvent/$id',
      body: {
        'title': title,
        'description': description,
        'startAt': startAt.toIso8601String(),
      },
    );

    if (res.statusCode != 200) {
      throw Exception('Update failed');
    }

    return CalendarEventEntry.fromJson(jsonDecode(res.body));
  }

  Future<void> delete(int id) async {
    final res = await ApiClient.delete('/api/CalendarEvent/$id');

    if (res.statusCode != 204) {
      throw Exception('Delete failed');
    }
  }

  Future<CalendarEventEntry> create({
    required CreateCalendarEventRequest request,
  }) async {
    final resp = await ApiClient.post(
      '/api/CalendarEvent',
      body: request.toJson(),
    );

    if (resp.statusCode != 200 && resp.statusCode != 201) {
      throw Exception('Failed to save calendar event');
    }

    return CalendarEventEntry.fromJson(jsonDecode(resp.body));
  }
}

class CalendarEventScreen extends StatefulWidget {
  final int babyId;
  final String babyName;
  final int? userId;

  const CalendarEventScreen({
    super.key,
    required this.babyId,
    required this.babyName,
    this.userId,
  });

  @override
  State<CalendarEventScreen> createState() => _CalendarEventScreenState();
}

class _CalendarEventScreenState extends State<CalendarEventScreen> {
  final _service = CalendarEventApiService();

  bool _loading = true;
  bool _saving = false;
  CalendarEventEntry? _editingEvent;
  final Map<DateTime, List<CalendarEventEntry>> _eventsByDay = {};

  DateTime _focusedDay = DateTime.now();
  DateTime _selectedDay = DateTime.now();

  final _titleCtrl = TextEditingController();
  final _descriptionCtrl = TextEditingController();
  TimeOfDay? _timeOfDay;

  @override
  void initState() {
    super.initState();
    _selectedDay = _dateOnly(DateTime.now());
    _focusedDay = _selectedDay;
    _loadMonth(_focusedDay);
  }

  @override
  void dispose() {
    _titleCtrl.dispose();
    _descriptionCtrl.dispose();
    super.dispose();
  }

  DateTime _dateOnly(DateTime d) => DateTime(d.year, d.month, d.day);
  DateTime _monthStart(DateTime d) => DateTime(d.year, d.month, 1);
  DateTime _monthEnd(DateTime d) =>
      DateTime(d.year, d.month + 1, 0, 23, 59, 59);

  Future<void> _loadMonth(DateTime month, {bool calendarOnly = false}) async {
    if (!calendarOnly) {
      setState(() => _loading = true);
    }

    try {
      final list = await _service.getForBabyInRange(
        babyId: widget.babyId,
        from: _monthStart(month),
        to: _monthEnd(month),
      );

      if (mounted) {
        setState(() {
          _eventsByDay.clear();
          for (final ev in list) {
            final key = _dateOnly(ev.startAt);
            _eventsByDay.putIfAbsent(key, () => []).add(ev);
          }
        });
      }
    } catch (_) {
      NestlyToast.error(context, 'Greška pri učitavanju termina');
    }

    if (mounted) {
      setState(() {
        _loading = false;
      });
    }
  }

  List<CalendarEventEntry> _eventsForDay(DateTime day) =>
      _eventsByDay[_dateOnly(day)] ?? const [];

  Future<void> _pickTime() async {
    final picked = await showTimePicker(
      context: context,
      initialTime: _timeOfDay ?? TimeOfDay.now(),
    );
    if (picked != null) setState(() => _timeOfDay = picked);
  }

  Future<void> _saveEvent() async {
    if (_saving) return;

    if (_titleCtrl.text.trim().isEmpty) {
      NestlyToast.info(context, 'Naziv termina je obavezan.');
      return;
    }

    setState(() => _saving = true);

    final time = _timeOfDay ?? TimeOfDay.now();

    final startAt = DateTime(
      _selectedDay.year,
      _selectedDay.month,
      _selectedDay.day,
      time.hour,
      time.minute,
    );

    try {
      if (_editingEvent == null) {
        await _service.create(
          request: CreateCalendarEventRequest(
            babyId: widget.babyId,
            userId: widget.userId,
            title: _titleCtrl.text.trim(),
            description: _descriptionCtrl.text.trim().isEmpty
                ? null
                : _descriptionCtrl.text.trim(),
            startAt: startAt,
          ),
        );
      } else {
        await _service.update(
          id: _editingEvent!.id,
          title: _titleCtrl.text.trim(),
          description: _descriptionCtrl.text.trim().isEmpty
              ? null
              : _descriptionCtrl.text.trim(),
          startAt: startAt,
        );
      }

      _cancelEdit();
      await _loadMonth(_focusedDay);

      NestlyToast.success(
        context,
        _editingEvent == null
            ? 'Termin je uspješno sačuvan.'
            : 'Termin je ažuriran.',
        accentColor: AppColors.seed,
      );
    } catch (_) {
      NestlyToast.error(context, 'Greška pri spremanju termina.');
    }

    if (mounted) setState(() => _saving = false);
  }

  void _cancelEdit() {
    setState(() {
      _editingEvent = null;
      _titleCtrl.clear();
      _descriptionCtrl.clear();
      _timeOfDay = null;
    });
  }

  Future<void> _confirmDelete(CalendarEventEntry ev) async {
    final confirm = await showDialog<bool>(
      context: context,
      builder: (_) => AlertDialog(
        title: const Text('Obrisati termin?'),
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
      await _service.delete(ev.id);
      await _loadMonth(_focusedDay);
      NestlyToast.success(
        context,
        'Termin obrisan.',
        accentColor: AppColors.seed,
      );
    } catch (_) {
      NestlyToast.error(context, 'Greška pri brisanju.');
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppColors.bg,
      appBar: AppBar(
        backgroundColor: Colors.transparent,
        elevation: 0,
        iconTheme: const IconThemeData(color: AppColors.seed),
        title: Text(
          'Kalendar termina',
          style: Theme.of(context).textTheme.titleLarge?.copyWith(
            fontWeight: FontWeight.w700,
            color: AppColors.seed,
          ),
        ),
        centerTitle: true,
      ),
      body: _loading
          ? const Center(child: CircularProgressIndicator())
          : Padding(
              padding: const EdgeInsets.all(AppSpacing.lg),
              child: Column(
                children: [
                  _buildCalendar(),
                  const SizedBox(height: AppSpacing.lg),
                  Expanded(
                    child: SingleChildScrollView(
                      child: Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: [
                          _buildEventList(_eventsForDay(_selectedDay)),
                          const SizedBox(height: AppSpacing.lg),
                          _buildForm(),
                        ],
                      ),
                    ),
                  ),
                ],
              ),
            ),
    );
  }

  Widget _buildCalendar() {
    return NestlyCalendar(
      focusedDay: _focusedDay,
      selectedDay: _selectedDay,

      accentColor: AppColors.seed,

      markerIcon: Icons.event_note_rounded,

      eventLoader: (day) => _eventsForDay(day),

      lastDay: DateTime.now().add(const Duration(days: 365 * 5)),

      onDaySelected: (selected, focused) {
        setState(() {
          _selectedDay = _dateOnly(selected);
          _focusedDay = _dateOnly(focused);
        });
      },

      onPageChanged: (focused) {
        _focusedDay = _dateOnly(focused);
        _loadMonth(_focusedDay, calendarOnly: true);
      },
    );
  }

  Widget _buildEventList(List<CalendarEventEntry> events) {
    final label =
        '${_selectedDay.day.toString().padLeft(2, '0')}.'
        '${_selectedDay.month.toString().padLeft(2, '0')}.'
        '${_selectedDay.year}.';

    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          'Termini za $label',
          style: const TextStyle(
            fontWeight: FontWeight.w700,
            color: AppColors.seed,
          ),
        ),
        const SizedBox(height: 8),
        if (events.isEmpty)
          const Text(
            'Nema zakazanih termina za ovaj dan.',
            style: TextStyle(color: AppColors.textSecondary),
          )
        else
          Column(children: events.map(_eventTile).toList()),
      ],
    );
  }

  Widget _eventTile(CalendarEventEntry ev) {
    final time =
        '${ev.startAt.hour.toString().padLeft(2, '0')}:${ev.startAt.minute.toString().padLeft(2, '0')}';

    return Card(
      margin: const EdgeInsets.only(bottom: 8),
      shape: RoundedRectangleBorder(
        borderRadius: BorderRadius.circular(AppRadius.lg),
      ),
      child: Padding(
        padding: const EdgeInsets.all(AppSpacing.lg),
        child: Row(
          children: [
            const Icon(Icons.event_note_rounded, color: AppColors.seed),
            const SizedBox(width: AppSpacing.md),
            Expanded(
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Text(
                    ev.title,
                    style: const TextStyle(fontWeight: FontWeight.w700),
                  ),
                  Text(time, style: const TextStyle(fontSize: 12)),
                  if (ev.description?.isNotEmpty == true)
                    Padding(
                      padding: const EdgeInsets.only(top: 4),
                      child: Text(ev.description!),
                    ),
                ],
              ),
            ),
            IconButton(
              icon: const Icon(Icons.edit, color: AppColors.seed),
              onPressed: () {
                setState(() {
                  _editingEvent = ev;
                  _titleCtrl.text = ev.title;
                  _descriptionCtrl.text = ev.description ?? '';
                  _selectedDay = _dateOnly(ev.startAt);
                  _timeOfDay = TimeOfDay(
                    hour: ev.startAt.hour,
                    minute: ev.startAt.minute,
                  );
                });
              },
            ),
            IconButton(
              icon: const Icon(Icons.delete, color: Colors.red),
              onPressed: () => _confirmDelete(ev),
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildForm() {
    final timeLabel = _timeOfDay == null
        ? 'Odaberi vrijeme'
        : _timeOfDay!.format(context);

    InputDecoration deco(String label) => InputDecoration(
      labelText: label,
      filled: true,
      fillColor: Colors.white,
      border: OutlineInputBorder(
        borderRadius: BorderRadius.circular(AppRadius.lg),
      ),
    );

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
              _editingEvent == null ? 'Novi termin' : 'Uređivanje termina',
              style: TextStyle(
                fontWeight: FontWeight.w700,
                color: AppColors.seed,
              ),
            ),
            const SizedBox(height: 12),
            TextField(controller: _titleCtrl, decoration: deco('Naziv')),
            const SizedBox(height: 12),
            TextField(
              controller: _descriptionCtrl,
              maxLines: 3,
              decoration: deco('Opis (opcionalno)'),
            ),
            const SizedBox(height: 12),
            InkWell(
              onTap: _pickTime,
              child: InputDecorator(
                decoration: deco('Vrijeme termina'),
                child: Row(
                  mainAxisAlignment: MainAxisAlignment.spaceBetween,
                  children: [
                    Text(timeLabel),
                    const Icon(
                      Icons.access_time_rounded,
                      color: AppColors.babyBlue,
                    ),
                  ],
                ),
              ),
            ),
            const SizedBox(height: 16),
            SizedBox(
              width: double.infinity,
              child: ElevatedButton(
                onPressed: _saving ? null : _saveEvent,
                style: ElevatedButton.styleFrom(
                  backgroundColor: AppColors.seed,
                  foregroundColor: Colors.white,
                  padding: const EdgeInsets.symmetric(vertical: 14),
                  shape: RoundedRectangleBorder(
                    borderRadius: BorderRadius.circular(AppRadius.lg),
                  ),
                ),

                child: _saving
                    ? const CircularProgressIndicator(color: Colors.white)
                    : Text(
                        _editingEvent == null
                            ? 'Sačuvaj termin'
                            : 'Spremi promjene',
                        style: const TextStyle(fontWeight: FontWeight.w700),
                      ),
              ),
            ),
            if (_editingEvent != null) ...[
              const SizedBox(height: 12),
              SizedBox(
                width: double.infinity,
                child: OutlinedButton(
                  onPressed: _cancelEdit,
                  style: OutlinedButton.styleFrom(
                    foregroundColor: AppColors.seed,
                    side: const BorderSide(color: AppColors.seed),
                  ),
                  child: const Text(
                    'Odustani',
                    style: TextStyle(fontWeight: FontWeight.w700),
                  ),
                ),
              ),
            ],
          ],
        ),
      ),
    );
  }
}
