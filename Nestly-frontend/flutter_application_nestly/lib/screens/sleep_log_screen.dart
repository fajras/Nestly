import 'dart:convert';
import 'dart:io' show Platform;

import 'package:fl_chart/fl_chart.dart';
import 'package:flutter/foundation.dart' show kIsWeb;
import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;

import 'package:flutter_application_nestly/main.dart';

class SleepLogEntry {
  final int id;
  final int babyId;
  final DateTime sleepDate;
  final DateTime startTime;
  final DateTime endTime;
  final String? notes;

  SleepLogEntry({
    required this.id,
    required this.babyId,
    required this.sleepDate,
    required this.startTime,
    required this.endTime,
    this.notes,
  });

  factory SleepLogEntry.fromJson(Map<String, dynamic> json) {
    return SleepLogEntry(
      id: json['id'] as int,
      babyId: json['babyId'] as int,
      sleepDate: DateTime.parse(json['sleepDate'] as String),
      startTime: DateTime.parse(json['startTime'] as String),
      endTime: DateTime.parse(json['endTime'] as String),
      notes: json['notes'] as String?,
    );
  }

  double get durationHours {
    var end = endTime;
    var start = startTime;

    if (end.isBefore(start)) {
      end = end.add(const Duration(days: 1));
    }

    return end.difference(start).inMinutes / 60.0;
  }
}

class CreateSleepLogRequest {
  final int babyId;
  final DateTime sleepDate;
  final DateTime startTime;
  final DateTime endTime;
  final String? notes;

  CreateSleepLogRequest({
    required this.babyId,
    required this.sleepDate,
    required this.startTime,
    required this.endTime,
    this.notes,
  });

  Map<String, dynamic> toJson() => {
    'babyId': babyId,
    'sleepDate': sleepDate.toIso8601String(),
    'startTime': startTime.toIso8601String(),
    'endTime': endTime.toIso8601String(),
    'notes': notes,
  };
}

class SleepDaySummary {
  final DateTime date;
  final double hours;

  SleepDaySummary({required this.date, required this.hours});
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

class SleepLogApiService {
  String get _baseUrl => '$_apiBase/api/SleepLog';

  Future<List<SleepLogEntry>> getLast7Days({
    required int babyId,
    String? token,
  }) async {
    final now = DateTime.now();
    final from = now.subtract(const Duration(days: 6));

    final uri = Uri.parse(
      '$_baseUrl?BabyId=$babyId'
      '&DateFrom=${from.toIso8601String()}'
      '&DateTo=${now.toIso8601String()}',
    );
    final resp = await http.get(uri, headers: _headers(token));
    if (resp.statusCode != 200) {
      throw Exception('Greška pri dohvaćanju dnevnika spavanja');
    }

    final List<dynamic> data = jsonDecode(resp.body) as List<dynamic>;
    return data
        .map((e) => SleepLogEntry.fromJson(e as Map<String, dynamic>))
        .toList();
  }

  Future<SleepLogEntry> create({
    required CreateSleepLogRequest request,
    String? token,
  }) async {
    final uri = Uri.parse(_baseUrl);
    final resp = await http.post(
      uri,
      headers: _headers(token),
      body: jsonEncode(request.toJson()),
    );
    debugPrint('Response status: ${resp.statusCode}');
    debugPrint('Response body: ${resp.body}');
    if (resp.statusCode != 201 && resp.statusCode != 200) {
      throw Exception(
        'Greška pri spremanju zapisa spavanja '
        '(status: ${resp.statusCode}) -> ${resp.body}',
      );
    }

    final Map<String, dynamic> data =
        jsonDecode(resp.body) as Map<String, dynamic>;
    return SleepLogEntry.fromJson(data);
  }
}

class SleepLogOverviewScreen extends StatefulWidget {
  final int babyId;
  final String babyName;
  final String? token;

  const SleepLogOverviewScreen({
    super.key,
    required this.babyId,
    required this.babyName,
    this.token,
  });

  @override
  State<SleepLogOverviewScreen> createState() => _SleepLogOverviewScreenState();
}

class _SleepLogOverviewScreenState extends State<SleepLogOverviewScreen> {
  final _service = SleepLogApiService();

  bool _isLoading = true;
  bool _isSaving = false;

  List<SleepLogEntry> _entries = [];
  List<SleepDaySummary> _last7Days = [];

  DateTime _selectedDate = DateTime.now();
  TimeOfDay? _startTime;
  TimeOfDay? _endTime;
  final _notesCtrl = TextEditingController();

  @override
  void initState() {
    super.initState();
    _loadData();
  }

  @override
  void dispose() {
    _notesCtrl.dispose();
    super.dispose();
  }

  Future<void> _loadData() async {
    setState(() => _isLoading = true);
    try {
      final list = await _service.getLast7Days(
        babyId: widget.babyId,
        token: widget.token,
      );
      _entries = list;
      _last7Days = _buildLast7DaysSummary(list);
      setState(() => _isLoading = false);
    } catch (e) {
      setState(() => _isLoading = false);
      if (!mounted) return;
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Greška pri učitavanju podataka: $e')),
      );
    }
  }

