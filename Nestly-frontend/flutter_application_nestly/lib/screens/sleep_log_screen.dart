import 'dart:convert';
import 'package:fl_chart/fl_chart.dart';
import 'package:flutter/material.dart';
import 'package:flutter_application_nestly/layouts/nestly_toast.dart';
import 'package:flutter_application_nestly/network/api_client.dart';
import 'package:flutter_application_nestly/main.dart';

class SleepLogEntry {
  final int id;
  final int babyId;
  final DateTime sleepDate;
  final Duration startTime;
  final Duration endTime;
  final String? notes;

  const SleepLogEntry({
    required this.id,
    required this.babyId,
    required this.sleepDate,
    required this.startTime,
    required this.endTime,
    this.notes,
  });

  factory SleepLogEntry.fromJson(Map<String, dynamic> json) {
    Duration parseTime(String value) {
      final parts = value.split(':');
      return Duration(hours: int.parse(parts[0]), minutes: int.parse(parts[1]));
    }

    return SleepLogEntry(
      id: json['id'],
      babyId: json['babyId'],
      sleepDate: DateTime.parse(json['sleepDate']),
      startTime: parseTime(json['startTime']),
      endTime: parseTime(json['endTime']),
      notes: json['notes'],
    );
  }

  double get durationHours {
    final end = endTime < startTime
        ? endTime + const Duration(hours: 24)
        : endTime;

    return (end - startTime).inMinutes / 60.0;
  }
}

class CreateSleepLogRequest {
  final int babyId;
  final DateTime sleepDate;
  final String startTime;
  final String endTime;
  final String? notes;

  const CreateSleepLogRequest({
    required this.babyId,
    required this.sleepDate,
    required this.startTime,
    required this.endTime,
    this.notes,
  });

  Map<String, dynamic> toJson() => {
    'babyId': babyId,
    'sleepDate': sleepDate.toIso8601String(),
    'startTime': startTime,
    'endTime': endTime,
    'notes': notes,
  };
}

class SleepDaySummary {
  final DateTime date;
  final double hours;

  const SleepDaySummary({required this.date, required this.hours});
}

class SleepLogApiService {
  Future<List<SleepLogEntry>> getLast7Days({required int babyId}) async {
    final now = DateTime.now();
    final from = now.subtract(const Duration(days: 6));

    final res = await ApiClient.get(
      '/api/SleepLog'
      '?BabyId=$babyId'
      '&DateFrom=${from.toIso8601String()}'
      '&DateTo=${now.toIso8601String()}',
    );

    if (res.statusCode != 200) {
      throw Exception('Failed to load sleep logs');
    }

    final List raw = jsonDecode(res.body);
    return raw.map((e) => SleepLogEntry.fromJson(e)).toList();
  }

  Future<void> update({
    required int id,
    required DateTime sleepDate,
    required String startTime,
    required String endTime,
  }) async {
    final res = await ApiClient.patch(
      '/api/SleepLog/$id',
      body: {
        'sleepDate': sleepDate.toIso8601String(),
        'startTime': startTime,
        'endTime': endTime,
      },
    );

    if (res.statusCode != 200) {
      throw Exception('Update failed');
    }
  }

  Future<void> delete(int id) async {
    final res = await ApiClient.delete('/api/SleepLog/$id');

    if (res.statusCode != 204) {
      throw Exception('Delete failed');
    }
  }

  Future<void> create({required CreateSleepLogRequest request}) async {
    final res = await ApiClient.post('/api/SleepLog', body: request.toJson());

    if (res.statusCode != 200 && res.statusCode != 201) {
      throw Exception('Failed to save sleep log');
    }
  }
}

class SleepLogOverviewScreen extends StatefulWidget {
  final int babyId;
  final String babyName;

  const SleepLogOverviewScreen({
    super.key,
    required this.babyId,
    required this.babyName,
  });

  @override
  State<SleepLogOverviewScreen> createState() => _SleepLogOverviewScreenState();
}

class _SleepLogOverviewScreenState extends State<SleepLogOverviewScreen> {
  final SleepLogApiService _service = SleepLogApiService();

  bool _loading = true;
  bool _saving = false;
  List<SleepLogEntry> _entries = [];
  SleepLogEntry? _editingEntry;
  List<SleepDaySummary> _last7Days = const [];

  DateTime _selectedDate = DateTime.now();
  TimeOfDay? _startTime;
  TimeOfDay? _endTime;
  void _cancelEdit() {
    setState(() {
      _editingEntry = null;
      _startTime = null;
      _endTime = null;
      _selectedDate = DateTime.now();
    });
  }

