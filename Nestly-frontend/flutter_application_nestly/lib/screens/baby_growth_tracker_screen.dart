import 'dart:convert';
import 'dart:io' show Platform;

import 'package:fl_chart/fl_chart.dart';
import 'package:flutter/foundation.dart' show kIsWeb;
import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;

import 'package:flutter_application_nestly/main.dart';

class BabyGrowthEntry {
  final int id;
  final int babyId;
  final int weekNumber;
  final double? weightKg;
  final double? heightCm;
  final double? headCircumferenceCm;

  BabyGrowthEntry({
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

  CreateBabyGrowthRequest({
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

  BabyGrowthPatchRequest({
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

class BabyGrowthApiService {
  String get _baseUrl => '$_apiBase/api/BabyGrowth';

  Future<List<BabyGrowthEntry>> getForBaby({
    required int babyId,
    String? token,
  }) async {
    final uri = Uri.parse('$_baseUrl?BabyId=$babyId');
    final resp = await http.get(uri, headers: _headers(token));

    if (resp.statusCode != 200) {
      throw Exception('Failed to load baby growth data');
    }

    final List<dynamic> data = jsonDecode(resp.body) as List<dynamic>;
    return data
        .map((e) => BabyGrowthEntry.fromJson(e as Map<String, dynamic>))
        .toList();
  }

  Future<BabyGrowthEntry> create({
    required CreateBabyGrowthRequest request,
    String? token,
  }) async {
    final uri = Uri.parse(_baseUrl);
    final resp = await http.post(
      uri,
      headers: _headers(token),
      body: jsonEncode(request.toJson()),
    );

    if (resp.statusCode != 201 && resp.statusCode != 200) {
      throw Exception('Failed to create baby growth entry');
    }

    final Map<String, dynamic> data =
        jsonDecode(resp.body) as Map<String, dynamic>;
    return BabyGrowthEntry.fromJson(data);
  }

  Future<BabyGrowthEntry> patch({
    required int id,
    required BabyGrowthPatchRequest patch,
    String? token,
  }) async {
    final uri = Uri.parse('$_baseUrl/$id');
    final resp = await http.patch(
      uri,
      headers: _headers(token),
      body: jsonEncode(patch.toJson()),
    );

    if (resp.statusCode != 200) {
      throw Exception('Failed to update baby growth entry');
    }

    final Map<String, dynamic> data =
        jsonDecode(resp.body) as Map<String, dynamic>;
    return BabyGrowthEntry.fromJson(data);
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

  List<BabyGrowthEntry> _entries = [];
  BabyGrowthEntry? _selected;
  bool _isNewEntry = false;
  bool _isLoading = true;

  final _weightCtrl = TextEditingController();
  final _heightCtrl = TextEditingController();
  final _headCtrl = TextEditingController();

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
      list.sort((a, b) => a.weekNumber.compareTo(b.weekNumber));
      setState(() {
        _entries = list;
        _selected = _entries.isNotEmpty ? _entries.last : null;
        _isNewEntry = false;
        _isLoading = false;
      });
      _fillFormFromSelected();
    } catch (e) {
      setState(() => _isLoading = false);
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Greška pri učitavanju podataka: $e')),
      );
    }
  }

  int get _maxWeek => _entries.isEmpty
      ? 1
      : _entries.map((e) => e.weekNumber).reduce((a, b) => a > b ? a : b);

  BabyGrowthEntry? get _latestEntry => _entries.isEmpty
      ? null
      : _entries.reduce((a, b) => a.weekNumber > b.weekNumber ? a : b);

  bool get _isEditingLatest =>
      !_isNewEntry &&
      _selected != null &&
      _latestEntry != null &&
      _selected!.id == _latestEntry!.id;

  void _startNewWeekEntry() {
    final nextWeek = _entries.isEmpty ? 1 : _maxWeek + 1;
    setState(() {
      _selected = BabyGrowthEntry(
        id: 0,
        babyId: widget.babyId,
        weekNumber: nextWeek,
        weightKg: null,
        heightCm: null,
        headCircumferenceCm: null,
      );
      _isNewEntry = true;
    });
    _weightCtrl.clear();
    _heightCtrl.clear();
    _headCtrl.clear();
  }

  void _fillFormFromSelected() {
    if (_selected == null) return;

    _weightCtrl.text = _selected!.weightKg != null
        ? _selected!.weightKg!.toStringAsFixed(1)
        : '';
    _heightCtrl.text = _selected!.heightCm != null
        ? _selected!.heightCm!.toStringAsFixed(1)
        : '';
    _headCtrl.text = _selected!.headCircumferenceCm != null
        ? _selected!.headCircumferenceCm!.toStringAsFixed(1)
        : '';
  }

  void _onWeekSelectedFromChart(int week) {
    final entry = _entries.firstWhere(
      (e) => e.weekNumber == week,
      orElse: () =>
          _selected ??
          _latestEntry ??
          (_entries.isNotEmpty ? _entries.last : _entries.first),
    );
    setState(() {
      _selected = entry;
      _isNewEntry = false;
    });
    _fillFormFromSelected();
  }

  Future<void> _onSavePressed() async {
    final weight = _weightCtrl.text.trim().isEmpty
        ? null
        : double.tryParse(_weightCtrl.text.trim());
    final height = _heightCtrl.text.trim().isEmpty
        ? null
        : double.tryParse(_heightCtrl.text.trim());
    final head = _headCtrl.text.trim().isEmpty
        ? null
        : double.tryParse(_headCtrl.text.trim());

    if (_selected == null) return;

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
        setState(() {
          _entries.add(created);
          _entries.sort((a, b) => a.weekNumber.compareTo(b.weekNumber));
          _selected = created;
          _isNewEntry = false;
        });
      } else if (_isEditingLatest) {
        final updated = await _service.patch(
          id: _selected!.id,
          patch: BabyGrowthPatchRequest(
            weightKg: weight,
            heightCm: height,
            headCircumferenceCm: head,
          ),
        );
        final idx = _entries.indexWhere((e) => e.id == updated.id);
        if (idx != -1) {
          setState(() {
            _entries[idx] = updated;
            _selected = updated;
          });
        }
      } else {
        ScaffoldMessenger.of(context).showSnackBar(
          const SnackBar(
            content: Text('Možete uređivati samo posljednji unos.'),
          ),
        );
      }
    } catch (e) {
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Greška pri spremanju podataka: $e')),
      );
    } finally {
      setState(() => _isLoading = false);
    }
  }

  @override
  Widget build(BuildContext context) {
    final selectedWeek =
        _selected?.weekNumber ?? (_entries.isEmpty ? 1 : _maxWeek);

    return Scaffold(
      backgroundColor: AppColors.bg,
      appBar: AppBar(
        backgroundColor: Colors.transparent,
        elevation: 0,
        leading: IconButton(
          icon: const Icon(Icons.arrow_back_ios_new_rounded),
          color: AppColors.seed,
          onPressed: () => Navigator.of(context).pop(),
        ),
        centerTitle: true,
        title: Text(
          "Praćenje rasta",
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
                  Row(
                    children: [
                      Text(
                        "Sedmica $selectedWeek",
                        style: Theme.of(context).textTheme.titleMedium
                            ?.copyWith(
                              fontWeight: FontWeight.w700,
                              color: AppColors.seed,
                            ),
                      ),
                      const Spacer(),
                      TextButton.icon(
                        onPressed: _startNewWeekEntry,
                        style: TextButton.styleFrom(
                          foregroundColor: AppColors.seed,
                          textStyle: const TextStyle(
                            fontWeight: FontWeight.w600,
                          ),
                        ),
                        icon: Icon(
                          Icons.add_circle_outline,
                          color: AppColors.seed,
                        ),
                        label: const Text("Nova sedmica"),
                      ),
                    ],
                  ),
                  const SizedBox(height: 8),
                  Expanded(
                    child: _buildFormCard(
                      canEdit: _isNewEntry || _isEditingLatest,
                    ),
                  ),
                ],
              ),
            ),
    );
  }

  Widget _buildChartCard() {
    if (_entries.isEmpty) {
      return Container(
        height: 600,
        decoration: BoxDecoration(
          borderRadius: BorderRadius.circular(24),
          color: Colors.white,
        ),
        child: Center(
          child: Text(
            "Još nema podataka o rastu.\nDodajte prvi unos za sedmicu 1.",
            textAlign: TextAlign.center,
            style: TextStyle(color: AppColors.textPrimary, fontSize: 13),
          ),
        ),
      );
    }

    final maxWeek = _maxWeek.toDouble();
    final weightSpots = _entries
        .where((e) => e.weightKg != null)
        .map((e) => FlSpot(e.weekNumber.toDouble(), e.weightKg!))
        .toList();
    final heightSpots = _entries
        .where((e) => e.heightCm != null)
        .map((e) => FlSpot(e.weekNumber.toDouble(), e.heightCm!))
        .toList();
    final headSpots = _entries
        .where((e) => e.headCircumferenceCm != null)
        .map((e) => FlSpot(e.weekNumber.toDouble(), e.headCircumferenceCm!))
        .toList();

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
      child: Column(
        children: [
          Row(
            children: [
              _legendDot(color: AppColors.babyBlue, label: "Težina"),
              const SizedBox(width: 12),
              _legendDot(color: AppColors.seed, label: "Dužina"),
              const SizedBox(width: 12),
              _legendDot(color: AppColors.babyPink, label: "Obim glave"),
            ],
          ),
          const SizedBox(height: 8),
          Expanded(
            child: LineChart(
              LineChartData(
                minX: 1,
                maxX: maxWeek,
                lineTouchData: LineTouchData(
                  touchCallback: (event, response) {
                    if (!event.isInterestedForInteractions ||
                        response == null ||
                        response.lineBarSpots == null ||
                        response.lineBarSpots!.isEmpty) {
                      return;
                    }
                    final spot = response.lineBarSpots!.first;
                    _onWeekSelectedFromChart(spot.x.round());
                  },
                ),
                gridData: FlGridData(show: true),
                titlesData: FlTitlesData(
                  leftTitles: AxisTitles(
                    sideTitles: SideTitles(
                      showTitles: true,
                      reservedSize: 32,
                      getTitlesWidget: (value, meta) => Text(
                        value.toInt().toString(),
                        style: TextStyle(
                          fontSize: 10,
                          color: AppColors.textSecondary,
                        ),
                      ),
                    ),
                  ),
                  bottomTitles: AxisTitles(
                    sideTitles: SideTitles(
                      showTitles: true,
                      getTitlesWidget: (value, meta) => Text(
                        value.toInt().toString(),
                        style: TextStyle(
                          fontSize: 10,
                          color: AppColors.textSecondary,
                        ),
                      ),
                    ),
                  ),
                  rightTitles: const AxisTitles(
                    sideTitles: SideTitles(showTitles: false),
                  ),
                  topTitles: const AxisTitles(
                    sideTitles: SideTitles(showTitles: false),
                  ),
                ),
                borderData: FlBorderData(show: true),
                lineBarsData: [
                  LineChartBarData(
                    spots: weightSpots,
                    color: AppColors.babyBlue,
                    isCurved: true,
                    barWidth: 3,
                    dotData: const FlDotData(show: true),
                  ),
                  LineChartBarData(
                    spots: heightSpots,
                    color: AppColors.seed,
                    isCurved: true,
                    barWidth: 3,
                    dotData: const FlDotData(show: true),
                  ),
                  LineChartBarData(
                    spots: headSpots,
                    color: AppColors.babyPink,
                    isCurved: true,
                    barWidth: 3,
                    dotData: const FlDotData(show: true),
                  ),
                ],
              ),
            ),
          ),
          const SizedBox(height: 4),
          Text(
            "sedmica",
            style: TextStyle(fontSize: 11, color: AppColors.textSecondary),
          ),
        ],
      ),
    );
  }

