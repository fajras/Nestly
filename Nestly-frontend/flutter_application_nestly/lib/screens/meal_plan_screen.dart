import 'dart:convert';
import 'dart:io' show Platform;

import 'package:flutter/foundation.dart' show kIsWeb;
import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;

import 'package:flutter_application_nestly/main.dart';
import 'package:flutter_application_nestly/layouts/nestly_toast.dart';

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

Map<String, String> _jsonHeaders() => {
  'Content-Type': 'application/json',
  'Accept': 'application/json',
};

class MealRecommendation {
  final int id;
  final int weekNumber;
  final int foodTypeId;
  final String foodName;

  MealRecommendation({
    required this.id,
    required this.weekNumber,
    required this.foodTypeId,
    required this.foodName,
  });
}

class MealPlanApiService {
  final int babyId;
  final String baseUrl;

  MealPlanApiService({required this.babyId, String? baseUrl})
    : baseUrl = baseUrl ?? _apiBase;

  String get _planBase => '$baseUrl/api/MealPlan';
  String get _recBase => '$baseUrl/api/MealPlan/Recommendation';

  Future<List<MealRecommendation>> fetchRecommendations() async {
    final uri = Uri.parse(_recBase);
    final res = await http
        .get(uri, headers: _jsonHeaders())
        .timeout(const Duration(seconds: 10));

    if (res.statusCode != 200) {
      throw Exception('Greška pri učitavanju preporuka (${res.statusCode}).');
    }

    final body = jsonDecode(res.body);
    if (body is! List) return [];

    return body.map((raw) {
      final m = raw as Map<String, dynamic>;
      return MealRecommendation(
        id: (m['id'] ?? m['Id']) as int,
        weekNumber: (m['weekNumber'] ?? m['WeekNumber']) as int,
        foodTypeId: (m['foodTypeId'] ?? m['FoodTypeId']) as int,
        foodName: (m['foodName'] ?? m['FoodName'] ?? '').toString(),
      );
    }).toList()..sort((a, b) {
      final c = a.weekNumber.compareTo(b.weekNumber);
      if (c != 0) return c;
      return a.foodName.compareTo(b.foodName);
    });
  }

  Future<Map<int, int?>> fetchRatingsForBaby() async {
    final uri = Uri.parse('$_planBase?BabyId=$babyId');

    final res = await http
        .get(uri, headers: _jsonHeaders())
        .timeout(const Duration(seconds: 10));

    if (res.statusCode != 200) {
      throw Exception('Greška pri učitavanju ocjena (${res.statusCode}).');
    }

    final body = jsonDecode(res.body);
    if (body is! List) return {};

    final map = <int, int?>{};
    for (final raw in body) {
      final m = raw as Map<String, dynamic>;
      final foodTypeId = (m['foodTypeId'] ?? m['FoodTypeId']) as int;
      final ratingDynamic = m['rating'] ?? m['Rating'];
      final rating = ratingDynamic == null
          ? null
          : int.tryParse('$ratingDynamic');
      map[foodTypeId] = rating;
    }
    return map;
  }

  Future<void> rateFood({required int foodTypeId, required int rating}) async {
    final uri = Uri.parse(_planBase);

    final body = jsonEncode({
      'babyId': babyId,
      'foodTypeId': foodTypeId,
      'rating': rating,
      'triedAt': DateTime.now().toIso8601String(),
    });

    final res = await http
        .post(uri, headers: _jsonHeaders(), body: body)
        .timeout(const Duration(seconds: 10));

    if (res.statusCode != 200 && res.statusCode != 201) {
      throw Exception('Greška pri spremanju ocjene (${res.statusCode}).');
    }
  }
}

class MealRecommendationScreen extends StatefulWidget {
  const MealRecommendationScreen({super.key, required this.babyId});

  final int babyId;

  @override
  State<MealRecommendationScreen> createState() =>
      _MealRecommendationScreenState();
}

class _MealRecommendationScreenState extends State<MealRecommendationScreen> {
  late final MealPlanApiService _service;

  bool _loading = true;
  bool _saving = false;
  String? _error;

  List<MealRecommendation> _items = [];
  Map<int, int?> _ratings = {};
  final Map<int, int> _dirtyRatings = {};

  @override
  void initState() {
    super.initState();
    _service = MealPlanApiService(babyId: widget.babyId);
    _load();
  }

  Future<void> _load() async {
    setState(() {
      _loading = true;
      _error = null;
    });

    try {
      final recs = await _service.fetchRecommendations();
      final ratings = await _service.fetchRatingsForBaby();
      if (!mounted) return;
      setState(() {
        _items = recs;
        _ratings = ratings;
        _dirtyRatings.clear();
        _loading = false;
      });
    } catch (e) {
      if (!mounted) return;
      setState(() {
        _error = e.toString();
        _loading = false;
      });
    }
  }

