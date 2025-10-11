import 'package:flutter/material.dart';
import 'package:flutter_application_nestly/main.dart';
import 'package:flutter_application_nestly/screens/advice_center_screen.dart';
import 'package:flutter_application_nestly/screens/baby_growth_screen.dart';
import 'package:flutter_application_nestly/screens/symptom_diary_screen.dart';

class HomeDashboardScreen extends StatelessWidget {
  const HomeDashboardScreen({super.key, required this.conceptionDate});

  final DateTime conceptionDate;

  static const int _GESTATION_DAYS = 266;
  static const Color _roseDark = Color(0xFFE68FB0);

  int _daysSinceConception() {
    final now = DateTime.now();
    return now.difference(conceptionDate).inDays.clamp(0, 10000);
  }

  int _currentWeek() {
    final days = _daysSinceConception();
    final week = (days / 7).floor() + 1;
    return week < 1 ? 1 : week;
  }

  int _daysRemaining() {
    return _GESTATION_DAYS - _daysSinceConception();
  }

  double _progress() {
    final done = _daysSinceConception().clamp(0, _GESTATION_DAYS);
    return (done / _GESTATION_DAYS).clamp(0.0, 1.0);
  }

  @override
  Widget build(BuildContext context) {
    final week = _currentWeek();
    final remaining = _daysRemaining();
    final subtitle = remaining > 0
        ? 'Preostalo $remaining dana'
        : 'Termin je prošao';

    return Scaffold(
      backgroundColor: AppColors.bg,
      body: SafeArea(
        child: Center(
          child: SingleChildScrollView(
            padding: const EdgeInsets.all(AppSpacing.xl),
            child: ConstrainedBox(
              constraints: const BoxConstraints(maxWidth: 520),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.stretch,
                children: [
                  _HeaderSimple(
                    title: 'Sedmica $week',
                    subtitle: subtitle,
                    progress: _progress(),
                  ),
                  const SizedBox(height: AppSpacing.xl),

                  _GradientPinkCard(
                    icon: Icons.local_florist_rounded,
                    label: 'Veličina ploda',
                    onTap: () => _open(
                      context,
                      BabyGrowthScreen(
                        week: _currentWeek(),
                        remainingDays: _daysRemaining(),
                      ),
                    ),
                  ),
                  const SizedBox(height: AppSpacing.md),
                  _GradientPinkCard(
                    icon: Icons.fact_check_rounded,
                    label: 'Dnevnik simptoma',
                    onTap: () => _open(context, const SymptomDiaryScreen()),
                  ),
                  const SizedBox(height: AppSpacing.md),
                  _GradientPinkCard(
                    icon: Icons.lightbulb_outline_rounded,
                    label: 'Savjetni centar',
                    onTap: () => _open(
                      context,
                      const AdviceCenterScreen(
                        /* highlightWeek: currentWeek */
                      ),
                    ),
                  ),

                  const SizedBox(height: AppSpacing.md),
                  _GradientPinkCard(
                    icon: Icons.article_outlined,
                    label: 'Blog',
                    onTap: () => _open(context, const _Placeholder('Blog')),
                  ),
                  const SizedBox(height: AppSpacing.md),
                  _GradientPinkCard(
                    icon: Icons.help_outline_rounded,
                    label: 'Pitanja',
                    onTap: () => _open(context, const _Placeholder('Pitanja')),
                  ),
                  const SizedBox(height: AppSpacing.md),
                  _GradientPinkCard(
                    icon: Icons.medical_services_outlined,
                    label: 'Terapija',
                    onTap: () => _open(context, const _Placeholder('Terapija')),
                  ),
                  const SizedBox(height: AppSpacing.md),
                  _GradientPinkCard(
                    icon: Icons.manage_accounts_rounded,
                    label: 'Uredi profil',
                    subtitle: 'Promijenite lične podatke i preferencije',
                    onTap: () =>
                        _open(context, const _Placeholder('Uredi profil')),
                  ),
                  const SizedBox(height: AppSpacing.xl),

                  _BabyBornCard(
                    icon: Icons.favorite_rounded,
                    label: 'Beba je rođena',
                    subtitle: 'Zabilježite rođenje i započnite novi period',
                    onTap: () =>
                        _open(context, const _Placeholder('Beba je rođena')),
                  ),
                ],
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
  });

  final String title;
  final String subtitle;
  final double progress;

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
            Container(
              height: 12,
              decoration: BoxDecoration(
                borderRadius: BorderRadius.circular(12),
                color: Colors.grey.shade200,
              ),
              child: ClipRRect(
                borderRadius: BorderRadius.circular(12),
                child: Stack(
                  children: [
                    FractionallySizedBox(
                      widthFactor: progress,
                      child: Container(
                        decoration: const BoxDecoration(
                          gradient: LinearGradient(
                            colors: [AppColors.babyPink, AppColors.roseDark],
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
          ],
        ),
      ),
    );
  }
}

/* ---------- KARTICE: babyPink → white + roze ikonice ---------- */
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
