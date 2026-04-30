import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:flutter_application_nestly/network/api_client.dart';
import 'package:flutter_application_nestly/layouts/nestly_calendar.dart';
import 'package:flutter_application_nestly/layouts/nestly_toast.dart';
import 'package:flutter_application_nestly/main.dart';
import 'package:flutter/services.dart';

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
      feedTime: _parseTime(json['feedTime']?.toString()),
      amountMl: json['amountMl']?.toDouble(),
      foodTypeId: json['foodTypeId'],
      notes: json['notes'],
    );
  }

  static Duration _parseTime(String? time) {
    final value = (time ?? '00:00:00').toString();
    final parts = value.split(':');

    final hours = parts.isNotEmpty ? int.tryParse(parts[0]) ?? 0 : 0;
    final minutes = parts.length > 1 ? int.tryParse(parts[1]) ?? 0 : 0;

    return Duration(hours: hours, minutes: minutes);
  }
}

class FeedingLogApiService {
  final int babyId;

  FeedingLogApiService(this.babyId);
  Future<void> update({
    required int id,
    required DateTime date,
    required Duration time,
    required double amountMl,
    String? notes,
  }) async {
    final res = await ApiClient.patch(
      '/api/FeedingLog/$id',
      body: {
        'feedDate': date.toIso8601String(),
        'feedTime':
            '${time.inHours.toString().padLeft(2, '0')}:${(time.inMinutes % 60).toString().padLeft(2, '0')}:00',
        'amountMl': amountMl,
        'notes': notes,
      },
    );

    if (res.statusCode != 200) {
      throw Exception('Update failed');
    }
  }

