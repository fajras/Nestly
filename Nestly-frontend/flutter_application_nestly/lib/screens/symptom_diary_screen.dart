import 'dart:convert';
import 'dart:io' show Platform;

import 'package:flutter/foundation.dart' show kIsWeb;
import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;

import 'package:flutter_application_nestly/layouts/nestly_toast.dart';
import 'package:flutter_application_nestly/main.dart';

Map<String, String> defaultHeaders({String? token}) => {
  'Content-Type': 'application/json',
  'Accept': 'application/json',
  if (token != null) 'Authorization': 'Bearer $token',
};

String _devBase() {
  if (kIsWeb) return 'http://localhost:5167';
  if (Platform.isAndroid) return 'http://10.0.2.2:5167';
  if (Platform.isIOS || Platform.isMacOS) return 'http://localhost:5167';
  return 'http://localhost:5167';
}

String get kApiBase =>
    const String.fromEnvironment('API_BASE', defaultValue: '').isNotEmpty
    ? const String.fromEnvironment('API_BASE')
    : _devBase();

String get kSymptomDiaryBase => '$kApiBase/api/SymptomDiary';

class SymptomDiaryEntry {
  final int id;
  final int parentProfileId;
  final DateTime date;
  final int? nausea;
  final int? fatigue;
  final int? headache;
  final int? heartburn;
  final int? legSwelling;

  SymptomDiaryEntry({
    required this.id,
    required this.parentProfileId,
    required this.date,
    this.nausea,
    this.fatigue,
    this.headache,
    this.heartburn,
    this.legSwelling,
  });

  factory SymptomDiaryEntry.fromJson(Map<String, dynamic> json) {
    T? _get<T>(String camel, String pascal) {
      final v = json[camel] ?? json[pascal];
      if (v == null) return null;
      if (T == int) return (v as num).toInt() as T;
      if (T == DateTime) return DateTime.parse(v.toString()) as T;
      return v as T;
    }

    final id = _get<int>('id', 'Id');
    final parentId = _get<int>('parentProfileId', 'ParentProfileId');
    final date = _get<DateTime>('date', 'Date');

    if (id == null || parentId == null || date == null) {
      throw Exception('Neispravan SymptomDiary JSON: $json');
    }

    return SymptomDiaryEntry(
      id: id,
      parentProfileId: parentId,
      date: date,
      nausea: _get<int>('nausea', 'Nausea'),
      fatigue: _get<int>('fatigue', 'Fatigue'),
      headache: _get<int>('headache', 'Headache'),
      heartburn: _get<int>('heartburn', 'Heartburn'),
      legSwelling: _get<int>('legSwelling', 'LegSwelling'),
    );
  }
}

class SymptomDiaryApiService {
  String get _baseUrl => kSymptomDiaryBase;

  String _formatDate(DateTime d) {
    final y = d.year.toString().padLeft(4, '0');
    final m = d.month.toString().padLeft(2, '0');
    final day = d.day.toString().padLeft(2, '0');
    return '$y-$m-$day';
  }

  Future<SymptomDiaryEntry?> getByDate(
    int parentProfileId,
    DateTime date,
  ) async {
    final formatted = _formatDate(date);
    final uri = Uri.parse('$_baseUrl/by-date').replace(
      queryParameters: {
        'parentProfileId': parentProfileId.toString(),
        'date': formatted,
      },
    );

    final res = await http
        .get(uri, headers: defaultHeaders())
        .timeout(const Duration(seconds: 10));

    if (res.statusCode == 404) return null;

    if (res.statusCode != 200) {
      throw Exception(
        'Failed to load symptom diary (${res.statusCode}): ${res.body}',
      );
    }

    final data = jsonDecode(res.body) as Map<String, dynamic>;
    return SymptomDiaryEntry.fromJson(data);
  }

  Future<SymptomDiaryEntry> create(
    int parentProfileId,
    DateTime date,
    Map<String, int> values,
  ) async {
    final formatted = _formatDate(date);

    final body = {
      'parentProfileId': parentProfileId,
      'date': formatted,
      'nausea': values['mucnina'],
      'fatigue': values['umor'],
      'headache': values['glavobolja'],
      'heartburn': values['zgaravica'],
      'legSwelling': values['oticanje'],
    };

    final uri = Uri.parse(_baseUrl);

    final res = await http
        .post(uri, headers: defaultHeaders(), body: jsonEncode(body))
        .timeout(const Duration(seconds: 10));

    if (res.statusCode != 201) {
      throw Exception(
        'Failed to create symptom diary (${res.statusCode}): ${res.body}',
      );
    }

    final data = jsonDecode(res.body) as Map<String, dynamic>;
    return SymptomDiaryEntry.fromJson(data);
  }