  List<SleepDaySummary> _buildLast7DaysSummary(List<SleepLogEntry> entries) {
    final now = DateTime.now();
    final today = DateTime(now.year, now.month, now.day);

    final Map<DateTime, double> map = {};
    for (int i = 6; i >= 0; i--) {
      final d = today.subtract(Duration(days: i));
      map[d] = 0.0;
    }

    for (final e in entries) {
      final d = DateTime(e.sleepDate.year, e.sleepDate.month, e.sleepDate.day);
      if (map.containsKey(d)) {
        map[d] = (map[d] ?? 0) + e.durationHours;
      }
    }

    return map.entries
        .map((e) => SleepDaySummary(date: e.key, hours: e.value))
        .toList()
      ..sort((a, b) => a.date.compareTo(b.date));
  }

  String _weekdayLabel(DateTime date) {
    switch (date.weekday) {
      case DateTime.monday:
        return 'P';
      case DateTime.tuesday:
        return 'U';
      case DateTime.wednesday:
        return 'S';
      case DateTime.thursday:
        return 'Č';
      case DateTime.friday:
        return 'P';
      case DateTime.saturday:
        return 'S';
      case DateTime.sunday:
        return 'N';
      default:
        return '';
    }
  }

  double get _maxHours {
    if (_last7Days.isEmpty) return 10;
    final max = _last7Days
        .map((e) => e.hours)
        .fold<double>(0, (prev, curr) => curr > prev ? curr : prev);
    return (max < 10 ? 10 : max + 1);
  }

  Future<void> _pickDate() async {
    final picked = await showDatePicker(
      context: context,
      initialDate: _selectedDate,
      firstDate: DateTime.now().subtract(const Duration(days: 365)),
      lastDate: DateTime.now(),
    );
    if (picked != null) {
      setState(() => _selectedDate = picked);
    }
  }

  Future<void> _pickStartTime() async {
    final picked = await showTimePicker(
      context: context,
      initialTime: _startTime ?? TimeOfDay.now(),
    );
    if (picked != null) {
      setState(() => _startTime = picked);
    }
  }

  Future<void> _pickEndTime() async {
    final picked = await showTimePicker(
      context: context,
      initialTime: _endTime ?? TimeOfDay.now(),
    );
    if (picked != null) {
      setState(() => _endTime = picked);
    }
  }

  String get _durationText {
    if (_startTime == null || _endTime == null) return "";
    final start = DateTime(
      _selectedDate.year,
      _selectedDate.month,
      _selectedDate.day,
      _startTime!.hour,
      _startTime!.minute,
    );
    var end = DateTime(
      _selectedDate.year,
      _selectedDate.month,
      _selectedDate.day,
      _endTime!.hour,
      _endTime!.minute,
    );
    if (end.isBefore(start)) {
      end = end.add(const Duration(days: 1));
    }
    final diff = end.difference(start);
    final h = diff.inHours;
    final m = diff.inMinutes % 60;
    if (h == 0 && m == 0) return "";
    return "${h} h ${m.toString().padLeft(2, '0')} min";
  }

  Future<void> _save() async {
    if (_startTime == null || _endTime == null) {
      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(
          content: Text('Molimo unesite početak i kraj spavanja.'),
        ),
      );
      return;
    }

    setState(() => _isSaving = true);
    final dateOnly = DateTime(
      _selectedDate.year,
      _selectedDate.month,
      _selectedDate.day,
    );
    final start = DateTime(
      _selectedDate.year,
      _selectedDate.month,
      _selectedDate.day,
      _startTime!.hour,
      _startTime!.minute,
    );
    var end = DateTime(
      _selectedDate.year,
      _selectedDate.month,
      _selectedDate.day,
      _endTime!.hour,
      _endTime!.minute,
    );
    if (end.isBefore(start)) {
      end = end.add(const Duration(days: 1));
    }