  int? _getRatingForFood(int foodTypeId) => _ratings[foodTypeId];

  void _onChangeRating(int foodTypeId, int newRating) {
    setState(() {
      _ratings[foodTypeId] = newRating;
      _dirtyRatings[foodTypeId] = newRating;
    });
  }

  Future<void> _saveChanges() async {
    if (_dirtyRatings.isEmpty) {
      NestlyToast.info(context, 'Nema izmjena za spremiti.');
      return;
    }

    setState(() {
      _saving = true;
    });

    try {
      for (final entry in _dirtyRatings.entries) {
        await _service.rateFood(foodTypeId: entry.key, rating: entry.value);
      }

      if (!mounted) return;
      setState(() {
        _dirtyRatings.clear();
        _saving = false;
      });

      NestlyToast.success(context, 'Ocjene su uspješno sačuvane.');
    } catch (e) {
      if (!mounted) return;
      setState(() {
        _saving = false;
      });
      NestlyToast.error(context, 'Greška pri spremanju ocjena: $e');
    }
  }

  @override
  Widget build(BuildContext context) {
    final hasChanges = _dirtyRatings.isNotEmpty;

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
          'Plan ishrane',
          style: Theme.of(context).textTheme.titleLarge?.copyWith(
            fontWeight: FontWeight.w800,
            color: AppColors.roseDark,
          ),
        ),
        centerTitle: true,
      ),
      body: RefreshIndicator(
        color: AppColors.roseDark,
        onRefresh: _load,
        child: _buildBody(),
      ),
      bottomNavigationBar: SafeArea(
        child: Padding(
          padding: const EdgeInsets.fromLTRB(
            AppSpacing.xl,
            0,
            AppSpacing.xl,
            AppSpacing.lg,
          ),
          child: SizedBox(
            height: 52,
            child: ElevatedButton(
              onPressed: !_saving && hasChanges ? _saveChanges : null,
              style: ElevatedButton.styleFrom(
                foregroundColor: AppColors.card,
                backgroundColor: AppColors.roseDark,
                disabledBackgroundColor: AppColors.roseDark.withOpacity(0.35),
                shape: RoundedRectangleBorder(
                  borderRadius: BorderRadius.circular(AppRadius.lg),
                ),
                elevation: 0,
                textStyle: const TextStyle(fontWeight: FontWeight.w700),
              ),
              child: _saving
                  ? const SizedBox(
                      height: 22,
                      width: 22,
                      child: CircularProgressIndicator(strokeWidth: 2),
                    )
                  : Text(hasChanges ? 'Sačuvaj promjene' : 'Nema izmjena'),
            ),
          ),
        ),
      ),
    );
  }

  Widget _buildBody() {
    if (_loading) {
      return ListView(
        physics: const AlwaysScrollableScrollPhysics(),
        padding: const EdgeInsets.all(AppSpacing.xl),
        children: [
          const SizedBox(height: AppSpacing.lg),
          ...List.generate(
            4,
            (_) => Container(
              height: 64,
              margin: const EdgeInsets.only(bottom: 8),
              decoration: BoxDecoration(
                color: Colors.white,
                borderRadius: BorderRadius.circular(AppRadius.lg),
              ),
            ),
          ),
        ],
      );
    }

    if (_error != null) {
      return ListView(
        physics: const AlwaysScrollableScrollPhysics(),
        padding: const EdgeInsets.all(AppSpacing.xl),
        children: [
          const SizedBox(height: AppSpacing.lg),
          Container(
            padding: const EdgeInsets.all(AppSpacing.lg),
            decoration: BoxDecoration(
              color: Colors.red.withOpacity(.06),
              borderRadius: BorderRadius.circular(AppRadius.lg),
              border: Border.all(color: Colors.red.withOpacity(.35)),
            ),
            child: Text(
              'Greška: $_error',
              style: const TextStyle(
                color: Colors.red,
                fontWeight: FontWeight.w600,
              ),
            ),
          ),
        ],
      );
    }

    if (_items.isEmpty) {
      return ListView(
        physics: const AlwaysScrollableScrollPhysics(),
        padding: const EdgeInsets.all(AppSpacing.xl),
        children: [
          const SizedBox(height: AppSpacing.lg),
          Container(
            padding: const EdgeInsets.all(AppSpacing.lg),
            decoration: BoxDecoration(
              color: AppColors.roseDark.withOpacity(.08),
              borderRadius: BorderRadius.circular(AppRadius.lg),
            ),
            child: Text(
              'Još nema definisanih preporuka za uvođenje čvrste hrane.',
              textAlign: TextAlign.center,
              style: Theme.of(
                context,
              ).textTheme.bodyMedium?.copyWith(color: AppColors.textSecondary),
            ),
          ),
        ],
      );
    }

    final items = _items;
    final children = <Widget>[];
    int? currentWeek;

    for (final rec in items) {
      if (currentWeek != rec.weekNumber) {
        currentWeek = rec.weekNumber;
        children.add(const SizedBox(height: AppSpacing.lg));
        children.add(_WeekHeader(weekNumber: rec.weekNumber));
        children.add(const SizedBox(height: AppSpacing.sm));
      }

      final rating = _getRatingForFood(rec.foodTypeId);

      children.add(
        _FoodRow(
          recommendation: rec,
          rating: rating,
          onRatingChanged: (val) => _onChangeRating(rec.foodTypeId, val),
        ),
      );
      children.add(
        Divider(height: 1, color: AppColors.babyPink.withOpacity(0.25)),
      );
    }

    return ListView(
      physics: const AlwaysScrollableScrollPhysics(),
      padding: const EdgeInsets.fromLTRB(
        AppSpacing.xl,
        AppSpacing.xl,
        AppSpacing.xl,
        AppSpacing.xl,
      ),
      children: [
        const SizedBox(height: AppSpacing.lg),
        ...children,
        const SizedBox(height: 80),
      ],
    );
  }
}

