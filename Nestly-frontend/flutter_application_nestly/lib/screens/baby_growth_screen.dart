import 'dart:convert';
import 'package:flutter/foundation.dart' show compute, kIsWeb;
import 'package:flutter/material.dart';
import 'package:flutter_application_nestly/network/api_client.dart';
import 'package:flutter_application_nestly/main.dart';

class FetalWeekDto {
  final int weekNumber;
  final String babyDevelopment;
  final String motherChanges;
  final String? imageUrl;

  const FetalWeekDto({
    required this.weekNumber,
    required this.babyDevelopment,
    required this.motherChanges,
    this.imageUrl,
  });

  factory FetalWeekDto.fromJson(Map<String, dynamic> json) {
    String s(dynamic v) => (v ?? '').toString();
    return FetalWeekDto(
      weekNumber: (json['weekNumber'] ?? json['WeekNumber'] ?? 0) as int,
      babyDevelopment: s(json['babyDevelopment'] ?? json['BabyDevelopment']),
      motherChanges: s(json['motherChanges'] ?? json['MotherChanges']),
      imageUrl: (json['imageUrl'] ?? json['ImageUrl'])?.toString(),
    );
  }
}

FetalWeekDto _parseWeek(String body) {
  final map = json.decode(body) as Map<String, dynamic>;
  return FetalWeekDto.fromJson(map);
}

class FetalApi {
  static const int _maxWeeks = 40;

  static final Map<int, Future<FetalWeekDto>> _cache = {};

  Future<FetalWeekDto> getByWeek(int week) {
    final w = week.clamp(1, _maxWeeks);

    final cached = _cache[w];
    if (cached != null) return cached;

    final future = _fetch(w);
    _cache[w] = future;
    future.catchError((e) {
      _cache.remove(w);
      throw e;
    });

    return future;
  }

  Future<FetalWeekDto> _fetch(int week) async {
    final res = await ApiClient.get(
      '/api/FetalDevelopmentWeek/week/$week',
    ).timeout(const Duration(seconds: 10));

    if (res.statusCode != 200) {
      final error = jsonDecode(res.body);
      throw Exception(
        error["message"] ?? "Greška pri učitavanju podataka za sedmicu",
      );
    }

    if (kIsWeb) {
      return _parseWeek(res.body);
    }

    return compute(_parseWeek, res.body);
  }

  void prefetchAround(int week) {
    final w = week.clamp(1, _maxWeeks);

    if (w > 1 && !_cache.containsKey(w - 1)) {
      getByWeek(w - 1);
    }
    if (w < _maxWeeks && !_cache.containsKey(w + 1)) {
      getByWeek(w + 1);
    }
  }
}

class BabyGrowthScreen extends StatefulWidget {
  const BabyGrowthScreen({super.key, required this.week});

  final int week;

  @override
  State<BabyGrowthScreen> createState() => _BabyGrowthScreenState();
}

