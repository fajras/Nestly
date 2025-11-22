import 'dart:convert';
import 'dart:io' show Platform;

import 'package:flutter/foundation.dart' show kIsWeb;
import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;
import 'package:table_calendar/table_calendar.dart';

import 'package:flutter_application_nestly/main.dart';

class CalendarEventEntry {
  final int id;
  final int babyId;
  final int? userId;
  final String title;
  final String? description;
  final DateTime startAt;
  final DateTime? endAt;

  CalendarEventEntry({
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
      endAt: json['endAt'] != null
          ? DateTime.parse(json['endAt'] as String)
          : null,
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

  CreateCalendarEventRequest({
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

Map<String, String> _headers([String? token]) => {
  'Content-Type': 'application/json',
  'Accept': 'application/json',
  if (token != null) 'Authorization': 'Bearer $token',
};

class CalendarEventApiService {
  String get _baseUrl => '$_apiBase/api/CalendarEvent';

  Future<List<CalendarEventEntry>> getForBabyInRange({
    required int babyId,
    required DateTime from,
    required DateTime to,
    String? token,
  }) async {
    final uri = Uri.parse(
      '$_baseUrl?BabyId=$babyId'
      '&From=${from.toIso8601String()}'
      '&To=${to.toIso8601String()}',
    );

    final resp = await http.get(uri, headers: _headers(token));
    if (resp.statusCode != 200) {
      throw Exception('Greška pri dohvaćanju termina');
    }

    final List<dynamic> data = jsonDecode(resp.body) as List<dynamic>;
    return data
        .map((e) => CalendarEventEntry.fromJson(e as Map<String, dynamic>))
        .toList();
  }

  Future<CalendarEventEntry> create({
    required CreateCalendarEventRequest request,
    String? token,
  }) async {
    final uri = Uri.parse(_baseUrl);
    final resp = await http.post(
      uri,
      headers: _headers(token),
      body: jsonEncode(request.toJson()),
    );

    if (resp.statusCode != 201 && resp.statusCode != 200) {
      throw Exception('Greška pri spremanju termina');
    }

    final Map<String, dynamic> data =
        jsonDecode(resp.body) as Map<String, dynamic>;
    return CalendarEventEntry.fromJson(data);
  }
}

class CalendarEventScreen extends StatefulWidget {
  final int babyId;
  final String babyName;
  final String? token;
  final int? userId;

  const CalendarEventScreen({
    super.key,
    required this.babyId,
    required this.babyName,
    this.token,
    this.userId,
  });

  @override
  State<CalendarEventScreen> createState() => _CalendarEventScreenState();
}

class _CalendarEventScreenState extends State<CalendarEventScreen> {
  final _service = CalendarEventApiService();

  bool _isLoading = true;
  bool _isSaving = false;

  List<CalendarEventEntry> _events = [];

  final Map<DateTime, List<CalendarEventEntry>> _eventsByDay = {};

  DateTime _focusedDay = DateTime.now();
  DateTime _selectedDay = DateTime.now();

  final _titleCtrl = TextEditingController();
  final _descriptionCtrl = TextEditingController();
  TimeOfDay? _timeOfDay;

  @override
  void initState() {
    super.initState();
    _selectedDay = DateTime(
      DateTime.now().year,
      DateTime.now().month,
      DateTime.now().day,
    );
    _focusedDay = _selectedDay;
    _loadEventsForMonth(_focusedDay);
  }

  @override
  void dispose() {
    _titleCtrl.dispose();
    _descriptionCtrl.dispose();
    super.dispose();
  }

  DateTime _firstDayOfMonth(DateTime date) =>
      DateTime(date.year, date.month, 1);

  DateTime _lastDayOfMonth(DateTime date) =>
      DateTime(date.year, date.month + 1, 0, 23, 59, 59);

  DateTime _dateOnly(DateTime dt) => DateTime(dt.year, dt.month, dt.day);

  Future<void> _loadEventsForMonth(DateTime month) async {
    setState(() => _isLoading = true);
    try {
      final from = _firstDayOfMonth(month);
      final to = _lastDayOfMonth(month);

      final list = await _service.getForBabyInRange(
        babyId: widget.babyId,
        from: from,
        to: to,
        token: widget.token,
      );

      _events = list;
      _eventsByDay.clear();
      for (final ev in list) {
        final key = _dateOnly(ev.startAt);
        _eventsByDay.putIfAbsent(key, () => []);
        _eventsByDay[key]!.add(ev);
      }

      setState(() => _isLoading = false);
    } catch (e) {
      setState(() => _isLoading = false);
      if (!mounted) return;
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Greška pri učitavanju termina: $e')),
      );
    }
  }

  List<CalendarEventEntry> _getEventsForDay(DateTime day) {
    return _eventsByDay[_dateOnly(day)] ?? [];
  }

  Future<void> _pickTime() async {
    final picked = await showTimePicker(
      context: context,
      initialTime: _timeOfDay ?? TimeOfDay.now(),
    );
    if (picked != null) {
      setState(() => _timeOfDay = picked);
    }
  }

  Future<void> _saveEvent() async {
    if (_titleCtrl.text.trim().isEmpty) {
      ScaffoldMessenger.of(
        context,
      ).showSnackBar(const SnackBar(content: Text('Unesite naziv termina.')));
      return;
    }

    setState(() => _isSaving = true);

    final time = _timeOfDay ?? const TimeOfDay(hour: 10, minute: 0);
    final startAt = DateTime(
      _selectedDay.year,
      _selectedDay.month,
      _selectedDay.day,
      time.hour,
      time.minute,
    );

    try {
      await _service.create(
        request: CreateCalendarEventRequest(
          babyId: widget.babyId,
          userId: widget.userId,
          title: _titleCtrl.text.trim(),
          description: _descriptionCtrl.text.trim().isEmpty
              ? null
              : _descriptionCtrl.text.trim(),
          startAt: startAt,
          endAt: null,
        ),
        token: widget.token,
      );

      if (!mounted) return;

      _titleCtrl.clear();
      _descriptionCtrl.clear();
      _timeOfDay = null;

      await _loadEventsForMonth(_focusedDay);

      ScaffoldMessenger.of(
        context,
      ).showSnackBar(const SnackBar(content: Text('Termin je sačuvan.')));
    } catch (e) {
      if (!mounted) return;
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Greška pri spremanju termina: $e')),
      );
    } finally {
      if (mounted) setState(() => _isSaving = false);
    }
  }

  InputDecoration _fieldDecoration(String label) => InputDecoration(
    labelText: label,
    filled: true,
    fillColor: Colors.white,
    border: OutlineInputBorder(borderRadius: BorderRadius.circular(14)),
    enabledBorder: OutlineInputBorder(
      borderRadius: BorderRadius.circular(14),
      borderSide: BorderSide(color: AppColors.babyBlue.withOpacity(0.35)),
    ),
  );

  @override
  Widget build(BuildContext context) {
    final selectedEvents = _getEventsForDay(_selectedDay);

    return Scaffold(
      backgroundColor: AppColors.bg,
      appBar: AppBar(
        backgroundColor: Colors.transparent,
        elevation: 0,
        leading: IconButton(
          icon: const Icon(
            Icons.arrow_back_ios_new_rounded,
            color: AppColors.seed,
          ),
          onPressed: () => Navigator.of(context).pop(),
        ),
        centerTitle: true,
        title: Text(
          "Kalendar termina",
          style: Theme.of(context).textTheme.titleMedium?.copyWith(
            fontWeight: FontWeight.w800,
            color: AppColors.seed,
          ),
        ),
      ),
      body: _isLoading
          ? const Center(child: CircularProgressIndicator())
          : Padding(
              padding: const EdgeInsets.fromLTRB(16, 0, 16, 16),
              child: Column(
                children: [
                  _buildCalendar(),
                  const SizedBox(height: 12),
                  Expanded(
                    child: SingleChildScrollView(
                      child: Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: [
                          _buildEventList(selectedEvents),
                          const SizedBox(height: 16),
                          _buildFormCard(),
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
    return Container(
      decoration: BoxDecoration(
        color: Colors.white,
        borderRadius: BorderRadius.circular(24),
        boxShadow: [
          BoxShadow(
            color: Colors.black12.withOpacity(0.05),
            blurRadius: 8,
            offset: const Offset(0, 3),
          ),
        ],
      ),
      child: TableCalendar<CalendarEventEntry>(
        firstDay: DateTime.utc(2020, 1, 1),
        lastDay: DateTime.utc(2100, 12, 31),
        focusedDay: _focusedDay,
        calendarFormat: CalendarFormat.month,
        startingDayOfWeek: StartingDayOfWeek.monday,
        selectedDayPredicate: (day) => isSameDay(day, _selectedDay),
        eventLoader: _getEventsForDay,
        headerStyle: HeaderStyle(
          formatButtonVisible: false,
          titleCentered: true,
          titleTextStyle: const TextStyle(
            fontWeight: FontWeight.w700,
            fontSize: 18,
            color: AppColors.seed,
          ),
          leftChevronIcon: const Icon(
            Icons.chevron_left_rounded,
            color: AppColors.seed,
          ),
          rightChevronIcon: const Icon(
            Icons.chevron_right_rounded,
            color: AppColors.seed,
          ),
        ),
        calendarStyle: CalendarStyle(
          todayDecoration: BoxDecoration(
            color: AppColors.babyPink.withOpacity(0.9),
            shape: BoxShape.circle,
          ),
          selectedDecoration: const BoxDecoration(
            color: AppColors.babyBlue,
            shape: BoxShape.circle,
          ),
          selectedTextStyle: const TextStyle(color: Colors.white),
          todayTextStyle: const TextStyle(color: Colors.white),
          markerDecoration: const BoxDecoration(
            color: AppColors.seed,
            shape: BoxShape.circle,
          ),
          markersMaxCount: 3,
        ),
        daysOfWeekStyle: const DaysOfWeekStyle(
          weekdayStyle: TextStyle(
            fontWeight: FontWeight.w600,
            color: AppColors.textSecondary,
          ),
          weekendStyle: TextStyle(
            fontWeight: FontWeight.w600,
            color: AppColors.textSecondary,
          ),
        ),
        onDaySelected: (selectedDay, focusedDay) {
          setState(() {
            _selectedDay = _dateOnly(selectedDay);
            _focusedDay = focusedDay;
          });
        },
        onPageChanged: (focusedDay) {
          _focusedDay = focusedDay;
          _loadEventsForMonth(focusedDay);
        },
      ),
    );
  }

  Widget _buildEventList(List<CalendarEventEntry> events) {
    final dateLabel =
        "${_selectedDay.day.toString().padLeft(2, '0')}.${_selectedDay.month.toString().padLeft(2, '0')}.${_selectedDay.year}.";

    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          "Termini za $dateLabel",
          style: Theme.of(context).textTheme.bodyMedium?.copyWith(
            fontWeight: FontWeight.w700,
            color: AppColors.seed,
          ),
        ),
        const SizedBox(height: 8),
        if (events.isEmpty)
          const Text(
            "Nema zakazanih termina za ovaj dan.",
            style: TextStyle(fontSize: 13),
          )
        else
          Column(children: events.map((e) => _eventTile(e)).toList()),
      ],
    );
  }

  Widget _eventTile(CalendarEventEntry ev) {
    final timeText =
        "${ev.startAt.hour.toString().padLeft(2, '0')}:${ev.startAt.minute.toString().padLeft(2, '0')}";

    return Container(
      margin: const EdgeInsets.only(bottom: 8),
      padding: const EdgeInsets.all(12),
      decoration: BoxDecoration(
        color: Colors.white,
        borderRadius: BorderRadius.circular(16),
        border: Border.all(color: AppColors.babyBlue.withOpacity(0.25)),
      ),
      child: Row(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          const Icon(
            Icons.event_note_rounded,
            color: AppColors.babyBlue,
            size: 22,
          ),
          const SizedBox(width: 10),
          Expanded(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(
                  ev.title,
                  style: const TextStyle(
                    fontWeight: FontWeight.w700,
                    fontSize: 14,
                  ),
                ),
                const SizedBox(height: 2),
                Text(
                  timeText,
                  style: const TextStyle(
                    fontSize: 12,
                    color: AppColors.textSecondary,
                  ),
                ),
                if (ev.description != null && ev.description!.isNotEmpty)
                  Padding(
                    padding: const EdgeInsets.only(top: 4),
                    child: Text(
                      ev.description!,
                      style: const TextStyle(fontSize: 13),
                    ),
                  ),
              ],
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildFormCard() {
    final timeLabel = _timeOfDay == null
        ? "Odaberi vrijeme"
        : _timeOfDay!.format(context);

    return Container(
      padding: const EdgeInsets.all(16),
      decoration: BoxDecoration(
        color: AppColors.bg.withOpacity(0.9),
        borderRadius: BorderRadius.circular(22),
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Text(
            "Novi termin",
            style: Theme.of(context).textTheme.bodyMedium?.copyWith(
              fontWeight: FontWeight.w700,
              color: AppColors.seed,
            ),
          ),
          const SizedBox(height: 10),
          TextField(
            controller: _titleCtrl,
            decoration: _fieldDecoration("Naziv (npr. Vakcina)"),
          ),
          const SizedBox(height: 12),
          TextField(
            controller: _descriptionCtrl,
            maxLines: 3,
            decoration: _fieldDecoration("Opis (opcionalno)"),
          ),
          const SizedBox(height: 12),
          InkWell(
            onTap: _pickTime,
            borderRadius: BorderRadius.circular(14),
            child: InputDecorator(
              decoration: _fieldDecoration("Vrijeme termina"),
              child: Row(
                mainAxisAlignment: MainAxisAlignment.spaceBetween,
                children: [
                  Text(timeLabel),
                  const Icon(
                    Icons.access_time_rounded,
                    size: 18,
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
              onPressed: _isSaving ? null : _saveEvent,
              style: ElevatedButton.styleFrom(
                backgroundColor: AppColors.seed,
                foregroundColor: Colors.white,
                padding: const EdgeInsets.symmetric(vertical: 14),
                shape: RoundedRectangleBorder(
                  borderRadius: BorderRadius.circular(16),
                ),
              ),
              child: _isSaving
                  ? const SizedBox(
                      height: 18,
                      width: 18,
                      child: CircularProgressIndicator(
                        strokeWidth: 2,
                        valueColor: AlwaysStoppedAnimation<Color>(Colors.white),
                      ),
                    )
                  : const Text(
                      "Sačuvaj termin",
                      style: TextStyle(
                        fontWeight: FontWeight.w700,
                        fontSize: 16,
                      ),
                    ),
            ),
          ),
        ],
      ),
    );
  }
}