  @override
  void initState() {
    super.initState();
    _load();
  }

  Future<void> _load() async {
    try {
      final entries = await _service.getLast7Days(babyId: widget.babyId);

      setState(() {
        _entries = entries;
        _last7Days = _buildSummary(entries);
        _loading = false;
      });
    } catch (_) {
      setState(() => _loading = false);
      NestlyToast.error(context, 'Greška pri učitavanju');
    }
  }

  List<SleepDaySummary> _buildSummary(List<SleepLogEntry> entries) {
    final today = DateTime.now();
    final start = DateTime(
      today.year,
      today.month,
      today.day,
    ).subtract(const Duration(days: 6));

    final hours = List<double>.filled(7, 0);

    for (final e in entries) {
      final idx = DateTime(
        e.sleepDate.year,
        e.sleepDate.month,
        e.sleepDate.day,
      ).difference(start).inDays;

      if (idx >= 0 && idx < 7) {
        hours[idx] += e.durationHours;
      }
    }

    return List.generate(
      7,
      (i) => SleepDaySummary(
        date: start.add(Duration(days: i)),
        hours: hours[i],
      ),
    );
  }

  double get _maxHours {
    double max = 10;
    for (final d in _last7Days) {
      if (d.hours > max) max = d.hours + 1;
    }
    return max;
  }

  Future<void> _pickDate() async {
    final picked = await showDatePicker(
      context: context,
      initialDate: _selectedDate,
      firstDate: DateTime.now().subtract(const Duration(days: 365)),
      lastDate: DateTime.now(),
    );
    if (picked != null) setState(() => _selectedDate = picked);
  }

  Future<void> _pickStart() async {
    final picked = await showTimePicker(
      context: context,
      initialTime: _startTime ?? TimeOfDay.now(),
    );
    if (picked != null) setState(() => _startTime = picked);
  }

  Future<void> _pickEnd() async {
    final picked = await showTimePicker(
      context: context,
      initialTime: _endTime ?? TimeOfDay.now(),
    );
    if (picked != null) setState(() => _endTime = picked);
  }

  String get _durationLabel {
    if (_startTime == null || _endTime == null) return '-';

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

    if (end.isBefore(start)) end = end.add(const Duration(days: 1));

    final diff = end.difference(start);
    return '${diff.inHours} h ${(diff.inMinutes % 60).toString().padLeft(2, '0')} min';
  }

  String _formatDuration(Duration duration) {
    final hours = duration.inHours;
    final minutes = duration.inMinutes % 60;

    return '${hours.toString().padLeft(2, '0')}:${minutes.toString().padLeft(2, '0')}';
  }