class _BabyGrowthScreenState extends State<BabyGrowthScreen>
    with AutomaticKeepAliveClientMixin {
  static const int _maxWeeks = 40;

  final FetalApi _api = FetalApi();

  late int _currentWeek;
  late Future<FetalWeekDto> _futureWeek;

  @override
  bool get wantKeepAlive => true;

  @override
  void initState() {
    super.initState();

    _currentWeek = widget.week.clamp(1, _maxWeeks);
    _futureWeek = _api.getByWeek(_currentWeek);

    _api.prefetchAround(_currentWeek);
  }

  void _loadWeek(int week) {
    if (!mounted) return;

    final w = week.clamp(1, _maxWeeks);

    setState(() {
      _currentWeek = w;
      _futureWeek = _api.getByWeek(w);
    });

    _api.prefetchAround(w);
  }

  void _goPrev() {
    if (_currentWeek <= 1) return;
    _loadWeek(_currentWeek - 1);
  }

  void _goNext() {
    if (_currentWeek >= _maxWeeks) return;
    _loadWeek(_currentWeek + 1);
  }

  void _goToday() {
    _loadWeek(widget.week.clamp(1, _maxWeeks));
  }

  @override
  Widget build(BuildContext context) {
    super.build(context);

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
          'Sedmica $_currentWeek',
          style: Theme.of(context).textTheme.titleLarge?.copyWith(
            fontWeight: FontWeight.w700,
            color: AppColors.roseDark,
          ),
        ),
        centerTitle: true,
        actions: [
          IconButton(
            tooltip: 'Idi na trenutnu sedmicu',
            icon: const Icon(
              Icons.restart_alt_rounded,
              color: AppColors.roseDark,
            ),
            onPressed: _goToday,
          ),
        ],
      ),
      body: GestureDetector(
        onHorizontalDragEnd: (details) {
          final v = details.primaryVelocity;
          if (v == null) return;
          if (v < -300) _goNext();
          if (v > 300) _goPrev();
        },
        child: FutureBuilder<FetalWeekDto>(
          future: _futureWeek,
          builder: (context, snap) {
            if (snap.connectionState == ConnectionState.waiting) {
              return const Center(
                child: CircularProgressIndicator(color: AppColors.roseDark),
              );
            }

            if (snap.hasError || !snap.hasData) {
              return Center(
                child: Padding(
                  padding: const EdgeInsets.all(AppSpacing.xl),
                  child: Text(
                    'Nije moguće učitati podatke.',
                    style: Theme.of(
                      context,
                    ).textTheme.bodyMedium?.copyWith(color: Colors.red[700]),
                  ),
                ),
              );
            }

            final dto = snap.data!;

            return SingleChildScrollView(
              padding: const EdgeInsets.all(AppSpacing.xl),
              child: Center(
                child: ConstrainedBox(
                  constraints: const BoxConstraints(maxWidth: 680),
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.center,
                    children: [
                      RepaintBoundary(
                        child: Row(
                          mainAxisAlignment: MainAxisAlignment.center,
                          children: [
                            IconButton(
                              onPressed: _currentWeek > 1 ? _goPrev : null,
                              icon: const Icon(Icons.chevron_left_rounded),
                              color: AppColors.roseDark,
                              iconSize: 34,
                            ),
                            Container(
                              width: 200,
                              height: 200,
                              decoration: BoxDecoration(
                                color: AppColors.babyBlue.withOpacity(0.15),
                                shape: BoxShape.circle,
                              ),
                              child: Center(
                                child: ClipOval(
                                  child:
                                      (dto.imageUrl != null &&
                                          dto.imageUrl!.isNotEmpty)
                                      ? Image.network(
                                          dto.imageUrl!,
                                          width: 140,
                                          height: 140,
                                          fit: BoxFit.cover,
                                          loadingBuilder:
                                              (context, child, progress) {
                                                if (progress == null) {
                                                  return child;
                                                }
                                                final expected =
                                                    progress.expectedTotalBytes;
                                                final loaded = progress
                                                    .cumulativeBytesLoaded;
                                                return SizedBox(
                                                  width: 32,
                                                  height: 32,
                                                  child:
                                                      CircularProgressIndicator(
                                                        strokeWidth: 3,
                                                        color:
                                                            AppColors.roseDark,
                                                        value: expected != null
                                                            ? loaded / expected
                                                            : null,
                                                      ),
                                                );
                                              },
                                          errorBuilder: (_, __, ___) =>
                                              const Icon(
                                                Icons.child_care_rounded,
                                                size: 80,
                                                color: AppColors.roseDark,
                                              ),
                                        )
                                      : const Icon(
                                          Icons.child_care_rounded,
                                          size: 80,
                                          color: AppColors.roseDark,
                                        ),
                                ),
                              ),
                            ),
                            IconButton(
                              onPressed: _currentWeek < _maxWeeks
                                  ? _goNext
                                  : null,
                              icon: const Icon(Icons.chevron_right_rounded),
                              color: AppColors.roseDark,
                              iconSize: 34,
                            ),
                          ],
                        ),
                      ),
                      const SizedBox(height: AppSpacing.xl),
                      _InfoCard(
                        title: 'Razvoj bebe',
                        icon: Icons.child_care_rounded,
                        description: dto.babyDevelopment.isNotEmpty
                            ? dto.babyDevelopment
                            : '—',
                      ),
                      const SizedBox(height: AppSpacing.lg),
                      _InfoCard(
                        title: 'Promjene kod majke',
                        icon: Icons.pregnant_woman_rounded,
                        description: dto.motherChanges.isNotEmpty
                            ? dto.motherChanges
                            : '—',
                      ),
                    ],
                  ),
                ),
              ),
            );
          },
        ),
      ),
    );
  }
}

class _InfoCard extends StatelessWidget {
  const _InfoCard({
    required this.title,
    required this.description,
    required this.icon,
  });

  final String title;
  final String description;
  final IconData icon;

  @override
  Widget build(BuildContext context) {
    return Card(
      color: Colors.white,
      elevation: 3,
      shape: RoundedRectangleBorder(
        borderRadius: BorderRadius.circular(AppRadius.lg),
      ),
      child: Padding(
        padding: const EdgeInsets.all(AppSpacing.xl),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Row(
              children: [
                Icon(icon, color: AppColors.roseDark),
                const SizedBox(width: AppSpacing.sm),
                Text(
                  title,
                  style: Theme.of(context).textTheme.titleLarge?.copyWith(
                    fontWeight: FontWeight.w700,
                    color: AppColors.roseDark,
                  ),
                ),
              ],
            ),
            const SizedBox(height: AppSpacing.md),
            Text(
              description,
              style: Theme.of(context).textTheme.bodyLarge?.copyWith(
                color: AppColors.textSecondary,
                height: 1.6,
              ),
            ),
          ],
        ),
      ),
    );
  }
}
