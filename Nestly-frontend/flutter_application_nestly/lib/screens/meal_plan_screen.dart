import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:flutter_application_nestly/network/api_client.dart';
import 'package:flutter_application_nestly/main.dart';
import 'package:flutter_application_nestly/layouts/nestly_toast.dart';

class MealRecommendation {
  final int id;
  final int weekNumber;
  final int foodTypeId;
  final String foodName;

  const MealRecommendation({
    required this.id,
    required this.weekNumber,
    required this.foodTypeId,
    required this.foodName,
  });

  factory MealRecommendation.fromJson(Map<String, dynamic> json) {
    return MealRecommendation(
      id: int.parse((json['id'] ?? json['Id']).toString()),
      weekNumber: int.parse(
        (json['weekNumber'] ?? json['WeekNumber']).toString(),
      ),
      foodTypeId: int.parse(
        (json['foodTypeId'] ?? json['FoodTypeId']).toString(),
      ),
      foodName: (json['foodName'] ?? json['FoodName'] ?? '').toString(),
    );
  }
}

class MealPlanApiService {
  final int babyId;

  MealPlanApiService({required this.babyId});

  Future<List<MealRecommendation>> fetchRecommendations() async {
    try {
      int page = 1;
      const pageSize = 100;

      List<MealRecommendation> result = [];

      while (true) {
        final res = await ApiClient.get(
          '/api/MealPlan/Recommendation?page=$page&pageSize=$pageSize',
        );

        if (res.statusCode != 200) {
          throw Exception(
            "Failed to load meal recommendations from the server.",
          );
        }

        final data = jsonDecode(res.body);
        final List items = data['items'];

        if (items.isEmpty) break;

        final parsed = items
            .map<Map<String, dynamic>>((e) => e as Map<String, dynamic>)
            .map(MealRecommendation.fromJson)
            .toList();

        result.addAll(parsed);

        if (items.length < pageSize) break;

        page++;
      }

      result.sort((a, b) {
        final byWeek = a.weekNumber.compareTo(b.weekNumber);
        return byWeek != 0 ? byWeek : a.foodName.compareTo(b.foodName);
      });

      return result;
    } catch (_) {
      throw Exception(
        "Unable to retrieve meal recommendations. Please try again later.",
      );
    }
  }

  Future<Map<int, int?>> fetchRatingsForBaby() async {
    try {
      int page = 1;
      const pageSize = 100;

      final map = <int, int?>{};

      while (true) {
        final res = await ApiClient.get(
          '/api/MealPlan?BabyId=$babyId&page=$page&pageSize=$pageSize',
        );

        if (res.statusCode != 200) {
          throw Exception("Failed to load baby food ratings.");
        }

        final data = jsonDecode(res.body);
        final List items = data['items'];

        if (items.isEmpty) break;

        for (final raw in items.cast<Map<String, dynamic>>()) {
          final foodTypeId = int.tryParse(raw['foodTypeId'].toString());
          if (foodTypeId == null) continue;

          final rating = raw['rating'];

          map[foodTypeId] = rating == null
              ? null
              : int.tryParse(rating.toString());
        }

        if (items.length < pageSize) break;

        page++;
      }

      return map;
    } catch (_) {
      throw Exception("Unable to retrieve saved food ratings for the baby.");
    }
  }

  Future<void> rateFood({required int foodTypeId, required int rating}) async {
    try {
      final res = await ApiClient.post(
        '/api/MealPlan',
        body: {
          'babyId': babyId,
          'foodTypeId': foodTypeId,
          'rating': rating,
          'triedAt': DateTime.now().toIso8601String(),
        },
      );

      if (res.statusCode != 200 && res.statusCode != 201) {
        throw Exception("Failed to save the food rating.");
      }
    } catch (_) {
      throw Exception("An error occurred while saving the food rating.");
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

  List<MealRecommendation> _items = const [];
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
      final results = await Future.wait([
        _service.fetchRecommendations(),
        _service.fetchRatingsForBaby(),
      ]);

      if (!mounted) return;

      setState(() {
        _items = results[0] as List<MealRecommendation>;
        _ratings = results[1] as Map<int, int?>;
        _dirtyRatings.clear();
        _loading = false;
      });
    } catch (_) {
      if (!mounted) return;
      NestlyToast.error(context, "Greška pri učitavanju preporuka.");
    }
  }

  void _onRatingChanged(int foodTypeId, int rating) {
    setState(() {
      _ratings[foodTypeId] = rating;
      _dirtyRatings[foodTypeId] = rating;
    });
  }

  Future<void> _saveChanges() async {
    if (_dirtyRatings.isEmpty) {
      NestlyToast.info(context, 'Nema izmjena za spremiti.');
      return;
    }

    setState(() => _saving = true);

    try {
      await Future.wait(
        _dirtyRatings.entries.map(
          (entry) =>
              _service.rateFood(foodTypeId: entry.key, rating: entry.value),
        ),
      );

      if (!mounted) return;

      setState(() {
        _dirtyRatings.clear();
        _saving = false;
      });

      NestlyToast.success(context, 'Ocjene su uspješno sačuvane.');
    } catch (_) {
      if (!mounted) return;
      setState(() => _saving = false);
      NestlyToast.error(
        context,
        'Došlo je do greške prilikom spremanja ocjena.',
      );
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
            fontWeight: FontWeight.w700,
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
                backgroundColor: AppColors.roseDark,
                foregroundColor: Colors.white,
                disabledBackgroundColor: AppColors.roseDark.withOpacity(0.35),
                disabledForegroundColor: Colors.white.withOpacity(0.8),
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
                      child: CircularProgressIndicator(
                        strokeWidth: 2,
                        valueColor: AlwaysStoppedAnimation<Color>(Colors.white),
                      ),
                    )
                  : Text(
                      hasChanges ? 'Sačuvaj promjene' : 'Nema izmjena',
                      style: const TextStyle(),
                    ),
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
        children: List.generate(
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
      );
    }

    if (_error != null) {
      return ListView(
        physics: const AlwaysScrollableScrollPhysics(),
        padding: const EdgeInsets.all(AppSpacing.xl),
        children: [
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

    final children = <Widget>[];
    int? currentWeek;

    for (final rec in _items) {
      if (currentWeek != rec.weekNumber) {
        currentWeek = rec.weekNumber;
        children.add(const SizedBox(height: AppSpacing.lg));
        children.add(_WeekHeader(weekNumber: rec.weekNumber));
      }

      children.add(
        _FoodRow(
          recommendation: rec,
          rating: _ratings[rec.foodTypeId],
          onRatingChanged: (v) => _onRatingChanged(rec.foodTypeId, v),
        ),
      );

      children.add(
        Divider(height: 1, color: AppColors.babyPink.withOpacity(0.25)),
      );
    }

    return ListView(
      physics: const AlwaysScrollableScrollPhysics(),
      padding: const EdgeInsets.all(AppSpacing.xl),
      children: [...children, const SizedBox(height: 80)],
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
            '$weekNumber. sedmica',
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
    final current = rating ?? 0;

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
              children: List.generate(5, (i) {
                final value = i + 1;
                final filled = value <= current;

                return GestureDetector(
                  onTap: () => onRatingChanged(value),
                  child: Padding(
                    padding: const EdgeInsets.symmetric(horizontal: 2),
                    child: Icon(
                      Icons.favorite_rounded,
                      size: 20,
                      color: filled
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
