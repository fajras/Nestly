import 'dart:convert';
import 'package:fl_chart/fl_chart.dart';
import 'package:flutter/material.dart';
import 'package:flutter_application_nestly/layouts/nestly_toast.dart'
    show NestlyToast;
import 'package:flutter_application_nestly/network/api_client.dart'
    show ApiClient;
import 'package:flutter_application_nestly/main.dart';
import 'package:flutter/services.dart';

class BabyGrowthEntry {
  final int id;
  final int babyId;
  final int weekNumber;
  final double? weightKg;
  final double? heightCm;
  final double? headCircumferenceCm;

  const BabyGrowthEntry({
    required this.id,
    required this.babyId,
    required this.weekNumber,
    this.weightKg,
    this.heightCm,
    this.headCircumferenceCm,
  });

  factory BabyGrowthEntry.fromJson(Map<String, dynamic> json) {
    return BabyGrowthEntry(
      id: json['id'] as int,
      babyId: json['babyId'] as int,
      weekNumber: json['weekNumber'] as int,
      weightKg: (json['weightKg'] as num?)?.toDouble(),
      heightCm: (json['heightCm'] as num?)?.toDouble(),
      headCircumferenceCm: (json['headCircumferenceCm'] as num?)?.toDouble(),
    );
  }
}

class CreateBabyGrowthRequest {
  final int babyId;
  final int weekNumber;
  final double? weightKg;
  final double? heightCm;
  final double? headCircumferenceCm;

  const CreateBabyGrowthRequest({
    required this.babyId,
    required this.weekNumber,
    this.weightKg,
    this.heightCm,
    this.headCircumferenceCm,
  });

  Map<String, dynamic> toJson() => {
    'babyId': babyId,
    'weekNumber': weekNumber,
    'weightKg': weightKg,
    'heightCm': heightCm,
    'headCircumferenceCm': headCircumferenceCm,
  };
}

class BabyGrowthPatchRequest {
  final double? weightKg;
  final double? heightCm;
  final double? headCircumferenceCm;

  const BabyGrowthPatchRequest({
    this.weightKg,
    this.heightCm,
    this.headCircumferenceCm,
  });

  Map<String, dynamic> toJson() => {
    'weightKg': weightKg,
    'heightCm': heightCm,
    'headCircumferenceCm': headCircumferenceCm,
  };
}

class BabyGrowthApiService {
  static const String _basePath = '/api/BabyGrowth';

  static final Map<int, List<BabyGrowthEntry>> _cache = {};

  Future<List<BabyGrowthEntry>> getForBaby({required int babyId}) async {
    if (_cache.containsKey(babyId)) {
      return _cache[babyId]!;
    }

    final resp = await ApiClient.get('$_basePath?BabyId=$babyId');

    if (resp.statusCode != 200) {
      throw Exception('Failed to load baby growth data');
    }

    final List<dynamic> data = jsonDecode(resp.body);
    final list = data.map((e) => BabyGrowthEntry.fromJson(e)).toList()
      ..sort((a, b) => a.weekNumber.compareTo(b.weekNumber));

    _cache[babyId] = list;
    return list;
  }

  Future<BabyGrowthEntry> create({
    required CreateBabyGrowthRequest request,
  }) async {
    final resp = await ApiClient.post(_basePath, body: request.toJson());

    if (resp.statusCode != 201 && resp.statusCode != 200) {
      throw Exception('Failed to create baby growth entry');
    }

    return BabyGrowthEntry.fromJson(jsonDecode(resp.body));
  }

  Future<BabyGrowthEntry> patch({
    required int id,
    required BabyGrowthPatchRequest patch,
  }) async {
    final resp = await ApiClient.patch('$_basePath/$id', body: patch.toJson());

    if (resp.statusCode != 200) {
      throw Exception('Failed to update baby growth entry');
    }

    return BabyGrowthEntry.fromJson(jsonDecode(resp.body));
  }
}

class BabyGrowthTrackerScreen extends StatefulWidget {
  final int babyId;
  final String babyName;

  const BabyGrowthTrackerScreen({
    super.key,
    required this.babyId,
    required this.babyName,
  });

