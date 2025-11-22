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

String snippet(String text, {int max = 200}) {
  if (text.isEmpty) return '—';
  if (text.length <= max) return text;
  return text.substring(0, max).trimRight() + '...';
}

class FetalWeekDto {
  final String babyDevelopment;
  final String motherChanges;
  final String? imageUrl;
  final int weekNumber;

  FetalWeekDto({
    required this.babyDevelopment,
    required this.motherChanges,
    required this.weekNumber,
    this.imageUrl,
  });

  factory FetalWeekDto.fromJson(Map<String, dynamic> json) {
    String _s(dynamic v) => (v ?? '').toString();

    return FetalWeekDto(
      babyDevelopment: _s(json['babyDevelopment'] ?? json['BabyDevelopment']),
      motherChanges: _s(json['motherChanges'] ?? json['MotherChanges']),
      imageUrl: (json['imageUrl'] ?? json['ImageUrl'])?.toString(),
      weekNumber: (json['weekNumber'] ?? json['WeekNumber'] ?? 0) as int,
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

class AdviceCenterScreen extends StatefulWidget {
  const AdviceCenterScreen({super.key, required this.gestationalWeek});
  final int gestationalWeek;
  @override
  State<AdviceCenterScreen> createState() => _AdviceCenterScreenState();
}

class _AdviceCenterScreenState extends State<AdviceCenterScreen> {
  final FetalApi _fetalApi = FetalApi();

  late final List<int> _weeks;
  late int _currentWeek;

  late Future<FetalWeekDto> _featuredFuture;

  @override
  void initState() {
    super.initState();
    _weeks = List<int>.generate(40, (i) => i + 1);
    _currentWeek = widget.gestationalWeek.clamp(1, 40);

    _featuredFuture = _fetalApi.getByWeek(_currentWeek);
  }

  Future<void> _refresh() async {
    setState(() {
      _currentWeek = widget.gestationalWeek.clamp(1, 40);
      _featuredFuture = _fetalApi.getByWeek(_currentWeek);
    });
  }

  void _openDetail(int week) {
    Navigator.of(
      context,
    ).push(MaterialPageRoute(builder: (_) => AdviceDetailScreen(week: week)));
  }

  @override
  Widget build(BuildContext context) {
    final highlight = _currentWeek;

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
          'Savjetni centar',
          style: Theme.of(context).textTheme.titleLarge?.copyWith(
            fontWeight: FontWeight.w900,
            color: AppColors.roseDark,
          ),
        ),
        centerTitle: true,
      ),
      body: RefreshIndicator(
        color: AppColors.roseDark,
        onRefresh: _refresh,
        child: SingleChildScrollView(
          physics: const AlwaysScrollableScrollPhysics(),
          padding: const EdgeInsets.all(AppSpacing.xl),
          child: Center(
            child: ConstrainedBox(
              constraints: const BoxConstraints(maxWidth: 680),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.stretch,
                children: [
                  FutureBuilder<FetalWeekDto>(
                    future: _featuredFuture,
                    builder: (context, snap) {
                      if (snap.connectionState == ConnectionState.waiting) {
                        return const Padding(
                          padding: EdgeInsets.symmetric(vertical: 32),
                          child: Center(
                            child: CircularProgressIndicator(
                              color: AppColors.roseDark,
                            ),
                          ),
                        );
                      }
                      if (snap.hasError || !snap.hasData) {
                        return _FeaturedAdviceCard(
                          week: highlight,
                          summary:
                              'Nije moguće učitati podatke: '
                              '${snap.error ?? ''}',
                          onTap: () => _openDetail(highlight),
                        );
                      }

                      final dto = snap.data!;
                      final summary = snippet(
                        dto.babyDevelopment.isNotEmpty
                            ? dto.babyDevelopment
                            : dto.motherChanges,
                        max: 220,
                      );

                      return _FeaturedAdviceCard(
                        week: dto.weekNumber,
                        summary: summary,
                        onTap: () => _openDetail(dto.weekNumber),
                      );
                    },
                  ),

                  const SizedBox(height: AppSpacing.xl),

                  Card(
                    elevation: 3,
                    color: AppColors.card,
                    shape: RoundedRectangleBorder(
                      borderRadius: BorderRadius.circular(AppRadius.xl),
                    ),
                    child: Padding(
                      padding: const EdgeInsets.symmetric(
                        horizontal: AppSpacing.lg,
                        vertical: AppSpacing.lg,
                      ),
                      child: Column(
                        children: [
                          for (int i = 0; i < _weeks.length; i++) ...[
                            WeekTile(
                              week: _weeks[i],
                              isCurrent: _weeks[i] == highlight,
                              onTap: () => _openDetail(_weeks[i]),
                            ),
                            if (i != _weeks.length - 1)
                              Divider(
                                height: 22,
                                color: Colors.black.withOpacity(0.06),
                              ),
                          ],
                        ],
                      ),
                    ),
                  ),
                ],
              ),
            ),
          ),
        ),
      ),
    );
  }
}

class WeekTile extends StatelessWidget {
  const WeekTile({
    super.key,
    required this.week,
    required this.onTap,
    this.isCurrent = false,
  });

  final int week;
  final bool isCurrent;
  final VoidCallback onTap;

