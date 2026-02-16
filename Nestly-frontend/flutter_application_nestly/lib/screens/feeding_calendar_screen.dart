import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:flutter_application_nestly/network/api_client.dart';
import 'package:flutter_application_nestly/layouts/nestly_calendar.dart';
import 'package:flutter_application_nestly/layouts/nestly_toast.dart';
import 'package:flutter_application_nestly/main.dart';

/// =============================================================
/// MODEL
/// =============================================================

class FeedingLog {
  final int id;
  final int babyId;
  final DateTime feedDate;
  final Duration feedTime;
  final double? amountMl;
  final int? foodTypeId;
  final String? notes;

  FeedingLog({
    required this.id,
    required this.babyId,
    required this.feedDate,
    required this.feedTime,
    this.amountMl,
    this.foodTypeId,
    this.notes,
  });

  bool isSameDay(DateTime day) {
    final d = DateTime(day.year, day.month, day.day);
    final f = DateTime(feedDate.year, feedDate.month, feedDate.day);
    return d.isAtSameMomentAs(f);
  }

  factory FeedingLog.fromJson(Map<String, dynamic> json) {
    return FeedingLog(
      id: json['id'],
      babyId: json['babyId'],
      feedDate: DateTime.parse(json['feedDate']),
      feedTime: _parseTime(json['feedTime']),
      amountMl: json['amountMl']?.toDouble(),
      foodTypeId: json['foodTypeId'],
      notes: json['notes'],
    );
  }

  static Duration _parseTime(String time) {
    final parts = time.split(':');
    return Duration(hours: int.parse(parts[0]), minutes: int.parse(parts[1]));
  }
}

/// =============================================================
/// API SERVICE
/// =============================================================

class FeedingLogApiService {
  final int babyId;

  FeedingLogApiService(this.babyId);

  Future<List<FeedingLog>> fetch() async {
    final res = await ApiClient.get('/api/FeedingLog?BabyId=$babyId');

    if (res.statusCode != 200) {
      throw Exception('Load failed');
    }

    final List data = jsonDecode(res.body);
    return data.map((e) => FeedingLog.fromJson(e)).toList();
  }

  Future<void> create({
    required DateTime date,
    required Duration time,
    double? amountMl,
    int? foodTypeId,
    String? notes,
  }) async {
    final res = await ApiClient.post(
      '/api/FeedingLog',
      body: {
        'babyId': babyId,
        'feedDate': date.toIso8601String(),
        'feedTime':
            '${time.inHours.toString().padLeft(2, '0')}:${(time.inMinutes % 60).toString().padLeft(2, '0')}:00',
        'amountMl': amountMl,
        'foodTypeId': foodTypeId,
        'notes': notes,
      },
    );

    if (res.statusCode != 200 && res.statusCode != 201) {
      throw Exception('Create failed');
    }
  }

  Future<void> delete(int id) async {
    final res = await ApiClient.delete('/api/FeedingLog/$id');

    if (res.statusCode != 204) {
      throw Exception('Delete failed');
    }
  }
}

/// =============================================================
/// MAIN SCREEN
/// =============================================================

class FeedingCalendarScreen extends StatefulWidget {
  const FeedingCalendarScreen({super.key, required this.babyId});

  final int babyId;

  @override
  State<FeedingCalendarScreen> createState() => _FeedingCalendarScreenState();
}

class _FeedingCalendarScreenState extends State<FeedingCalendarScreen> {
  late final FeedingLogApiService _service;

  DateTime _focusedDay = DateTime.now();
  DateTime? _selectedDay;

  bool _loading = true;
  List<FeedingLog> _logs = [];

  @override
  void initState() {
    super.initState();
    _service = FeedingLogApiService(widget.babyId);
    _load();
  }

  Future<void> _load() async {
    setState(() => _loading = true);
    try {
      _logs = await _service.fetch();
    } catch (_) {
      NestlyToast.error(context, 'Greška pri učitavanju dnevnika');
    }
    if (mounted) setState(() => _loading = false);
  }