  Widget _legendDot({required Color color, required String label}) {
    return Row(
      mainAxisSize: MainAxisSize.min,
      children: [
        Container(
          width: 10,
          height: 10,
          decoration: BoxDecoration(color: color, shape: BoxShape.circle),
        ),
        const SizedBox(width: 4),
        Text(
          label,
          style: TextStyle(fontSize: 11, color: AppColors.textPrimary),
        ),
      ],
    );
  }

  Widget _buildFormCard({required bool canEdit}) {
    InputDecoration inputDecoration(String label, String suffix) =>
        InputDecoration(
          labelText: label,
          filled: true,
          fillColor: Colors.white,
          suffixText: suffix,
          border: OutlineInputBorder(borderRadius: BorderRadius.circular(14)),
          enabledBorder: OutlineInputBorder(
            borderRadius: BorderRadius.circular(14),
            borderSide: BorderSide(color: AppColors.babyBlue.withOpacity(0.4)),
          ),
        );

    return SingleChildScrollView(
      child: Container(
        padding: const EdgeInsets.all(12),
        decoration: BoxDecoration(
          color: AppColors.bg.withOpacity(0.9),
          borderRadius: BorderRadius.circular(22),
        ),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            const SizedBox(height: 4),
            TextField(
              controller: _weightCtrl,
              enabled: canEdit,
              keyboardType: const TextInputType.numberWithOptions(
                decimal: true,
              ),
              decoration: inputDecoration("Težina", "kg"),
            ),
            const SizedBox(height: 12),

            const SizedBox(height: 4),
            TextField(
              controller: _heightCtrl,
              enabled: canEdit,
              keyboardType: const TextInputType.numberWithOptions(
                decimal: true,
              ),
              decoration: inputDecoration("Dužina", "cm"),
            ),
            const SizedBox(height: 12),

            const SizedBox(height: 4),
            TextField(
              controller: _headCtrl,
              enabled: canEdit,
              keyboardType: const TextInputType.numberWithOptions(
                decimal: true,
              ),
              decoration: inputDecoration("Obim glave", "cm"),
            ),
            const SizedBox(height: 16),
            SizedBox(
              width: double.infinity,
              child: ElevatedButton(
                onPressed: canEdit ? _onSavePressed : null,
                style: ElevatedButton.styleFrom(
                  backgroundColor: AppColors.seed,
                  foregroundColor: Colors.white,
                  padding: const EdgeInsets.symmetric(vertical: 12),
                  shape: RoundedRectangleBorder(
                    borderRadius: BorderRadius.circular(14),
                  ),
                ),
                child: const Text(
                  "Spremi",
                  style: TextStyle(fontWeight: FontWeight.w700),
                ),
              ),
            ),
            if (!canEdit && _selected != null)
              Padding(
                padding: const EdgeInsets.only(top: 6),
                child: Text(
                  "Možete uređivati samo posljednji unos.",
                  style: TextStyle(fontSize: 12, color: Colors.red.shade400),
                ),
              ),
          ],
        ),
      ),
    );
  }
}