  Future<void> _save() async {
    if (_startTime == null || _endTime == null) {
      NestlyToast.info(context, 'Unesite početak i kraj');
      return;
    }

    setState(() => _saving = true);

    final startStr =
        '${_startTime!.hour.toString().padLeft(2, '0')}:${_startTime!.minute.toString().padLeft(2, '0')}:00';

    final endStr =
        '${_endTime!.hour.toString().padLeft(2, '0')}:${_endTime!.minute.toString().padLeft(2, '0')}:00';
    try {
      if (_editingEntry == null) {
        await _service.create(
          request: CreateSleepLogRequest(
            babyId: widget.babyId,
            sleepDate: _selectedDate,
            startTime: startStr,
            endTime: endStr,
          ),
        );

        NestlyToast.success(
          context,
          'Zapis sačuvan',
          accentColor: AppColors.seed,
        );
      } else {
        await _service.update(
          id: _editingEntry!.id,
          sleepDate: _selectedDate,
          startTime: startStr,
          endTime: endStr,
        );

        NestlyToast.success(
          context,
          'Zapis ažuriran',
          accentColor: AppColors.seed,
        );
      }

      _editingEntry = null;
      _startTime = null;
      _endTime = null;

      await _load();
    } catch (_) {
      NestlyToast.error(context, 'Greška pri spremanju');
    }

    setState(() => _saving = false);
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppColors.bg,
      appBar: AppBar(
        backgroundColor: Colors.transparent,
        elevation: 0,
        leading: IconButton(
          icon: const Icon(Icons.arrow_back_ios_new_rounded),
          color: AppColors.seed,
          onPressed: () => Navigator.pop(context),
        ),
        centerTitle: true,
        title: Text(
          'Dnevnik spavanja',
          style: Theme.of(context).textTheme.titleLarge?.copyWith(
            fontWeight: FontWeight.w700,
            color: AppColors.seed,
          ),
        ),
      ),
      body: _loading
          ? const Center(child: CircularProgressIndicator())
          : SingleChildScrollView(
              padding: const EdgeInsets.all(AppSpacing.lg),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.stretch,
                children: [
                  _buildChartCard(),
                  const SizedBox(height: AppSpacing.lg),
                  _buildFormCard(),
                  const SizedBox(height: AppSpacing.lg),
                  _buildEntriesList(),
                ],
              ),
            ),
    );
  }

  Widget _buildEntriesList() {
    if (_entries.isEmpty) {
      return const SizedBox();
    }

    return Column(
      children: _entries.map((e) {
        return Card(
          margin: const EdgeInsets.only(bottom: 12),
          shape: RoundedRectangleBorder(
            borderRadius: BorderRadius.circular(AppRadius.lg),
          ),
          child: ListTile(
            title: Text(
              '${e.sleepDate.day}.${e.sleepDate.month}.',
              style: const TextStyle(
                fontWeight: FontWeight.w700,
                color: AppColors.seed,
              ),
            ),
            subtitle: Text(
              '${_formatDuration(e.startTime)} - ${_formatDuration(e.endTime)}',
            ),
            trailing: Row(
              mainAxisSize: MainAxisSize.min,
              children: [
                IconButton(
                  icon: const Icon(Icons.edit, color: AppColors.seed),
                  onPressed: () {
                    setState(() {
                      _editingEntry = e;
                      _selectedDate = e.sleepDate;
                      _startTime = TimeOfDay(
                        hour: e.startTime.inHours,
                        minute: e.startTime.inMinutes % 60,
                      );
                      _endTime = TimeOfDay(
                        hour: e.endTime.inHours,
                        minute: e.endTime.inMinutes % 60,
                      );
                    });
                  },
                ),
                IconButton(
                  icon: const Icon(Icons.delete, color: Colors.red),
                  onPressed: () => _confirmDelete(e),
                ),
              ],
            ),
          ),
        );
      }).toList(),
    );
  }

  Future<void> _confirmDelete(SleepLogEntry entry) async {
    final confirm = await showDialog<bool>(
      context: context,
      builder: (_) => AlertDialog(
        title: const Text('Obrisati zapis?'),
        content: const Text('Da li ste sigurni?'),
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
      await _service.delete(entry.id);
      await _load();
      NestlyToast.success(
        context,
        'Zapis obrisan',
        accentColor: AppColors.seed,
      );
    } catch (_) {
      NestlyToast.error(context, 'Greška pri brisanju');
    }
  }

  Widget _buildChartCard() {
    return Card(
      shape: RoundedRectangleBorder(
        borderRadius: BorderRadius.circular(AppRadius.xl),
      ),
      child: Padding(
        padding: const EdgeInsets.all(AppSpacing.lg),
        child: SizedBox(
          height: 260,
          child: BarChart(
            BarChartData(
              minY: 0,
              maxY: _maxHours,
              barTouchData: BarTouchData(
                enabled: true,
                touchTooltipData: BarTouchTooltipData(
                  tooltipRoundedRadius: 12,
                  tooltipPadding: const EdgeInsets.all(8),
                  getTooltipItem: (group, _, rod, __) {
                    final hours = rod.toY;
                    final h = hours.floor();
                    final m = ((hours - h) * 60).round();
                    return BarTooltipItem(
                      '$h h ${m.toString().padLeft(2, '0')} min',
                      const TextStyle(
                        color: Colors.white,
                        fontWeight: FontWeight.w700,
                      ),
                    );
                  },
                ),
              ),
              gridData: FlGridData(
                show: true,
                drawVerticalLine: false,
                horizontalInterval: 2,
                getDrawingHorizontalLine: (value) => FlLine(
                  color: AppColors.seed.withOpacity(0.08),
                  strokeWidth: 1,
                ),
              ),
              borderData: FlBorderData(show: false),
              titlesData: FlTitlesData(
                topTitles: const AxisTitles(
                  sideTitles: SideTitles(showTitles: false),
                ),
                rightTitles: const AxisTitles(
                  sideTitles: SideTitles(showTitles: false),
                ),
                leftTitles: AxisTitles(
                  sideTitles: SideTitles(
                    showTitles: true,
                    interval: 2,
                    reservedSize: 36,
                    getTitlesWidget: (v, _) => Text(
                      '${v.toInt()}h',
                      style: const TextStyle(
                        fontSize: 11,
                        color: AppColors.textSecondary,
                      ),
                    ),
                  ),
                ),
                bottomTitles: AxisTitles(
                  sideTitles: SideTitles(
                    showTitles: true,
                    getTitlesWidget: (v, _) => Padding(
                      padding: const EdgeInsets.only(top: 6),
                      child: Text(
                        const ['P', 'U', 'S', 'Č', 'P', 'S', 'N'][v.toInt()],
                        style: const TextStyle(
                          fontWeight: FontWeight.w600,
                          color: AppColors.seed,
                        ),
                      ),
                    ),
                  ),
                ),
              ),
              barGroups: List.generate(_last7Days.length, (i) {
                final isToday = _last7Days[i].date.day == DateTime.now().day;

                return BarChartGroupData(
                  x: i,
                  barRods: [
                    BarChartRodData(
                      toY: _last7Days[i].hours,
                      width: 18,
                      borderRadius: BorderRadius.circular(8),
                      gradient: LinearGradient(
                        begin: Alignment.bottomCenter,
                        end: Alignment.topCenter,
                        colors: isToday
                            ? [AppColors.seed, AppColors.seed.withOpacity(0.7)]
                            : [
                                AppColors.babyBlue,
                                AppColors.babyBlue.withOpacity(0.6),
                              ],
                      ),
                    ),
                  ],
                );
              }),
            ),
            swapAnimationDuration: const Duration(milliseconds: 350),
            swapAnimationCurve: Curves.easeOutCubic,
          ),
        ),
      ),
    );
  }

  Widget _buildFormCard() {
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
          children: [
            _picker(
              'Datum',
              _pickDate,
              '${_selectedDate.day.toString().padLeft(2, '0')}.${_selectedDate.month.toString().padLeft(2, '0')}.${_selectedDate.year}.',
            ),
            const SizedBox(height: 12),
            _picker(
              'Početak',
              _pickStart,
              _startTime?.format(context) ?? 'Odaberite vrijeme',
            ),
            const SizedBox(height: 12),
            _picker(
              'Kraj',
              _pickEnd,
              _endTime?.format(context) ?? 'Odaberite vrijeme',
            ),
            const SizedBox(height: 12),
            InputDecorator(
              decoration: deco('Trajanje'),
              child: Text(_durationLabel),
            ),
            const SizedBox(height: 16),
            Column(
              children: [
                SizedBox(
                  width: double.infinity,
                  child: ElevatedButton(
                    onPressed: _saving ? null : _save,
                    style: ElevatedButton.styleFrom(
                      backgroundColor: AppColors.seed,
                      foregroundColor: Colors.white,
                      padding: const EdgeInsets.symmetric(vertical: 14),
                      shape: RoundedRectangleBorder(
                        borderRadius: BorderRadius.circular(AppRadius.lg),
                      ),
                    ),
                    child: _saving
                        ? const CircularProgressIndicator(
                            color: Colors.white,
                            strokeWidth: 2,
                          )
                        : Text(
                            _editingEntry == null
                                ? 'Sačuvaj'
                                : 'Sačuvaj izmjene',
                            style: const TextStyle(fontWeight: FontWeight.w700),
                          ),
                  ),
                ),

                if (_editingEntry != null) ...[
                  const SizedBox(height: 12),
                  SizedBox(
                    width: double.infinity,
                    child: OutlinedButton(
                      onPressed: _saving ? null : _cancelEdit,
                      style: OutlinedButton.styleFrom(
                        foregroundColor: AppColors.seed,
                        side: const BorderSide(color: AppColors.seed),
                        padding: const EdgeInsets.symmetric(vertical: 14),
                        shape: RoundedRectangleBorder(
                          borderRadius: BorderRadius.circular(AppRadius.lg),
                        ),
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
          ],
        ),
      ),
    );
  }

  Widget _picker(String label, VoidCallback onTap, String value) {
    return InkWell(
      onTap: onTap,
      child: InputDecorator(
        decoration: InputDecoration(
          labelText: label,
          filled: true,
          fillColor: Colors.white,
          border: OutlineInputBorder(
            borderRadius: BorderRadius.circular(AppRadius.lg),
          ),
        ),
        child: Row(
          mainAxisAlignment: MainAxisAlignment.spaceBetween,
          children: [Text(value), const Icon(Icons.access_time_rounded)],
        ),
      ),
    );
  }
}