  List<FeedingLog> _forDay(DateTime day) =>
      _logs.where((e) => e.isSameDay(day)).toList();

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppColors.bg,
      appBar: AppBar(
        backgroundColor: Colors.transparent,
        elevation: 0,
        iconTheme: const IconThemeData(color: AppColors.seed),
        title: Text(
          'Dnevnik hranjenja',
          style: Theme.of(context).textTheme.titleLarge?.copyWith(
            fontWeight: FontWeight.w800,
            color: AppColors.seed,
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
                  markerIcon: Icons.restaurant_rounded,
                  accentColor: AppColors.seed,
                  eventLoader: (day) => _forDay(day),
                  onDaySelected: (selected, focused) {
                    setState(() {
                      _selectedDay = DateTime(
                        selected.year,
                        selected.month,
                        selected.day,
                      );
                      _focusedDay = DateTime(
                        focused.year,
                        focused.month,
                        focused.day,
                      );
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
      return const Center(child: Text('Odaberite dan'));
    }

    final list = _forDay(_selectedDay!);

    if (list.isEmpty) {
      return const Center(child: Text('Nema unosa za ovaj dan'));
    }

    return ListView(
      padding: const EdgeInsets.all(AppSpacing.lg),
      children: list.map(_card).toList(),
    );
  }

  Widget _card(FeedingLog log) {
    return Card(
      margin: const EdgeInsets.only(bottom: 12),
      shape: RoundedRectangleBorder(
        borderRadius: BorderRadius.circular(AppRadius.lg),
      ),
      child: Padding(
        padding: const EdgeInsets.all(AppSpacing.lg),
        child: Row(
          children: [
            const Icon(Icons.baby_changing_station, color: AppColors.seed),
            const SizedBox(width: AppSpacing.md),
            Expanded(
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Text(
                    _formatTime(log.feedTime),
                    style: const TextStyle(
                      fontWeight: FontWeight.w800,
                      color: AppColors.seed,
                    ),
                  ),
                  if (log.amountMl != null)
                    Text('Količina: ${log.amountMl} ml'),
                  if (log.notes != null && log.notes!.isNotEmpty)
                    Text(log.notes!),
                ],
              ),
            ),
            IconButton(
              icon: const Icon(Icons.delete),
              onPressed: () async {
                await _service.delete(log.id);
                _load();
              },
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
            backgroundColor: AppColors.seed,
            foregroundColor: Colors.white,
            minimumSize: const Size.fromHeight(52),
          ),
          onPressed: () async {
            final dateToUse = _selectedDay ?? DateTime.now();

            await Navigator.push(
              context,
              MaterialPageRoute(
                builder: (_) => AddFeedingLogScreen(
                  service: _service,
                  initialDate: dateToUse,
                ),
              ),
            );

            _load();
          },

          child: const Text('Dodaj unos'),
        ),
      ),
    );
  }

  String _formatTime(Duration d) {
    final h = d.inHours.toString().padLeft(2, '0');
    final m = (d.inMinutes % 60).toString().padLeft(2, '0');
    return '$h:$m';
  }
}

/// =============================================================
/// ADD SCREEN
/// =============================================================

class AddFeedingLogScreen extends StatefulWidget {
  const AddFeedingLogScreen({
    super.key,
    required this.service,
    required this.initialDate,
  });

  final FeedingLogApiService service;
  final DateTime initialDate;

  @override
  State<AddFeedingLogScreen> createState() => _AddFeedingLogScreenState();
}

class _AddFeedingLogScreenState extends State<AddFeedingLogScreen> {
  late DateTime _date;
  Duration _time = Duration(
    hours: DateTime.now().hour,
    minutes: DateTime.now().minute,
  );

  final _amount = TextEditingController();
  final _notes = TextEditingController();

  bool _saving = false;
  @override
  void initState() {
    super.initState();
    _date = DateTime(
      widget.initialDate.year,
      widget.initialDate.month,
      widget.initialDate.day,
    );
  }

  Future<void> _pickTime() async {
    final picked = await showTimePicker(
      context: context,
      initialTime: TimeOfDay(hour: _time.inHours, minute: _time.inMinutes % 60),
    );

    if (picked != null) {
      setState(() {
        _time = Duration(hours: picked.hour, minutes: picked.minute);
      });
    }
  }

  Future<void> _save() async {
    setState(() => _saving = true);
    try {
      await widget.service.create(
        date: _date,
        time: _time,
        amountMl: double.tryParse(_amount.text),
        notes: _notes.text,
      );
      Navigator.pop(context);
      NestlyToast.success(context, 'Unos dodan', accentColor: AppColors.seed);
    } catch (_) {
      NestlyToast.error(context, 'Greška');
    }
    if (mounted) setState(() => _saving = false);
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
          'Dodaj unos',
          style: Theme.of(context).textTheme.titleLarge?.copyWith(
            fontWeight: FontWeight.w800,
            color: AppColors.seed,
          ),
        ),
        centerTitle: true,
      ),

      body: Padding(
        padding: const EdgeInsets.all(AppSpacing.lg),
        child: Column(
          children: [
            InkWell(
              onTap: _pickTime,
              child: InputDecorator(
                decoration: _decoration(label: 'Vrijeme', icon: Icons.schedule),
                child: Text(_formatTime(_time)),
              ),
            ),
            const SizedBox(height: 12),
            TextField(
              controller: _amount,
              keyboardType: TextInputType.number,
              decoration: _decoration(
                label: 'Količina (ml/g)',
                icon: Icons.scale,
              ),
            ),
            const SizedBox(height: 12),
            TextField(
              controller: _notes,
              decoration: _decoration(label: 'Napomena', icon: Icons.note),
            ),
            const SizedBox(height: 24),
            SizedBox(
              height: 52,
              width: double.infinity,
              child: ElevatedButton(
                onPressed: _saving ? null : _save,
                style: ElevatedButton.styleFrom(
                  backgroundColor: AppColors.seed,
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

/// =============================================================
/// HELPERS
/// =============================================================

String _formatTime(Duration d) {
  final h = d.inHours.toString().padLeft(2, '0');
  final m = (d.inMinutes % 60).toString().padLeft(2, '0');
  return '$h:$m';
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
      borderSide: const BorderSide(color: AppColors.seed, width: 1.6),
    ),
    floatingLabelStyle: const TextStyle(
      color: AppColors.seed,
      fontWeight: FontWeight.w600,
    ),
    prefixIconColor: AppColors.seed,
  );
}
