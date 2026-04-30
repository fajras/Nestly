import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:flutter_application_nestly/network/api_client.dart';
import 'package:flutter_application_nestly/layouts/nestly_calendar.dart';
import 'package:flutter_application_nestly/layouts/nestly_toast.dart';
import 'package:flutter_application_nestly/main.dart';

class SymptomDiaryEntry {
  final int id;
  final DateTime date;
  final Map<String, int> values;

  SymptomDiaryEntry({
    required this.id,
    required this.date,
    required this.values,
  });

  factory SymptomDiaryEntry.fromJson(Map<String, dynamic> json) {
    int _i(dynamic v) => v == null ? 0 : (v as num).toInt();

    return SymptomDiaryEntry(
      id: json['id'],
      date: DateTime.parse(json['date']),
      values: {
        'mucnina': _i(json['nausea']),
        'umor': _i(json['fatigue']),
        'glavobolja': _i(json['headache']),
        'zgaravica': _i(json['heartburn']),
        'oticanje': _i(json['legSwelling']),
      },
    );
  }
}

class SymptomDiaryApiService {
  String _fmt(DateTime d) =>
      '${d.year}-${d.month.toString().padLeft(2, '0')}-${d.day.toString().padLeft(2, '0')}';

  Future<SymptomDiaryEntry?> getByDate(
    int parentProfileId,
    DateTime date,
  ) async {
    final res = await ApiClient.get(
      '/api/SymptomDiary/by-date'
      '?parentProfileId=$parentProfileId'
      '&date=${_fmt(date)}',
    );

    if (res.statusCode == 404) return null;
    if (res.statusCode != 200) {
      throw Exception('Failed to load symptom diary entry');
    }

    return SymptomDiaryEntry.fromJson(jsonDecode(res.body));
  }

  Future<Set<DateTime>> getMarkedDays(int parentProfileId) async {
    int page = 1;
    const pageSize = 100;

    Set<DateTime> result = {};

    while (true) {
      final res = await ApiClient.get(
        '/api/SymptomDiary/marked-days'
        '?parentProfileId=$parentProfileId&page=$page&pageSize=$pageSize',
      );

      if (res.statusCode != 200) break;

      final data = jsonDecode(res.body);
      final List items = data['items'];

      if (items.isEmpty) break;

      final parsed = items
          .map<DateTime>((e) => DateTime.parse(e as String))
          .map((d) => DateTime(d.year, d.month, d.day));

      result.addAll(parsed);

      if (items.length < pageSize) break;

      page++;
    }

    return result;
  }

  Future<int> create(
    int parentProfileId,
    DateTime date,
    Map<String, int> values,
  ) async {
    final res = await ApiClient.post(
      '/api/SymptomDiary',
      body: {
        'parentProfileId': parentProfileId,
        'date': _fmt(date),
        ..._mapToApi(values),
      },
    );

    if (res.statusCode != 201) {
      throw Exception('Failed to create symptom diary entry');
    }

    return jsonDecode(res.body)['id'] as int;
  }

  Future<void> update(int id, Map<String, int> values) async {
    final res = await ApiClient.patch(
      '/api/SymptomDiary/$id',
      body: _mapToApi(values),
    );

    if (res.statusCode != 200 && res.statusCode != 204) {
      throw Exception('Failed to update symptom diary entry');
    }
  }

  Map<String, int> _mapToApi(Map<String, int> v) => {
    'nausea': v['mucnina'] ?? 0,
    'fatigue': v['umor'] ?? 0,
    'headache': v['glavobolja'] ?? 0,
    'heartburn': v['zgaravica'] ?? 0,
    'legSwelling': v['oticanje'] ?? 0,
  };
}

class SymptomDiaryScreen extends StatefulWidget {
  const SymptomDiaryScreen({super.key, required this.parentProfileId});

  final int parentProfileId;

  @override
  State<SymptomDiaryScreen> createState() => _SymptomDiaryScreenState();
}

class _SymptomDiaryScreenState extends State<SymptomDiaryScreen> {
  final _service = SymptomDiaryApiService();