    try {
      await _service.create(
        request: CreateSleepLogRequest(
          babyId: widget.babyId,
          sleepDate: dateOnly,
          startTime: start,
          endTime: end,
        ),
        token: widget.token,
      );

      if (!mounted) return;

      setState(() {
        _startTime = null;
        _endTime = null;
      });

      await _loadData();

      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(content: Text('Zapis spavanja je sačuvan.')),
      );
    } catch (e) {
      setState(() => _isSaving = false);
      if (!mounted) return;
      ScaffoldMessenger.of(
        context,
      ).showSnackBar(SnackBar(content: Text('Greška pri spremanju: $e')));
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
          "Dnevnik spavanja",
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
                  _buildChartCard(),
                  const SizedBox(height: 16),
                  Expanded(
                    child: SingleChildScrollView(child: _buildFormCard()),
                  ),
                ],
              ),
            ),
    );
  }

  Widget _buildChartCard() {
    return Container(
      height: 260,
      padding: const EdgeInsets.all(12),
      decoration: BoxDecoration(
        borderRadius: BorderRadius.circular(24),
        color: Colors.white,
        boxShadow: [
          BoxShadow(
            color: Colors.black12.withOpacity(0.05),
            blurRadius: 8,
            offset: const Offset(0, 3),
          ),
        ],
      ),
      child: _last7Days.isEmpty
          ? const Center(
              child: Text(
                "Još nema zapisa za spavanje.\nDodajte prvi zapis ispod.",
                textAlign: TextAlign.center,
              ),
            )
          : Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(
                  "Sedmični prikaz spavanja (sati)",
                  style: Theme.of(context).textTheme.bodyMedium?.copyWith(
                    fontWeight: FontWeight.w600,
                    color: AppColors.seed,
                  ),
                ),
                const SizedBox(height: 8),
                Expanded(
                  child: BarChart(
                    BarChartData(
                      minY: 0,
                      maxY: _maxHours,
                      borderData: FlBorderData(show: false),
                      gridData: FlGridData(show: true, drawVerticalLine: false),
                      titlesData: FlTitlesData(
                        topTitles: const AxisTitles(
                          sideTitles: SideTitles(showTitles: false),
                        ),
                        rightTitles: const AxisTitles(
                          sideTitles: SideTitles(showTitles: false),
                        ),
                        leftTitles: const AxisTitles(
                          sideTitles: SideTitles(
                            showTitles: true,
                            reservedSize: 28,
                          ),
                        ),
                        bottomTitles: AxisTitles(
                          sideTitles: SideTitles(
                            showTitles: true,
                            getTitlesWidget: (value, meta) {
                              final index = value.toInt();
                              if (index < 0 || index >= _last7Days.length) {
                                return const SizedBox.shrink();
                              }
                              final d = _last7Days[index].date;
                              return Padding(
                                padding: const EdgeInsets.only(top: 4),
                                child: Text(
                                  _weekdayLabel(d),
                                  style: const TextStyle(fontSize: 11),
                                ),
                              );
                            },
                          ),
                        ),
                      ),
                      barGroups: List.generate(_last7Days.length, (index) {
                        final day = _last7Days[index];
                        return BarChartGroupData(
                          x: index,
                          barRods: [
                            BarChartRodData(
                              toY: day.hours,
                              width: 14,
                              borderRadius: BorderRadius.circular(6),
                              color: AppColors.babyBlue,
                              backDrawRodData: BackgroundBarChartRodData(
                                show: true,
                                toY: _maxHours,
                                color: AppColors.babyBlue.withOpacity(0.13),
                              ),
                            ),
                          ],
                        );
                      }),
                    ),
                  ),
                ),
              ],
            ),
    );
  }

  Widget _buildFormCard() {
    return Container(
      padding: const EdgeInsets.all(16),
      decoration: BoxDecoration(
        color: AppColors.bg.withOpacity(0.9),
        borderRadius: BorderRadius.circular(22),
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          const SizedBox(height: 4),
          InkWell(
            onTap: _pickDate,
            borderRadius: BorderRadius.circular(14),
            child: InputDecorator(
              decoration: _fieldDecoration("Datum"),
              child: Row(
                mainAxisAlignment: MainAxisAlignment.spaceBetween,
                children: [
                  Text(
                    "${_selectedDate.day.toString().padLeft(2, '0')}."
                    "${_selectedDate.month.toString().padLeft(2, '0')}."
                    "${_selectedDate.year}.",
                  ),
                  const Icon(
                    Icons.calendar_today_rounded,
                    size: 18,
                    color: AppColors.babyBlue,
                  ),
                ],
              ),
            ),
          ),
          const SizedBox(height: 14),

          const SizedBox(height: 4),
          InkWell(
            onTap: _pickStartTime,
            borderRadius: BorderRadius.circular(14),
            child: InputDecorator(
              decoration: _fieldDecoration("Početak"),
              child: Row(
                mainAxisAlignment: MainAxisAlignment.spaceBetween,
                children: [
                  Text(
                    _startTime == null
                        ? "Odaberite vrijeme"
                        : _startTime!.format(context),
                  ),
                  const Icon(
                    Icons.access_time_rounded,
                    size: 18,
                    color: AppColors.babyBlue,
                  ),
                ],
              ),
            ),
          ),
          const SizedBox(height: 14),

          const SizedBox(height: 4),
          InkWell(
            onTap: _pickEndTime,
            borderRadius: BorderRadius.circular(14),
            child: InputDecorator(
              decoration: _fieldDecoration("Kraj"),
              child: Row(
                mainAxisAlignment: MainAxisAlignment.spaceBetween,
                children: [
                  Text(
                    _endTime == null
                        ? "Odaberite vrijeme"
                        : _endTime!.format(context),
                  ),
                  const Icon(
                    Icons.access_time_rounded,
                    size: 18,
                    color: AppColors.babyBlue,
                  ),
                ],
              ),
            ),
          ),
          const SizedBox(height: 14),

          const SizedBox(height: 4),
          InputDecorator(
            decoration: _fieldDecoration("Trajanje"),
            child: Text(_durationText.isEmpty ? "-" : _durationText),
          ),
          const SizedBox(height: 14),

          SizedBox(
            width: double.infinity,
            child: ElevatedButton(
              onPressed: _isSaving ? null : _save,
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
                      "Sačuvaj",
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
