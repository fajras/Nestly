import 'package:flutter/material.dart';
import 'package:flutter_application_nestly/layouts/nestly_toast.dart';
import 'package:flutter_application_nestly/main.dart';

class SymptomDiaryScreen extends StatefulWidget {
  const SymptomDiaryScreen({super.key});

  @override
  State<SymptomDiaryScreen> createState() => _SymptomDiaryScreenState();
}

class _SymptomDiaryScreenState extends State<SymptomDiaryScreen> {
  final _service = _MockSymptomDiaryService(); // ⬅️ zamijeni stvarnim servisom
  late DateTime _date;
  bool _loading = true;
  bool _saving = false;
  String? _entryId; // ako postoji unos za taj datum -> PATCH, inače POST

  // skala 0..5 (0 = nije zabilježeno)
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

  int _asInt05(dynamic raw) {
    // prihvati int/double/string/null i vrati int u rasponu 0..5
    if (raw == null) return 0;
    if (raw is int) return raw.clamp(0, 5);
    if (raw is num) return raw.toInt().clamp(0, 5);
    if (raw is String) {
      final p = int.tryParse(raw) ?? 0;
      return p.clamp(0, 5);
    }
    return 0;
  }

  // dozvola uređivanja: samo ako je datum u prošlosti najviše 7 dana (i ne u budućnosti)
  bool get _canEdit {
    final now = DateTime.now();
    final diff = now
        .difference(DateTime(_date.year, _date.month, _date.day))
        .inDays;
    // diff < 0 => budućnost (zabrani), diff==0 danas (dozvoli), diff 1..7 (dozvoli), >7 (zabrani)
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
    // dozvoli biranje npr. zadnjih 300 dana do danas (možeš prilagoditi)
    final now = DateTime.now();
    final first = now.subtract(const Duration(days: 300));
    final picked = await showDatePicker(
      context: context,
      initialDate: _date,
      firstDate: first,
      lastDate: now,
      helpText: 'Odaberite datum',
      builder: (context, child) {
        // lagani zaobljeni stil
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
    final res = await _service.getByDate(
      _date,
    ); // GET /symptoms?date=yyyy-mm-dd
    if (!mounted) return;

    if (res == null) {
      // nema zapisa: sve 0
      for (final s in _symptoms) {
        s.value = 0;
      }
      _entryId = null;
    } else {
      _entryId = res.id;
      final map = res.values; // može sadržati null/double/string
      for (final s in _symptoms) {
        s.value = _asInt05(map[s.key]);
      }
    }
    setState(() => _loading = false);
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

    if (_entryId == null) {
      final created = await _service.create(_date, values); // POST
      _entryId = created.id;
      if (mounted) {
        NestlyToast.success(context, 'Unos spremljen.');
      }
    } else {
      await _service.update(_entryId!, values); // PATCH
      NestlyToast.success(context, 'Izmjene sačuvane.');
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
            color: AppColors.textPrimary,
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
                // datum čip (tap = promijeni datum)
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

                // Banner: samo pregled
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

                // kartica sa skalama
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

                // dugme Dodaj / Sačuvaj (disabled kad se ne može uređivati)
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

/* ─────────────  UI helpers  ───────────── */

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

  final int value; // 0..5
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

class _SymptomItem {
  final String key;
  final String label;
  int value; // 0..5
  _SymptomItem({required this.key, required this.label, this.value = 0});
}

/* ─────────────  MOCK SERVICE (zamijeni pravim API-jem)  ───────────── */
class _SymptomEntry {
  final String id;
  final DateTime date;
  final Map<String, int> values;
  _SymptomEntry({required this.id, required this.date, required this.values});
}

class _MockSymptomDiaryService {
  final Map<String, _SymptomEntry> _store = {};

  String _key(DateTime d) {
    final y = d.year.toString();
    final m = d.month.toString().padLeft(2, '0');
    final day = d.day.toString().padLeft(2, '0');
    return '$y-$m-$day';
  }

  Future<_SymptomEntry?> getByDate(DateTime date) async {
    await Future.delayed(const Duration(milliseconds: 250));
    return _store[_key(date)];
    // PRAVI API: GET /symptoms?date=yyyy-MM-dd
  }

  Future<_SymptomEntry> create(DateTime date, Map<String, int> values) async {
    await Future.delayed(const Duration(milliseconds: 250));
    final id = DateTime.now().millisecondsSinceEpoch.toString();
    final entry = _SymptomEntry(id: id, date: date, values: Map.of(values));
    _store[_key(date)] = entry;
    // PRAVI API: POST /symptoms { date, values }
    return entry;
  }

  Future<void> update(String id, Map<String, int> values) async {
    await Future.delayed(const Duration(milliseconds: 200));
    // PRAVI API: PATCH /symptoms/{id} { values }
    final kv = _store.entries.firstWhere(
      (e) => e.value.id == id,
      orElse: () => throw Exception('Not found'),
    );
    _store[kv.key] = _SymptomEntry(
      id: id,
      date: kv.value.date,
      values: Map.of(values),
    );
  }
}