  Future<void> update(int id, Map<String, int> values) async {
    final body = {
      'nausea': values['mucnina'],
      'fatigue': values['umor'],
      'headache': values['glavobolja'],
      'heartburn': values['zgaravica'],
      'legSwelling': values['oticanje'],
    };

    final uri = Uri.parse('$_baseUrl/$id');

    final res = await http
        .patch(uri, headers: defaultHeaders(), body: jsonEncode(body))
        .timeout(const Duration(seconds: 10));

    if (res.statusCode != 200 && res.statusCode != 204) {
      throw Exception(
        'Failed to update symptom diary (${res.statusCode}): ${res.body}',
      );
    }
  }
}

class SymptomDiaryScreen extends StatefulWidget {
  final int parentProfileId;

  const SymptomDiaryScreen({super.key, required this.parentProfileId});

  @override
  State<SymptomDiaryScreen> createState() => _SymptomDiaryScreenState();
}

class _SymptomDiaryScreenState extends State<SymptomDiaryScreen> {
  final _service = SymptomDiaryApiService();

  late DateTime _date;
  bool _loading = true;
  bool _saving = false;
  int? _entryId;

  final List<_SymptomItem> _symptoms = [
    _SymptomItem(key: 'mucnina', label: 'Mučnina'),
    _SymptomItem(key: 'umor', label: 'Umor'),
    _SymptomItem(key: 'glavobolja', label: 'Glavobolja'),
    _SymptomItem(key: 'zgaravica', label: 'Žgaravica'),
    _SymptomItem(key: 'oticanje', label: 'Oticanje nogu'),
  ];

  @override
  void initState() {
    super.initState();
    _date = DateTime.now();
    _load();
  }

  int _asInt05(int? v) {
    if (v == null) return 0;
    if (v < 0) return 0;
    if (v > 5) return 5;
    return v;
  }

  bool get _canEdit {
    final now = DateTime.now();
    final diff = now
        .difference(DateTime(_date.year, _date.month, _date.day))
        .inDays;
    return diff >= 0 && diff <= 7;
  }

  String _formatBosnianDate(DateTime d) {
    final months = [
      'januar',
      'februar',
      'mart',
      'april',
      'maj',
      'juni',
      'juli',
      'august',
      'septembar',
      'oktobar',
      'novembar',
      'decembar',
    ];
    return '${d.day}. ${months[d.month - 1]}';
  }