  final Map<String, String> _labels = const {
    'mucnina': 'Mučnina',
    'umor': 'Umor',
    'glavobolja': 'Glavobolja',
    'zgaravica': 'Žgaravica',
    'oticanje': 'Oticanje nogu',
  };

  DateTime _focusedDay = DateTime.now();
  DateTime _selectedDay = DateTime.now();

  int? _entryId;
  bool _loading = true;
  bool _saving = false;

  Set<DateTime> _markedDays = {};
  late Map<String, int> _values;

  @override
  void initState() {
    super.initState();
    _values = {for (final k in _labels.keys) k: 0};
    _loadAll();
  }

  bool get _canEdit {
    final diff = DateTime.now().difference(
      DateTime(_selectedDay.year, _selectedDay.month, _selectedDay.day),
    );
    return diff.inDays >= 0 && diff.inDays <= 7;
  }

  Future<void> _loadAll() async {
    setState(() => _loading = true);

    try {
      if (_markedDays.isEmpty) {
        _markedDays = await _service.getMarkedDays(widget.parentProfileId);
      }

      if (!mounted) return;

      final entry = await _service.getByDate(
        widget.parentProfileId,
        _selectedDay,
      );

      if (!mounted) return;

      if (entry == null) {
        _entryId = null;
        _values.updateAll((_, __) => 0);
      } else {
        _entryId = entry.id;
        _values = Map.of(entry.values);
      }
    } catch (_) {
      NestlyToast.error(context, 'Greška pri učitavanju');
    }

    if (mounted) setState(() => _loading = false);
  }

  Future<void> _save() async {
    if (!_canEdit) {
      NestlyToast.warning(context, 'Izmjene su moguće samo za zadnjih 7 dana.');
      return;
    }

    final hasEmpty = _values.values.any((v) => v == 0);

    if (hasEmpty) {
      NestlyToast.warning(
        context,
        'Sva polja su obavezna. Odaberite intenzitet za svaki simptom.',
      );
      return;
    }

    setState(() => _saving = true);

    try {
      if (_entryId == null) {
        _entryId = await _service.create(
          widget.parentProfileId,
          _selectedDay,
          _values,
        );
        NestlyToast.success(context, 'Unos spremljen');
      } else {
        await _service.update(_entryId!, _values);
        NestlyToast.success(context, 'Izmjene sačuvane');
      }

      _markedDays = await _service.getMarkedDays(widget.parentProfileId);
    } catch (_) {
      NestlyToast.error(context, 'Greška pri spremanju');
    }

    if (mounted) setState(() => _saving = false);
  }

  String _fmtDate(DateTime d) => '${d.day}.${d.month}.${d.year}';
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
          'Dnevnik simptoma',
          style: Theme.of(context).textTheme.titleLarge?.copyWith(
            fontWeight: FontWeight.w700,
            color: AppColors.roseDark,
          ),
        ),
        centerTitle: true,
      ),

      body: _loading
          ? const Center(
              child: CircularProgressIndicator(color: AppColors.roseDark),
            )
          : SingleChildScrollView(
              padding: const EdgeInsets.all(AppSpacing.lg),
              child: Column(
                children: [
                  NestlyCalendar(
                    focusedDay: _focusedDay,
                    selectedDay: _selectedDay,
                    markerIcon: Icons.favorite_rounded,
                    eventLoader: (day) {
                      final d = _dayOnly(day);
                      return _markedDays.contains(d) ? const [1] : const [];
                    },
                    onDaySelected: (selected, focused) async {
                      setState(() {
                        _selectedDay = _dayOnly(selected);
                        _focusedDay = _dayOnly(focused);
                      });
                      await _loadAll();
                    },
                  ),

                  const SizedBox(height: AppSpacing.lg),
                  Text(
                    'Unosi za ${_fmtDate(_selectedDay)}',
                    style: const TextStyle(
                      fontWeight: FontWeight.w700,
                      color: AppColors.roseDark,
                    ),
                  ),
                  const SizedBox(height: AppSpacing.lg),
                  _SymptomsCard(
                    labels: _labels,
                    values: _values,
                    enabled: _canEdit,
                    onChanged: (k, v) => setState(() => _values[k] = v),
                  ),
                  const SizedBox(height: AppSpacing.xl),
                  _SaveButton(
                    enabled: _canEdit && !_saving,
                    saving: _saving,
                    onTap: _save,
                  ),
                ],
              ),
            ),
    );
  }
}

