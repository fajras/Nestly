import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:flutter_application_nestly/screens/baby_profile_create_screen.dart';
import 'package:http/http.dart' as http;

import 'package:flutter_application_nestly/main.dart';
import 'package:flutter_application_nestly/screens/advice_center_screen.dart';
import 'package:flutter_application_nestly/screens/baby_growth_screen.dart';
import 'package:flutter_application_nestly/screens/blog_module_screen.dart';
import 'package:flutter_application_nestly/screens/qa_module_screen.dart';
import 'package:flutter_application_nestly/screens/symptom_diary_screen.dart';
import 'package:flutter_application_nestly/screens/therapy_module_mock.dart';
import 'package:flutter_application_nestly/screens/baby_time_home_screen.dart';

class HomeDashboardScreen extends StatefulWidget {
  const HomeDashboardScreen({
    super.key,
    required this.parentProfileId,
    required this.token,
  });

  final int parentProfileId;
  final String token;

  @override
  State<HomeDashboardScreen> createState() => _HomeDashboardScreenState();
}

class _HomeDashboardScreenState extends State<HomeDashboardScreen> {
  static const String _baseUrl = 'http://10.0.2.2:5167'; // prilagodi po potrebi

  int? _gestationalWeek;
  int? _daysRemaining;
  DateTime? _lmpDate;
  DateTime? _dueDate;

  bool _loading = true;
  bool _error = false;

  bool _checkingBaby = false;
  bool _hasBaby = false;
  int? _babyId;
  String? _babyName;

  @override
  void initState() {
    super.initState();
    _loadPregnancyStatus();
    _loadBabyStatus();
  }

  void _openBabyTime() {
    if (!_hasBaby || _babyId == null) {
      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(
          content: Text(
            'BabyTime će biti dostupan nakon što dodate podatke o bebi.',
          ),
        ),
      );
      return;
    }