  Future<List<FeedingLog>> fetch() async {
    int page = 1;
    const pageSize = 100;

    List<FeedingLog> result = [];

    while (true) {
      final res = await ApiClient.get(
        '/api/FeedingLog?BabyId=$babyId&page=$page&pageSize=$pageSize',
      );

      if (res.statusCode != 200) {
        throw Exception('Load failed');
      }

      final data = jsonDecode(res.body);
      final List items = data['items'];

      if (items.isEmpty) break;

      final parsed = items
          .map<Map<String, dynamic>>((e) => e as Map<String, dynamic>)
          .map(FeedingLog.fromJson)
          .toList();

      result.addAll(parsed);

      if (items.length < pageSize) break;

      page++;
    }

    return result;
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

    if (res.statusCode != 200 && res.statusCode != 204) {
      throw Exception('Delete failed (${res.statusCode})');
    }
  }
}

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
    if (!mounted) return;
    setState(() => _loading = true);

    try {
      final logs = await _service.fetch();
      if (!mounted) return;

      setState(() {
        _logs = logs;
      });
    } catch (_) {
      if (!mounted) return;
      NestlyToast.error(context, 'Greška pri učitavanju dnevnika');
    } finally {
      if (mounted) {
        setState(() => _loading = false);
      }
    }
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
            fontWeight: FontWeight.w700,
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
            const Icon(Icons.restaurant_rounded, color: AppColors.seed),
            const SizedBox(width: AppSpacing.md),
            Expanded(
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Text(
                    _formatTime(log.feedTime),
                    style: const TextStyle(
                      fontWeight: FontWeight.w700,
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
              icon: const Icon(Icons.edit, color: AppColors.seed),
              onPressed: () async {
                await Navigator.push(
                  context,
                  MaterialPageRoute(
                    builder: (_) => AddFeedingLogScreen(
                      service: _service,
                      initialDate: log.feedDate,
                      existingLog: log,
                    ),
                  ),
                );
                _load();
              },
            ),
            IconButton(
              icon: const Icon(Icons.delete),
              onPressed: () async {
                final confirm = await showDialog<bool>(
                  context: context,
                  builder: (_) => AlertDialog(
                    title: const Text('Obrisati unos?'),
                    content: const Text(
                      'Da li ste sigurni da želite obrisati ovaj unos?',
                    ),
                    actions: [
                      TextButton(
                        onPressed: () => Navigator.pop(context, false),
                        child: const Text('Odustani'),
                      ),
                      TextButton(
                        onPressed: () => Navigator.pop(context, true),
                        child: const Text(
                          'Obriši',
                          style: TextStyle(color: Colors.red),
                        ),
                      ),
                    ],
                  ),
                );

                if (confirm != true) return;

                try {
                  await _service.delete(log.id);
                  if (!mounted) return;
                  NestlyToast.success(
                    context,
                    'Unos uspješno obrisan',
                    accentColor: AppColors.seed,
                  );

                  _load();
                } catch (_) {
                  NestlyToast.error(context, 'Brisanje nije uspjelo');
                }
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

class AddFeedingLogScreen extends StatefulWidget {
  const AddFeedingLogScreen({
    super.key,
    required this.service,
    required this.initialDate,
    this.existingLog,
  });

  final FeedingLogApiService service;
  final DateTime initialDate;
  final FeedingLog? existingLog;

  @override
  State<AddFeedingLogScreen> createState() => _AddFeedingLogScreenState();
}

class _AddFeedingLogScreenState extends State<AddFeedingLogScreen> {
  late DateTime _date;
  final _formKey = GlobalKey<FormState>();
  Duration _time = Duration(
    hours: DateTime.now().hour,
    minutes: DateTime.now().minute,
  );

  final _amount = TextEditingController();
  final _notes = TextEditingController();

  bool _saving = false;
  @override
  void dispose() {
    _amount.dispose();
    _notes.dispose();
    super.dispose();
  }

  @override
  void initState() {
    super.initState();

    if (widget.existingLog != null) {
      final log = widget.existingLog!;
      _date = log.feedDate;
      _time = log.feedTime;
      _amount.text = log.amountMl?.toString() ?? '';
      _notes.text = log.notes ?? '';
    } else {
      _date = DateTime(
        widget.initialDate.year,
        widget.initialDate.month,
        widget.initialDate.day,
      );
    }
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
    if (!_formKey.currentState!.validate()) return;

    final amount = double.tryParse(_amount.text.replaceAll(',', '.'));
    if (amount == null) {
      NestlyToast.error(context, 'Unesite validnu količinu');
      return;
    }

    setState(() => _saving = true);

    try {
      if (widget.existingLog == null) {
        await widget.service.create(
          date: _date,
          time: _time,
          amountMl: amount,
          notes: _notes.text,
        );
        if (!mounted) return;
        NestlyToast.success(context, 'Unos dodan', accentColor: AppColors.seed);
      } else {
        await widget.service.update(
          id: widget.existingLog!.id,
          date: _date,
          time: _time,
          amountMl: amount,
          notes: _notes.text,
        );
        if (!mounted) return;
        NestlyToast.success(
          context,
          'Unos ažuriran',
          accentColor: AppColors.seed,
        );
      }

      Navigator.pop(context);
    } catch (_) {
      if (!mounted) return;
      NestlyToast.error(context, 'Greška pri spremanju unosa');
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
          widget.existingLog == null ? 'Dodaj unos' : 'Uredi unos',
          style: Theme.of(context).textTheme.titleLarge?.copyWith(
            fontWeight: FontWeight.w700,
            color: AppColors.seed,
          ),
        ),
        centerTitle: true,
      ),

      body: Padding(
        padding: const EdgeInsets.all(AppSpacing.lg),
        child: Form(
          key: _formKey,
          child: Column(
            children: [
              InkWell(
                onTap: _pickTime,
                child: InputDecorator(
                  decoration: _decoration(
                    label: 'Vrijeme',
                    icon: Icons.schedule,
                  ),
                  child: Text(_formatTime(_time)),
                ),
              ),
              const SizedBox(height: 12),
              TextFormField(
                controller: _amount,
                keyboardType: const TextInputType.numberWithOptions(
                  decimal: true,
                ),
                inputFormatters: [
                  FilteringTextInputFormatter.allow(RegExp(r'^\d*\.?\d{0,2}$')),
                ],
                decoration: _decoration(
                  label: 'Količina (ml/g)',
                  icon: Icons.scale,
                ),
                validator: (value) {
                  if (value == null || value.trim().isEmpty) {
                    return 'Unesite količinu';
                  }
                  final normalized = value.replaceAll(',', '.');
                  if (double.tryParse(normalized) == null) {
                    return 'Dozvoljeni su samo brojevi';
                  }
                  return null;
                },
              ),
              const SizedBox(height: 12),
              TextField(
                controller: _notes,
                decoration: _decoration(
                  label: 'Napomena (opcionalno)',
                  icon: Icons.note,
                ),
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
                      : Text(
                          widget.existingLog == null
                              ? 'Sačuvaj'
                              : 'Sačuvaj izmjene',
                        ),
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }
}

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