  Future<void> _pickDate() async {
    final now = DateTime.now();
    final first = now.subtract(const Duration(days: 300));
    final picked = await showDatePicker(
      context: context,
      initialDate: _date,
      firstDate: first,
      lastDate: now,
      helpText: 'Odaberite datum',
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
        _date = DateTime(picked.year, picked.month, picked.day);
      });
      await _load();
    }
  }

  Future<void> _load() async {
    setState(() => _loading = true);

    try {
      final res = await _service.getByDate(widget.parentProfileId, _date);

      if (!mounted) return;

      if (res == null) {
        _entryId = null;
        for (final s in _symptoms) {
          s.value = 0;
        }
      } else {
        _entryId = res.id;

        for (final s in _symptoms) {
          switch (s.key) {
            case 'mucnina':
              s.value = _asInt05(res.nausea);
              break;
            case 'umor':
              s.value = _asInt05(res.fatigue);
              break;
            case 'glavobolja':
              s.value = _asInt05(res.headache);
              break;
            case 'zgaravica':
              s.value = _asInt05(res.heartburn);
              break;
            case 'oticanje':
              s.value = _asInt05(res.legSwelling);
              break;
            default:
              s.value = 0;
          }
        }
      }
    } catch (e) {
      if (mounted) {
        NestlyToast.error(context, 'Greška pri učitavanju dnevnika simptoma.');
      }
    }

    if (mounted) setState(() => _loading = false);
  }

  Future<void> _save() async {
    if (!_canEdit) {
      NestlyToast.warning(
        context,
        'Unos/izmjene su dostupni samo za datume unutar zadnjih 7 dana.',
      );
      return;
    }

    setState(() => _saving = true);

    final values = {for (final s in _symptoms) s.key: s.value};

    try {
      if (_entryId == null) {
        final created = await _service.create(
          widget.parentProfileId,
          _date,
          values,
        );
        _entryId = created.id;
        if (mounted) {
          NestlyToast.success(context, 'Unos spremljen.');
        }
      } else {
        await _service.update(_entryId!, values);
        if (mounted) {
          NestlyToast.success(context, 'Izmjene sačuvane.');
        }
      }
    } catch (e) {
      if (mounted) {
        NestlyToast.error(context, 'Greška pri spremanju podataka.');
      }
    }

    if (mounted) setState(() => _saving = false);
  }

  @override
  Widget build(BuildContext context) {
    final dateLabel = _formatBosnianDate(_date);
    final isExisting = _entryId != null;

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
          'Dnevnik simptoma',
          style: Theme.of(context).textTheme.titleLarge?.copyWith(
            fontWeight: FontWeight.w800,
            color: AppColors.roseDark,
          ),
        ),
        centerTitle: true,
      ),
      body: Center(
        child: SingleChildScrollView(
          padding: const EdgeInsets.all(AppSpacing.xl),
          child: ConstrainedBox(
            constraints: const BoxConstraints(maxWidth: 520),
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.stretch,
              children: [
                InkWell(
                  onTap: _pickDate,
                  borderRadius: BorderRadius.circular(AppRadius.lg),
                  child: Ink(
                    padding: const EdgeInsets.symmetric(
                      horizontal: 14,
                      vertical: 10,
                    ),
                    decoration: BoxDecoration(
                      color: AppColors.babyBlue.withOpacity(.25),
                      borderRadius: BorderRadius.circular(AppRadius.lg),
                    ),
                    child: Row(
                      mainAxisAlignment: MainAxisAlignment.center,
                      children: [
                        const Icon(
                          Icons.calendar_today_rounded,
                          size: 18,
                          color: AppColors.roseDark,
                        ),
                        const SizedBox(width: 8),
                        Text(
                          dateLabel,
                          style: Theme.of(context).textTheme.titleMedium
                              ?.copyWith(
                                fontWeight: FontWeight.w700,
                                color: AppColors.roseDark,
                              ),
                        ),
                        const SizedBox(width: 8),
                        const Icon(
                          Icons.edit_calendar_rounded,
                          size: 18,
                          color: AppColors.roseDark,
                        ),
                      ],
                    ),
                  ),
                ),

                const SizedBox(height: AppSpacing.md),

                if (!_canEdit)
                  Container(
                    margin: const EdgeInsets.only(top: 6),
                    padding: const EdgeInsets.symmetric(
                      horizontal: 12,
                      vertical: 10,
                    ),
                    decoration: BoxDecoration(
                      color: Colors.orange.withOpacity(.12),
                      borderRadius: BorderRadius.circular(AppRadius.md),
                      border: Border.all(color: Colors.orange.withOpacity(.35)),
                    ),
                    child: Row(
                      children: [
                        const Icon(
                          Icons.lock_clock_rounded,
                          color: Colors.orange,
                        ),
                        const SizedBox(width: 8),
                        Expanded(
                          child: Text(
                            'Unose starije od 7 dana nije moguće mijenjati.',
                            style: Theme.of(context).textTheme.bodySmall
                                ?.copyWith(
                                  color: Colors.orange.shade900,
                                  fontWeight: FontWeight.w600,
                                ),
                          ),
                        ),
                      ],
                    ),
                  ),

                const SizedBox(height: AppSpacing.xl),

                Card(
                  elevation: 3,
                  shape: RoundedRectangleBorder(
                    borderRadius: BorderRadius.circular(AppRadius.xl),
                  ),
                  color: Colors.white,
                  shadowColor: AppColors.babyPink.withOpacity(0.35),
                  child: Padding(
                    padding: const EdgeInsets.all(AppSpacing.xl),
                    child: _loading
                        ? const Padding(
                            padding: EdgeInsets.all(24),
                            child: Center(
                              child: CircularProgressIndicator(
                                color: AppColors.roseDark,
                              ),
                            ),
                          )
                        : Column(
                            children: [
                              for (int i = 0; i < _symptoms.length; i++) ...[
                                _IntensityTile(
                                  item: _symptoms[i],
                                  enabled: _canEdit,
                                  onChanged: (v) =>
                                      setState(() => _symptoms[i].value = v),
                                ),
                                if (i != _symptoms.length - 1)
                                  Divider(
                                    height: 22,
                                    thickness: .6,
                                    color: Colors.black.withOpacity(0.07),
                                  ),
                              ],
                            ],
                          ),
                  ),
                ),

                const SizedBox(height: AppSpacing.xl),

                SizedBox(
                  height: 52,
                  child: ElevatedButton(
                    style: ElevatedButton.styleFrom(
                      elevation: 0,
                      backgroundColor: _canEdit
                          ? AppColors.roseDark
                          : AppColors.roseDark.withOpacity(.45),
                      foregroundColor: Colors.white,
                      shape: RoundedRectangleBorder(
                        borderRadius: BorderRadius.circular(AppRadius.lg),
                      ),
                      textStyle: const TextStyle(fontWeight: FontWeight.w700),
                    ),
                    onPressed: (_loading || _saving || !_canEdit)
                        ? null
                        : _save,
                    child: _saving
                        ? const SizedBox(
                            width: 22,
                            height: 22,
                            child: CircularProgressIndicator(
                              strokeWidth: 2.4,
                              valueColor: AlwaysStoppedAnimation<Color>(
                                Colors.white,
                              ),
                            ),
                          )
                        : Text(isExisting ? 'Sačuvaj izmjene' : 'Dodaj'),
                  ),
                ),
              ],
            ),
          ),
        ),
      ),
    );
  }
}