    Navigator.of(context).push(
      MaterialPageRoute(
        builder: (_) => BabyTimeHomeScreen(
          babyName: _babyName ?? 'Vaša beba',
          babyId: _babyId!,
        ),
      ),
    );
  }

  Future<void> _loadBabyStatus() async {
    setState(() {
      _checkingBaby = true;
    });

    try {
      final uri = Uri.parse(
        '$_baseUrl/api/BabyProfile/latest-by-parent/${widget.parentProfileId}',
      );

      final response = await http.get(
        uri,
        headers: {
          'Content-Type': 'application/json',
          'Authorization': 'Bearer ${widget.token}',
        },
      );

      if (!mounted) return;

      if (response.statusCode == 200) {
        final data = jsonDecode(response.body) as Map<String, dynamic>;

        _hasBaby = true;
        _babyId = data['id'] as int?;
        _babyName = data['babyName'] as String?;
      } else if (response.statusCode == 404) {
        // nema bebe za ovog parenta
        _hasBaby = false;
        _babyId = null;
        _babyName = null;
      } else {
        // neki drugi error – tretiramo kao da nema bebe,
        // ali možeš i zasebno logirati
        _hasBaby = false;
      }
    } catch (e) {
      if (!mounted) return;
      _hasBaby = false;
    } finally {
      if (mounted) {
        setState(() {
          _checkingBaby = false;
        });
      }
    }
  }

  Future<void> _loadPregnancyStatus() async {
    setState(() {
      _loading = true;
      _error = false;
    });

    try {
      final uri = Uri.parse(
        '$_baseUrl/api/Pregnancy/status?parentProfileId=${widget.parentProfileId}',
      );

      final response = await http.get(
        uri,
        headers: {
          'Content-Type': 'application/json',
          'Authorization': 'Bearer ${widget.token}',
        },
      );

      if (!mounted) return;

      if (response.statusCode == 200) {
        final data = jsonDecode(response.body) as Map<String, dynamic>;

        _gestationalWeek = data['gestationalWeek'] as int?;
        _daysRemaining = data['daysRemaining'] as int?;

        final lmp = data['lmpDate'];
        final due = data['dueDate'];
        if (lmp != null) _lmpDate = DateTime.parse(lmp.toString());
        if (due != null) _dueDate = DateTime.parse(due.toString());
      } else {
        _error = true;
      }
    } catch (e) {
      if (!mounted) return;
      _error = true;
    } finally {
      if (mounted) {
        setState(() {
          _loading = false;
        });
      }
    }
  }

  int get _week {
    if (_gestationalWeek == null || _gestationalWeek! < 1) return 1;
    return _gestationalWeek!;
  }

  double get _progress {
    if (_daysRemaining == null) return 0.0;

    const totalDays = 280;
    final done = (totalDays - _daysRemaining!).clamp(0, totalDays);
    return done / totalDays;
  }

  String get _subtitle {
    if (_error) {
      return 'Nije moguće učitati podatke o trudnoći.';
    }
    if (_gestationalWeek == null || _daysRemaining == null) {
      return 'Nema podataka o trudnoći. Dodajte informacije u profilu.';
    }
    if (_daysRemaining! > 0) {
      return 'Preostalo ${_daysRemaining!} dana';
    }
    return 'Termin je prošao';
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppColors.bg,
      appBar: AppBar(
        backgroundColor: Colors.transparent,
        elevation: 0,
        centerTitle: true,
        title: Text(
          'BellyTime',
          style: Theme.of(context).textTheme.titleLarge?.copyWith(
            color: AppColors.roseDark,
            fontWeight: FontWeight.w800,
          ),
        ),
      ),
      body: GestureDetector(
        behavior: HitTestBehavior.opaque,
        onHorizontalDragEnd: (details) {
          // swipe zdesna nalijevo
          if (details.primaryVelocity != null &&
              details.primaryVelocity! < -300) {
            if (_checkingBaby) {
              ScaffoldMessenger.of(context).showSnackBar(
                const SnackBar(
                  content: Text(
                    'Provjeravam podatke o bebi, pokušajte za trenutak.',
                  ),
                ),
              );
              return;
            }

            if (!_hasBaby || _babyId == null) {
              ScaffoldMessenger.of(context).showSnackBar(
                const SnackBar(
                  content: Text(
                    'BabyTime će biti dostupan nakon što unesete podatke o bebi.\n'
                    'Koristite karticu "Beba je rođena" da dodate profil.',
                  ),
                ),
              );
              return;
            }

            _openBabyTime();
          }
        },
        child: SafeArea(
          child: Center(
            child: SingleChildScrollView(
              padding: const EdgeInsets.all(AppSpacing.xl),
              child: ConstrainedBox(
                constraints: const BoxConstraints(maxWidth: 520),
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.stretch,
                  children: [
                    _HeaderSimple(
                      title: 'Sedmica $_week',
                      subtitle: _subtitle,
                      progress: _progress,
                      loading: _loading,
                      week: _week,
                    ),

                    if (_hasBaby) const _SwipeHint(),

                    const SizedBox(height: AppSpacing.xl),

                    // Veličina ploda
                    _GradientPinkCard(
                      icon: Icons.local_florist_rounded,
                      label: 'Veličina ploda',
                      onTap: () =>
                          _open(context, BabyGrowthScreen(week: _week)),
                    ),
                    const SizedBox(height: AppSpacing.md),

                    // Dnevnik simptoma
                    _GradientPinkCard(
                      icon: Icons.fact_check_rounded,
                      label: 'Dnevnik simptoma',
                      onTap: () => _open(
                        context,
                        SymptomDiaryScreen(
                          parentProfileId: widget.parentProfileId,
                        ),
                      ),
                    ),
                    const SizedBox(height: AppSpacing.md),

                    // Savjetni centar
                    _GradientPinkCard(
                      icon: Icons.lightbulb_outline_rounded,
                      label: 'Savjetni centar',
                      onTap: () => _open(
                        context,
                        AdviceCenterScreen(gestationalWeek: _week),
                      ),
                    ),
                    const SizedBox(height: AppSpacing.md),

                    // Blog
                    _GradientPinkCard(
                      icon: Icons.article_outlined,
                      label: 'Blog',
                      onTap: () => _open(
                        context,
                        BlogScreen(parentProfileId: widget.parentProfileId),
                      ),
                    ),
                    const SizedBox(height: AppSpacing.md),

                    // Pitanja
                    _GradientPinkCard(
                      icon: Icons.help_outline_rounded,
                      label: 'Pitanja',
                      onTap: () => _open(
                        context,
                        MyQuestionsScreen(
                          parentProfileId: widget.parentProfileId,
                        ),
                      ),
                    ),
                    const SizedBox(height: AppSpacing.md),

                    // Terapija
                    _GradientPinkCard(
                      icon: Icons.medical_services_outlined,
                      label: 'Terapija',
                      onTap: () => _open(
                        context,
                        TherapyCalendarScreen(
                          parentProfileId: widget.parentProfileId,
                        ),
                      ),
                    ),
                    const SizedBox(height: AppSpacing.md),

                    // Uredi profil
                    _GradientPinkCard(
                      icon: Icons.manage_accounts_rounded,
                      label: 'Uredi profil',
                      subtitle: 'Promijenite lične podatke i preferencije',
                      onTap: () =>
                          _open(context, const _Placeholder('Uredi profil')),
                    ),
                    const SizedBox(height: AppSpacing.xl),

                    // Beba je rođena
                    _BabyBornCard(
                      icon: Icons.favorite_rounded,
                      label: 'Beba je rođena',
                      subtitle: 'Zabilježite rođenje i započnite novi period',
                      onTap: () => _open(
                        context,
                        BabyProfileCreateScreen(
                          parentProfileId: widget.parentProfileId,
                          // pregnancyId: currentPregnancyId,
                        ),
                      ),
                    ),
                  ],
                ),
              ),
            ),
          ),
        ),
      ),
    );
  }

  void _open(BuildContext context, Widget page) {
    Navigator.of(context).push(MaterialPageRoute(builder: (_) => page));
  }
}