  @override
  Widget build(BuildContext context) {
    final borderColor = isCurrent
        ? AppColors.roseDark
        : AppColors.babyBlue.withOpacity(.30);

    final badge = isCurrent
        ? Container(
            padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 3),
            decoration: BoxDecoration(
              color: AppColors.roseDark.withOpacity(.08),
              borderRadius: BorderRadius.circular(99),
              border: Border.all(color: AppColors.roseDark, width: 1.2),
            ),
            child: Text(
              'Trenutna',
              style: Theme.of(context).textTheme.labelSmall?.copyWith(
                fontWeight: FontWeight.w800,
                color: AppColors.roseDark,
                letterSpacing: .2,
              ),
            ),
          )
        : null;

    return InkWell(
      borderRadius: BorderRadius.circular(AppRadius.lg),
      onTap: onTap,
      child: Ink(
        padding: const EdgeInsets.symmetric(vertical: 14, horizontal: 10),
        decoration: BoxDecoration(
          color: Colors.white,
          borderRadius: BorderRadius.circular(AppRadius.lg),
          border: Border.all(color: borderColor, width: 1.1),
        ),
        child: Row(
          children: [
            Expanded(
              child: Text(
                'Sedmica $week',
                style: Theme.of(context).textTheme.titleMedium?.copyWith(
                  fontWeight: FontWeight.w900,
                  color: AppColors.roseDark,
                ),
              ),
            ),
            if (badge != null) ...[badge, const SizedBox(width: 8)],
            const Icon(Icons.chevron_right_rounded, color: AppColors.roseDark),
          ],
        ),
      ),
    );
  }
}

class _FeaturedAdviceCard extends StatelessWidget {
  const _FeaturedAdviceCard({
    required this.week,
    required this.summary,
    required this.onTap,
  });

  final int week;
  final String summary;
  final VoidCallback onTap;

  @override
  Widget build(BuildContext context) {
    return Card(
      elevation: 5,
      shape: RoundedRectangleBorder(
        borderRadius: BorderRadius.circular(AppRadius.xl),
      ),
      child: InkWell(
        borderRadius: BorderRadius.circular(AppRadius.xl),
        onTap: onTap,
        child: Ink(
          decoration: const BoxDecoration(
            gradient: LinearGradient(
              colors: [AppColors.babyPink, Colors.white],
              begin: Alignment.topLeft,
              end: Alignment.bottomRight,
            ),
            borderRadius: BorderRadius.all(Radius.circular(AppRadius.xl)),
          ),
          padding: const EdgeInsets.all(AppSpacing.xl),
          child: Row(
            crossAxisAlignment: CrossAxisAlignment.center,
            children: [
              Expanded(
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Text(
                      'Savjeti za sedmicu $week',
                      style: Theme.of(context).textTheme.titleLarge?.copyWith(
                        fontWeight: FontWeight.w900,
                        color: AppColors.roseDark,
                      ),
                    ),
                    const SizedBox(height: 10),
                    Text(
                      summary,
                      style: Theme.of(context).textTheme.bodyMedium?.copyWith(
                        color: AppColors.roseDark,
                        height: 1.5,
                      ),
                    ),
                  ],
                ),
              ),
              const SizedBox(width: AppSpacing.lg),
              Container(
                width: 96,
                height: 96,
                decoration: BoxDecoration(
                  color: Colors.white,
                  borderRadius: BorderRadius.circular(22),
                  boxShadow: [
                    BoxShadow(
                      color: Colors.black.withOpacity(.08),
                      blurRadius: 12,
                      offset: const Offset(0, 4),
                    ),
                  ],
                ),
                child: const Icon(
                  Icons.pregnant_woman_rounded,
                  size: 56,
                  color: AppColors.roseDark,
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }
}

class AdviceDetailScreen extends StatelessWidget {
  const AdviceDetailScreen({super.key, required this.week});

  final int week;

  @override
  Widget build(BuildContext context) {
    final api = FetalApi();

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
          'Sedmica $week',
          style: Theme.of(context).textTheme.titleLarge?.copyWith(
            fontWeight: FontWeight.w900,
            color: AppColors.roseDark,
          ),
        ),
        centerTitle: true,
      ),
      body: FutureBuilder<FetalWeekDto>(
        future: api.getByWeek(week),
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
                  crossAxisAlignment: CrossAxisAlignment.stretch,
                  children: [
                    _SectionCardSingle(
                      title: 'Razvoj bebe',
                      text: dto.babyDevelopment,
                      icon: Icons.child_care_rounded,
                    ),
                    const SizedBox(height: AppSpacing.lg),
                    _SectionCardSingle(
                      title: 'Promjene kod majke',
                      text: dto.motherChanges,
                      icon: Icons.pregnant_woman_rounded,
                    ),
                  ],
                ),
              ),
            ),
          );
        },
      ),
    );
  }
}

class _SectionCardSingle extends StatelessWidget {
  const _SectionCardSingle({
    required this.title,
    required this.text,
    required this.icon,
  });

  final String title;
  final String text;
  final IconData icon;

  @override
  Widget build(BuildContext context) {
    return Card(
      color: Colors.white,
      elevation: 3,
      shape: RoundedRectangleBorder(
        borderRadius: BorderRadius.circular(AppRadius.xl),
      ),
      child: Padding(
        padding: const EdgeInsets.all(AppSpacing.xl + 4),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Row(
              children: [
                Icon(icon, size: 24, color: AppColors.roseDark),
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
              text,
              textAlign: TextAlign.left,
              style: Theme.of(context).textTheme.bodyLarge?.copyWith(
                color: AppColors.textPrimary,
                height: 1.6,
              ),
            ),
          ],
        ),
      ),
    );
  }
}