  @override
  State<BabyGrowthTrackerScreen> createState() =>
      _BabyGrowthTrackerScreenState();
}

class _BabyGrowthTrackerScreenState extends State<BabyGrowthTrackerScreen> {
  final _service = BabyGrowthApiService();

  final _weightCtrl = TextEditingController();
  final _heightCtrl = TextEditingController();
  final _headCtrl = TextEditingController();
  int? _touchedWeek;
  List<BabyGrowthEntry> _entries = [];
  BabyGrowthEntry? _selected;
  bool _isNewEntry = false;
  bool _isLoading = true;
  void _selectWeek(int week) {
    final entry = _entries.firstWhere(
      (e) => e.weekNumber == week,
      orElse: () => _entries.last,
    );

    setState(() {
      _selected = entry;
      _isNewEntry = false;
      _touchedWeek = week;
    });

    _fillForm();
  }

  void _cancelEdit() {
    final last = _entries.isNotEmpty ? _entries.last : null;

    setState(() {
      _selected = last;
      _isNewEntry = false;
      _touchedWeek = null;
    });

    _fillForm();
  }

  @override
  void initState() {
    super.initState();
    _loadData();
  }

  @override
  void dispose() {
    _weightCtrl.dispose();
    _heightCtrl.dispose();
    _headCtrl.dispose();
    super.dispose();
  }

  Future<void> _loadData() async {
    try {
      final list = await _service.getForBaby(babyId: widget.babyId);
      setState(() {
        _entries = list;
        _selected = list.isNotEmpty ? list.last : null;
        _isLoading = false;
      });
      _fillForm();
    } catch (e) {
      setState(() => _isLoading = false);
      NestlyToast.error(context, 'Greška pri učitavanju podataka');
    }
  }

  int get _maxWeek => _entries.isEmpty ? 1 : _entries.last.weekNumber;

  bool get _canEdit => _selected != null;
  void _fillForm() {
    if (_selected == null) return;
    _weightCtrl.text = _selected!.weightKg?.toStringAsFixed(1) ?? '';
    _heightCtrl.text = _selected!.heightCm?.toStringAsFixed(1) ?? '';
    _headCtrl.text = _selected!.headCircumferenceCm?.toStringAsFixed(1) ?? '';
  }

  void _startNewWeek() {
    setState(() {
      _selected = BabyGrowthEntry(
        id: 0,
        babyId: widget.babyId,
        weekNumber: _maxWeek + 1,
      );
      _isNewEntry = true;
    });
    _weightCtrl.clear();
    _heightCtrl.clear();
    _headCtrl.clear();
  }

  Future<void> _save() async {
    if (_selected == null) return;

    final weight = double.tryParse(_weightCtrl.text);
    final height = double.tryParse(_heightCtrl.text);
    final head = double.tryParse(_headCtrl.text);

    if (weight == null || height == null || head == null) {
      NestlyToast.warning(
        context,
        'Unesite sve vrijednosti i koristite samo brojeve.',
        accentColor: AppColors.seed,
      );
      return;
    }

    setState(() => _isLoading = true);

    try {
      if (_isNewEntry) {
        final created = await _service.create(
          request: CreateBabyGrowthRequest(
            babyId: widget.babyId,
            weekNumber: _selected!.weekNumber,
            weightKg: weight,
            heightCm: height,
            headCircumferenceCm: head,
          ),
        );
        _entries.add(created);
        _selected = created;
        _isNewEntry = false;
      } else {
        final updated = await _service.patch(
          id: _selected!.id,
          patch: BabyGrowthPatchRequest(
            weightKg: weight,
            heightCm: height,
            headCircumferenceCm: head,
          ),
        );
        final idx = _entries.indexWhere((e) => e.id == updated.id);
        if (idx != -1) _entries[idx] = updated;
        _selected = updated;
      }
      BabyGrowthApiService._cache.remove(widget.babyId);

      NestlyToast.success(
        context,
        'Podaci su sačuvani',
        accentColor: AppColors.seed,
      );
    } catch (_) {
      NestlyToast.error(context, 'Greška pri spremanju');
    }

    setState(() => _isLoading = false);
  }