/* ---------- HEADER ---------- */

class _HeaderSimple extends StatelessWidget {
  const _HeaderSimple({
    required this.title,
    required this.subtitle,
    required this.progress,
    required this.loading,
    required this.week,
  });

  final String title;
  final String subtitle;
  final double progress;
  final bool loading;
  final int week;

  @override
  Widget build(BuildContext context) {
    return Card(
      color: AppColors.card,
      shape: RoundedRectangleBorder(
        borderRadius: BorderRadius.circular(AppRadius.xl),
      ),
      elevation: 2,
      child: Padding(
        padding: const EdgeInsets.all(AppSpacing.xl),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            if (loading) ...[
              Row(
                children: [
                  const SizedBox(
                    width: 18,
                    height: 18,
                    child: CircularProgressIndicator(strokeWidth: 2),
                  ),
                  const SizedBox(width: AppSpacing.sm),
                  Text(
                    'Učitavanje podataka o trudnoći...',
                    style: Theme.of(context).textTheme.bodyMedium?.copyWith(
                      color: AppColors.textSecondary,
                    ),
                  ),
                ],
              ),
            ] else ...[
              Text(
                title,
                style: Theme.of(context).textTheme.headlineSmall?.copyWith(
                  fontWeight: FontWeight.w900,
                  color: AppColors.roseDark,
                ),
              ),
              const SizedBox(height: AppSpacing.sm),
              Text(
                subtitle,
                style: Theme.of(context).textTheme.bodyMedium?.copyWith(
                  color: AppColors.textSecondary,
                  fontWeight: FontWeight.w600,
                ),
              ),
              const SizedBox(height: AppSpacing.lg),

              // NOVI PROGRESS BAR
              Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Container(
                    height: 16,
                    decoration: BoxDecoration(
                      borderRadius: BorderRadius.circular(999),
                      gradient: LinearGradient(
                        begin: Alignment.centerLeft,
                        end: Alignment.centerRight,
                        colors: [
                          AppColors.babyPink.withOpacity(0.25),
                          AppColors.babyBlue.withOpacity(0.25),
                        ],
                      ),
                    ),
                    child: ClipRRect(
                      borderRadius: BorderRadius.circular(999),
                      child: Stack(
                        children: [
                          // popunjeni dio – tamniji
                          FractionallySizedBox(
                            widthFactor: progress.clamp(0.0, 1.0),
                            alignment: Alignment.centerLeft,
                            child: Container(
                              decoration: const BoxDecoration(
                                gradient: LinearGradient(
                                  colors: [
                                    AppColors.babyPink,
                                    AppColors.roseDark,
                                  ],
                                  begin: Alignment.centerLeft,
                                  end: Alignment.centerRight,
                                ),
                              ),
                            ),
                          ),
                        ],
                      ),
                    ),
                  ),
                  const SizedBox(height: 6),
                  Text(
                    '${week.clamp(1, 40)} / 40 sedmica  •  ${(progress * 100).clamp(0, 100).round()}% trudnoće',
                    style: Theme.of(context).textTheme.bodySmall?.copyWith(
                      color: AppColors.textSecondary,
                      fontWeight: FontWeight.w600,
                    ),
                  ),
                ],
              ),
            ],
          ],
        ),
      ),
    );
  }
}

class _SwipeHint extends StatelessWidget {
  const _SwipeHint();