class _SymptomItem {
  final String key;
  final String label;
  int value;

  _SymptomItem({required this.key, required this.label, this.value = 0});
}

class _IntensityTile extends StatelessWidget {
  const _IntensityTile({
    required this.item,
    required this.onChanged,
    required this.enabled,
  });

  final _SymptomItem item;
  final ValueChanged<int> onChanged;
  final bool enabled;

  String _labelFor(int v) {
    switch (v) {
      case 1:
        return 'Blago';
      case 2:
        return 'Lagano';
      case 3:
        return 'Umjereno';
      case 4:
        return 'Jako';
      case 5:
        return 'Vrlo jako';
      default:
        return 'Nije zabilježeno';
    }
  }

  @override
  Widget build(BuildContext context) {
    final chips = _IntensityChips(
      value: item.value,
      enabled: enabled,
      onChanged: onChanged,
    );

    return Opacity(
      opacity: enabled ? 1.0 : 0.6,
      child: Column(
        children: [
          Row(
            children: [
              Icon(
                Icons.local_hospital_rounded,
                size: 18,
                color: AppColors.babyPink,
              ),
              const SizedBox(width: AppSpacing.sm),
              Expanded(
                child: Text(
                  item.label,
                  style: Theme.of(context).textTheme.titleMedium?.copyWith(
                    color: AppColors.textPrimary,
                    fontWeight: FontWeight.w700,
                  ),
                ),
              ),
              Text(
                item.value == 0 ? '-' : item.value.toString(),
                style: Theme.of(context).textTheme.titleMedium?.copyWith(
                  color: AppColors.roseDark,
                  fontWeight: FontWeight.w700,
                ),
              ),
            ],
          ),
          const SizedBox(height: AppSpacing.sm),
          chips,
          const SizedBox(height: 8),
          Align(
            alignment: Alignment.centerLeft,
            child: Text(
              _labelFor(item.value),
              style: Theme.of(
                context,
              ).textTheme.bodySmall?.copyWith(color: AppColors.textSecondary),
            ),
          ),
        ],
      ),
    );
  }
}

class _IntensityChips extends StatelessWidget {
  const _IntensityChips({
    required this.value,
    required this.onChanged,
    required this.enabled,
  });

  final int value;
  final ValueChanged<int> onChanged;
  final bool enabled;

  @override
  Widget build(BuildContext context) {
    final items = List<int>.generate(5, (i) => i + 1);
    return Row(
      mainAxisAlignment: MainAxisAlignment.spaceBetween,
      children: items.map((v) {
        final selected = value == v;
        return Expanded(
          child: Padding(
            padding: const EdgeInsets.symmetric(horizontal: 4),
            child: InkWell(
              borderRadius: BorderRadius.circular(10),
              onTap: enabled ? () => onChanged(v) : null,
              child: AnimatedContainer(
                duration: const Duration(milliseconds: 140),
                height: 40,
                decoration: BoxDecoration(
                  borderRadius: BorderRadius.circular(10),
                  color: selected
                      ? AppColors.babyPink
                      : AppColors.babyBlue.withOpacity(.18),
                  border: Border.all(
                    color: selected
                        ? AppColors.roseDark
                        : Colors.black.withOpacity(.05),
                    width: selected ? 1.2 : 1,
                  ),
                  boxShadow: selected
                      ? [
                          BoxShadow(
                            color: AppColors.babyPink.withOpacity(.35),
                            blurRadius: 10,
                            offset: const Offset(0, 4),
                          ),
                        ]
                      : [],
                ),
                alignment: Alignment.center,
                child: Text(
                  v.toString(),
                  style: Theme.of(context).textTheme.titleMedium?.copyWith(
                    fontWeight: FontWeight.w700,
                    color: selected ? Colors.white : AppColors.roseDark,
                  ),
                ),
              ),
            ),
          ),
        );
      }).toList(),
    );
  }
}