  @override
  Widget build(BuildContext context) {
    final selectedWeek = _selected?.weekNumber ?? 1;

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
          'Praćenje rasta',
          style: Theme.of(context).textTheme.titleLarge?.copyWith(
            fontWeight: FontWeight.w700,
            color: AppColors.seed,
          ),
        ),
      ),
      body: _isLoading
          ? const Center(child: CircularProgressIndicator())
          : SingleChildScrollView(
              padding: const EdgeInsets.all(AppSpacing.lg),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.stretch,
                children: [
                  _buildChartCard(),
                  const SizedBox(height: AppSpacing.lg),
                  Row(
                    children: [
                      Text(
                        _isNewEntry
                            ? 'Nova sedmica $selectedWeek'
                            : 'Sedmica $selectedWeek',
                        style: Theme.of(context).textTheme.titleLarge?.copyWith(
                          fontWeight: FontWeight.w700,
                          color: AppColors.seed,
                        ),
                      ),
                      const Spacer(),
                      TextButton.icon(
                        onPressed: _startNewWeek,
                        icon: const Icon(Icons.add_circle_outline),
                        label: const Text('Nova sedmica'),
                        style: TextButton.styleFrom(
                          foregroundColor: AppColors.seed,
                        ),
                      ),
                    ],
                  ),
                  const SizedBox(height: AppSpacing.md),
                  _buildFormCard(),
                ],
              ),
            ),
    );
  }

  Widget _buildChartCard() {
    return Card(
      shape: RoundedRectangleBorder(
        borderRadius: BorderRadius.circular(AppRadius.xl),
      ),
      child: Padding(
        padding: const EdgeInsets.all(AppSpacing.lg),
        child: Column(
          children: [
            SizedBox(
              height: 240,
              child: _entries.isEmpty
                  ? const Center(
                      child: Text(
                        'Još nema podataka.\nDodajte prvi unos.',
                        textAlign: TextAlign.center,
                      ),
                    )
                  : LineChart(
                      LineChartData(
                        minX: 1,
                        maxX: _maxWeek.toDouble(),
                        lineTouchData: LineTouchData(
                          touchCallback: (event, response) {
                            if (response == null ||
                                response.lineBarSpots == null)
                              return;

                            final spot = response.lineBarSpots!.first;
                            final week = spot.x.toInt();
                            _selectWeek(week);
                          },
                        ),
                        lineBarsData: [
                          LineChartBarData(
                            spots: _entries
                                .where((e) => e.weightKg != null)
                                .map(
                                  (e) => FlSpot(
                                    e.weekNumber.toDouble(),
                                    e.weightKg!,
                                  ),
                                )
                                .toList(),
                            color: AppColors.babyBlue,
                            isCurved: true,
                            barWidth: 3,
                            dotData: FlDotData(
                              show: true,
                              getDotPainter: (spot, percent, barData, index) {
                                final isSelected =
                                    spot.x.toInt() == _touchedWeek;
                                return FlDotCirclePainter(
                                  radius: isSelected ? 6 : 3,
                                  color: isSelected
                                      ? Colors.red
                                      : barData.color!,
                                  strokeWidth: isSelected ? 2 : 0,
                                  strokeColor: Colors.white,
                                );
                              },
                            ),
                          ),

                          LineChartBarData(
                            spots: _entries
                                .where((e) => e.heightCm != null)
                                .map(
                                  (e) => FlSpot(
                                    e.weekNumber.toDouble(),
                                    e.heightCm!,
                                  ),
                                )
                                .toList(),
                            color: AppColors.seed,
                            isCurved: true,
                            barWidth: 3,
                            dotData: FlDotData(
                              show: true,
                              getDotPainter: (spot, percent, barData, index) {
                                final isSelected =
                                    spot.x.toInt() == _touchedWeek;
                                return FlDotCirclePainter(
                                  radius: isSelected ? 6 : 3,
                                  color: isSelected
                                      ? Colors.red
                                      : barData.color!,
                                  strokeWidth: isSelected ? 2 : 0,
                                  strokeColor: Colors.white,
                                );
                              },
                            ),
                          ),

                          LineChartBarData(
                            spots: _entries
                                .where((e) => e.headCircumferenceCm != null)
                                .map(
                                  (e) => FlSpot(
                                    e.weekNumber.toDouble(),
                                    e.headCircumferenceCm!,
                                  ),
                                )
                                .toList(),
                            color: AppColors.babyPink,
                            isCurved: true,
                            barWidth: 3,
                            dotData: FlDotData(
                              show: true,
                              getDotPainter: (spot, percent, barData, index) {
                                final isSelected =
                                    spot.x.toInt() == _touchedWeek;
                                return FlDotCirclePainter(
                                  radius: isSelected ? 6 : 3,
                                  color: isSelected
                                      ? Colors.red
                                      : barData.color!,
                                  strokeWidth: isSelected ? 2 : 0,
                                  strokeColor: Colors.white,
                                );
                              },
                            ),
                          ),
                        ],
                      ),
                    ),
            ),
            const SizedBox(height: 12),
            _buildLegend(),
          ],
        ),
      ),
    );
  }

  Widget _buildLegend() {
    Widget item(Color color, String text) {
      return Row(
        mainAxisSize: MainAxisSize.min,
        children: [
          Container(
            width: 14,
            height: 14,
            decoration: BoxDecoration(
              color: color,
              borderRadius: BorderRadius.circular(4),
            ),
          ),
          const SizedBox(width: 6),
          Text(text, style: const TextStyle(fontWeight: FontWeight.w600)),
        ],
      );
    }

    return Wrap(
      spacing: 20,
      children: [
        item(AppColors.babyBlue, 'Težina (kg)'),
        item(AppColors.seed, 'Dužina (cm)'),
        item(AppColors.babyPink, 'Obim glave (cm)'),
      ],
    );
  }

  Widget _buildFormCard() {
    InputDecoration deco(String label, String suffix) => InputDecoration(
      labelText: label,
      suffixText: suffix,
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
            TextField(
              controller: _weightCtrl,
              enabled: _canEdit,
              keyboardType: const TextInputType.numberWithOptions(
                decimal: true,
              ),
              inputFormatters: [
                FilteringTextInputFormatter.allow(RegExp(r'^\d+\.?\d{0,2}')),
              ],
              decoration: deco('Težina', 'kg'),
            ),
            const SizedBox(height: 12),
            TextField(
              controller: _heightCtrl,
              enabled: _canEdit,
              keyboardType: const TextInputType.numberWithOptions(
                decimal: true,
              ),
              inputFormatters: [
                FilteringTextInputFormatter.allow(RegExp(r'^\d+\.?\d{0,2}')),
              ],
              decoration: deco('Dužina', 'cm'),
            ),
            const SizedBox(height: 12),
            TextField(
              controller: _headCtrl,
              enabled: _canEdit,
              keyboardType: const TextInputType.numberWithOptions(
                decimal: true,
              ),
              inputFormatters: [
                FilteringTextInputFormatter.allow(RegExp(r'^\d+\.?\d{0,2}')),
              ],
              decoration: deco('Obim glave', 'cm'),
            ),
            const SizedBox(height: 16),
            SizedBox(
              width: double.infinity,
              child: ElevatedButton(
                onPressed: _canEdit ? _save : null,
                style: ElevatedButton.styleFrom(
                  backgroundColor: AppColors.seed,
                  foregroundColor: Colors.white,
                  disabledBackgroundColor: AppColors.seed.withOpacity(.5),
                  disabledForegroundColor: Colors.white,
                  padding: const EdgeInsets.symmetric(vertical: 14),
                  shape: RoundedRectangleBorder(
                    borderRadius: BorderRadius.circular(AppRadius.lg),
                  ),
                ),
                child: const Text(
                  'Spremi',
                  style: TextStyle(fontWeight: FontWeight.w700),
                ),
              ),
            ),
            if (!_isNewEntry &&
                _selected != null &&
                _selected!.id != _entries.last.id) ...[
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