  @override
  Widget build(BuildContext context) {
    return Row(
      mainAxisAlignment: MainAxisAlignment.center,
      children: [
        Icon(
          Icons.swipe_left_rounded,
          size: 20,
          color: AppColors.textSecondary.withOpacity(0.8),
        ),
        const SizedBox(width: 6),
        Text(
          'Prevucite ekran nalijevo za BabyTime',
          style: Theme.of(context).textTheme.bodySmall?.copyWith(
            color: AppColors.textSecondary.withOpacity(0.9),
            fontWeight: FontWeight.w600,
          ),
        ),
      ],
    );
  }
}

class _GradientPinkCard extends StatelessWidget {
  const _GradientPinkCard({
    required this.icon,
    required this.label,
    required this.onTap,
    this.subtitle,
  });

  final IconData icon;
  final String label;
  final String? subtitle;
  final VoidCallback onTap;

  @override
  Widget build(BuildContext context) {
    return Card(
      elevation: 3,
      shape: RoundedRectangleBorder(
        borderRadius: BorderRadius.circular(AppRadius.lg),
      ),
      child: InkWell(
        borderRadius: BorderRadius.circular(AppRadius.lg),
        onTap: onTap,
        child: Ink(
          decoration: const BoxDecoration(
            gradient: LinearGradient(
              colors: [AppColors.babyPink, Colors.white],
              begin: Alignment.topLeft,
              end: Alignment.bottomRight,
            ),
            borderRadius: BorderRadius.all(Radius.circular(AppRadius.lg)),
          ),
          padding: const EdgeInsets.all(AppSpacing.lg),
          child: Row(
            children: [
              Container(
                height: 50,
                width: 50,
                decoration: BoxDecoration(
                  color: Colors.white,
                  borderRadius: BorderRadius.circular(14),
                  boxShadow: [
                    BoxShadow(
                      color: Colors.black.withOpacity(0.08),
                      blurRadius: 6,
                      offset: const Offset(0, 3),
                    ),
                  ],
                ),
                child: Icon(icon, color: AppColors.roseDark, size: 28),
              ),
              const SizedBox(width: AppSpacing.lg),
              Expanded(
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
                    if (subtitle != null)
                      Text(
                        subtitle!,
                        style: Theme.of(context).textTheme.bodySmall?.copyWith(
                          color: AppColors.roseDark,
                        ),
                      ),
                  ],
                ),
              ),
              const Icon(
                Icons.chevron_right_rounded,
                color: AppColors.roseDark,
              ),
            ],
          ),
        ),
      ),
    );
  }
}

/* ---------- BEBA JE ROĐENA ---------- */

class _BabyBornCard extends StatelessWidget {
  const _BabyBornCard({
    required this.icon,
    required this.label,
    required this.subtitle,
    required this.onTap,
  });

  final IconData icon;
  final String label;
  final String subtitle;
  final VoidCallback onTap;

  @override
  Widget build(BuildContext context) {
    return Card(
      elevation: 4,
      shape: RoundedRectangleBorder(
        borderRadius: BorderRadius.circular(AppRadius.lg),
      ),
      child: InkWell(
        borderRadius: BorderRadius.circular(AppRadius.lg),
        onTap: onTap,
        child: Ink(
          decoration: const BoxDecoration(
            color: AppColors.babyPink,
            borderRadius: BorderRadius.all(Radius.circular(AppRadius.lg)),
          ),
          padding: const EdgeInsets.all(AppSpacing.lg),
          child: Row(
            children: [
              Container(
                height: 50,
                width: 50,
                decoration: BoxDecoration(
                  color: Colors.white,
                  borderRadius: BorderRadius.circular(14),
                  boxShadow: [
                    BoxShadow(
                      color: Colors.black.withOpacity(0.1),
                      blurRadius: 6,
                      offset: const Offset(0, 3),
                    ),
                  ],
                ),
                child: const Icon(
                  Icons.favorite_rounded,
                  color: AppColors.roseDark,
                  size: 28,
                ),
              ),
              const SizedBox(width: AppSpacing.lg),
              Expanded(
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
                    const SizedBox(height: 4),
                    Text(
                      subtitle,
                      style: Theme.of(context).textTheme.bodySmall?.copyWith(
                        color: AppColors.roseDark.withOpacity(0.95),
                      ),
                    ),
                  ],
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }
}

/* ---------- PLACEHOLDER ---------- */

class _Placeholder extends StatelessWidget {
  const _Placeholder(this.title);
  final String title;

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: Text(title)),
      body: Center(
        child: Text(
          '$title (uskoro)',
          style: Theme.of(context).textTheme.titleLarge,
        ),
      ),
    );
  }
}