class _WeekHeader extends StatelessWidget {
  const _WeekHeader({required this.weekNumber});

  final int weekNumber;

  @override
  Widget build(BuildContext context) {
    return Container(
      padding: const EdgeInsets.symmetric(horizontal: 14, vertical: 8),
      decoration: BoxDecoration(
        color: AppColors.babyPink.withOpacity(0.18),
        borderRadius: BorderRadius.circular(999),
      ),
      child: Row(
        mainAxisSize: MainAxisSize.min,
        children: [
          const Icon(
            Icons.restaurant_rounded,
            size: 16,
            color: AppColors.roseDark,
          ),
          const SizedBox(width: 6),
          Text(
            '${weekNumber}. sedmica',
            style: Theme.of(context).textTheme.labelLarge?.copyWith(
              color: AppColors.roseDark,
              fontWeight: FontWeight.w700,
            ),
          ),
        ],
      ),
    );
  }
}

class _FoodRow extends StatelessWidget {
  const _FoodRow({
    required this.recommendation,
    required this.rating,
    required this.onRatingChanged,
  });

  final MealRecommendation recommendation;
  final int? rating;
  final ValueChanged<int> onRatingChanged;

  @override
  Widget build(BuildContext context) {
    final currentRating = rating ?? 0;

    return Padding(
      padding: const EdgeInsets.symmetric(vertical: 6),
      child: Container(
        padding: const EdgeInsets.symmetric(horizontal: 14, vertical: 10),
        decoration: BoxDecoration(
          color: Colors.white,
          borderRadius: BorderRadius.circular(AppRadius.lg),
          boxShadow: [
            BoxShadow(
              color: AppColors.roseDark.withOpacity(0.06),
              blurRadius: 6,
              offset: const Offset(0, 2),
            ),
          ],
        ),
        child: Row(
          children: [
            Container(
              height: 34,
              width: 34,
              decoration: BoxDecoration(
                color: AppColors.babyPink.withOpacity(.20),
                borderRadius: BorderRadius.circular(10),
              ),
              child: const Icon(
                Icons.local_dining_rounded,
                size: 20,
                color: AppColors.roseDark,
              ),
            ),

            const SizedBox(width: 12),

            Expanded(
              child: Text(
                recommendation.foodName,
                style: Theme.of(context).textTheme.bodyLarge?.copyWith(
                  fontWeight: FontWeight.w600,
                  fontSize: 16,
                  color: AppColors.roseDark,
                ),
              ),
            ),

            const SizedBox(width: 8),

            Row(
              mainAxisSize: MainAxisSize.min,
              children: List.generate(5, (index) {
                final value = index + 1;
                final isFilled = value <= currentRating;

                return GestureDetector(
                  onTap: () => onRatingChanged(value),
                  child: Padding(
                    padding: const EdgeInsets.symmetric(horizontal: 2),
                    child: Icon(
                      Icons.favorite_rounded,
                      size: 20,
                      color: isFilled
                          ? AppColors.roseDark
                          : AppColors.roseDark.withOpacity(0.20),
                    ),
                  ),
                );
              }),
            ),
          ],
        ),
      ),
    );
  }
}
