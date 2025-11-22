import 'dart:convert';
import 'dart:io' show Platform;

import 'package:flutter/foundation.dart' show kIsWeb;
import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;

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

String get kFetalWeekBase => '$kApiBase/api/FetalDevelopmentWeek';

class FetalWeekDto {
  final int weekNumber;
  final String babyDevelopment;
  final String motherChanges;
  final String? imageUrl;

  FetalWeekDto({
    required this.weekNumber,
    required this.babyDevelopment,
    required this.motherChanges,
    this.imageUrl,
  });

  factory FetalWeekDto.fromJson(Map<String, dynamic> json) {
    String _s(dynamic v) => (v ?? '').toString();
    return FetalWeekDto(
      weekNumber: (json['weekNumber'] ?? json['WeekNumber'] ?? 0) as int,
      babyDevelopment: _s(json['babyDevelopment'] ?? json['BabyDevelopment']),
      motherChanges: _s(json['motherChanges'] ?? json['MotherChanges']),
      imageUrl: (json['imageUrl'] ?? json['ImageUrl'])?.toString(),
    );
  }
}

class FetalApi {
  Future<FetalWeekDto> getByWeek(int week, {String? token}) async {
    final url = '$kFetalWeekBase/week/$week';
    final res = await http
        .get(Uri.parse(url), headers: defaultHeaders(token: token))
        .timeout(const Duration(seconds: 10));

    if (res.statusCode == 404) {
      throw Exception('Nema podataka za sedmicu $week.');
    }
    if (res.statusCode != 200) {
      throw Exception(
        'Greška (${res.statusCode}) prilikom dohvaćanja podataka.',
      );
    }

    final map = json.decode(res.body) as Map<String, dynamic>;
    return FetalWeekDto.fromJson(map);
  }
}

class BabyGrowthScreen extends StatefulWidget {
  const BabyGrowthScreen({super.key, required this.week});

  final int week;
  @override
  State<BabyGrowthScreen> createState() => _BabyGrowthScreenState();
}

class _BabyGrowthScreenState extends State<BabyGrowthScreen> {
  final FetalApi _fetalApi = FetalApi();

  late int _currentWeek;

  late Future<FetalWeekDto> _futureWeek;

  @override
  void initState() {
    super.initState();

    _currentWeek = widget.week.clamp(1, 40);

    _futureWeek = _loadWeek();
  }

  Future<FetalWeekDto> _loadWeek() {
    return _fetalApi.getByWeek(_currentWeek);
  }

  void _goPrev() {
    if (_currentWeek <= 1) return;
    setState(() {
      _currentWeek -= 1;
      _futureWeek = _loadWeek();
    });
  }

  void _goNext() {
    if (_currentWeek >= 40) return;
    setState(() {
      _currentWeek += 1;
      _futureWeek = _loadWeek();
    });
  }

  void _goToday() {
    setState(() {
      _currentWeek = widget.week.clamp(1, 40);
      _futureWeek = _loadWeek();
    });
  }

  @override
  Widget build(BuildContext context) {
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
            fontWeight: FontWeight.w800,
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
          if (details.primaryVelocity == null) return;
          if (details.primaryVelocity! < 0) {
            _goNext();
          } else {
            _goPrev();
          }
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
                    'Greška: ${snap.error ?? 'Nije moguće učitati podatke.'}',
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
                      Row(
                        mainAxisAlignment: MainAxisAlignment.center,
                        crossAxisAlignment: CrossAxisAlignment.center,
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
                                            (context, child, loadingProgress) {
                                              if (loadingProgress == null) {
                                                return child;
                                              }

                                              final expected = loadingProgress
                                                  .expectedTotalBytes;
                                              final loaded = loadingProgress
                                                  .cumulativeBytesLoaded;

                                              return Center(
                                                child: SizedBox(
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
                            onPressed: _currentWeek < 40 ? _goNext : null,
                            icon: const Icon(Icons.chevron_right_rounded),
                            color: AppColors.roseDark,
                            iconSize: 34,
                          ),
                        ],
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
                    fontWeight: FontWeight.w800,
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