class _SymptomsCard extends StatelessWidget {
  const _SymptomsCard({
    required this.labels,
    required this.values,
    required this.enabled,
    required this.onChanged,
  });

  final Map<String, String> labels;
  final Map<String, int> values;
  final bool enabled;
  final void Function(String, int) onChanged;

  @override
  Widget build(BuildContext context) {
    return Card(
      shape: RoundedRectangleBorder(
        borderRadius: BorderRadius.circular(AppRadius.xl),
      ),
      child: Padding(
        padding: const EdgeInsets.all(AppSpacing.xl),
        child: Column(
          children: labels.keys.map((k) {
            return Column(
              children: [
                _IntensityTile(
                  label: labels[k]!,
                  value: values[k]!,
                  enabled: enabled,
                  onChanged: (v) => onChanged(k, v),
                ),
                if (k != labels.keys.last)
                  Divider(color: Colors.black.withOpacity(.07)),
              ],
            );
          }).toList(),
        ),
      ),
    );
  }
}

class _IntensityTile extends StatelessWidget {
  const _IntensityTile({
    required this.label,
    required this.value,
    required this.enabled,
    required this.onChanged,
  });

  final String label;
  final int value;
  final bool enabled;
  final ValueChanged<int> onChanged;

  @override
  Widget build(BuildContext context) {
    return Opacity(
      opacity: enabled ? 1 : .6,
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Text(
            label,
            style: Theme.of(context).textTheme.titleMedium?.copyWith(
              fontWeight: FontWeight.w700,
              color: AppColors.roseDark,
            ),
          ),

          const SizedBox(height: 6),
          Row(
            children: List.generate(5, (i) {
              final v = i + 1;
              final selected = value == v;
              return Expanded(
                child: InkWell(
                  onTap: enabled ? () => onChanged(v) : null,
                  child: Container(
                    height: 40,
                    margin: const EdgeInsets.symmetric(horizontal: 4),
                    alignment: Alignment.center,
                    decoration: BoxDecoration(
                      color: selected
                          ? AppColors.babyPink
                          : AppColors.babyBlue.withOpacity(.18),
                      borderRadius: BorderRadius.circular(10),
                    ),
                    child: Text(
                      '$v',
                      style: TextStyle(
                        fontWeight: FontWeight.w700,
                        color: selected ? Colors.white : AppColors.roseDark,
                      ),
                    ),
                  ),
                ),
              );
            }),
          ),
        ],
      ),
    );
  }
}

class _SaveButton extends StatelessWidget {
  const _SaveButton({
    required this.enabled,
    required this.saving,
    required this.onTap,
  });

  final bool enabled;
  final bool saving;
  final VoidCallback onTap;

  @override
  Widget build(BuildContext context) {
    return SizedBox(
      height: 52,
      width: 120,
      child: ElevatedButton(
        style: ElevatedButton.styleFrom(
          backgroundColor: AppColors.roseDark,
          foregroundColor: Colors.white,
          disabledBackgroundColor: AppColors.roseDark.withOpacity(.4),
          disabledForegroundColor: Colors.white70,
          elevation: 0,
          shape: RoundedRectangleBorder(
            borderRadius: BorderRadius.circular(AppRadius.lg),
          ),
          textStyle: const TextStyle(fontWeight: FontWeight.w700),
        ),
        onPressed: enabled ? onTap : null,
        child: saving
            ? const CircularProgressIndicator(color: Colors.white)
            : const Text('Sačuvaj'),
      ),
    );
  }
}
