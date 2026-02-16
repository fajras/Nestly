import 'package:flutter/material.dart';
import 'package:flutter_application_nestly/main.dart';

import 'package:flutter_application_nestly/screens/baby_growth_tracker_screen.dart';
import 'package:flutter_application_nestly/screens/calendar_event_screen.dart';
import 'package:flutter_application_nestly/screens/chat_home_screen.dart';
import 'package:flutter_application_nestly/screens/diaper_log_calendar_screen.dart';
import 'package:flutter_application_nestly/screens/feeding_calendar_screen.dart';
import 'package:flutter_application_nestly/screens/health_tracking_screen.dart';
import 'package:flutter_application_nestly/screens/meal_plan_screen.dart';
import 'package:flutter_application_nestly/screens/milestone_screen.dart';
import 'package:flutter_application_nestly/screens/sleep_log_screen.dart';

class BabyTimeHomeScreen extends StatelessWidget {
  final String babyName;
  final int babyId;
  final int parentProfileId;
  final String gender;

  const BabyTimeHomeScreen({
    super.key,
    required this.babyName,
    required this.babyId,
    required this.parentProfileId,
    required this.gender,
  });
  bool get _isGirl {
    final g = gender.toLowerCase();
    return g == 'female' || g == 'f';
  }

  void _push(BuildContext context, Widget screen) {
    Navigator.of(context).push(MaterialPageRoute(builder: (_) => screen));
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
          'BabyTime',
          style: Theme.of(context).textTheme.titleLarge?.copyWith(
            color: AppColors.seed,
            fontWeight: FontWeight.w900,
          ),
        ),
      ),
      body: SafeArea(
        child: SingleChildScrollView(
          padding: const EdgeInsets.all(AppSpacing.xl),
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.stretch,
            children: [
              _HeaderCard(babyName: babyName, isGirl: _isGirl),
              const SizedBox(height: AppSpacing.xl),

              _menu(
                context,
                AppColors.seed,
                Icons.show_chart_rounded,
                'Praćenje rasta',
                () => _push(
                  context,
                  BabyGrowthTrackerScreen(babyId: babyId, babyName: babyName),
                ),
              ),

              _menu(
                context,
                AppColors.roseDark,
                Icons.restaurant_rounded,
                'Plan ishrane',
                () => _push(context, MealRecommendationScreen(babyId: babyId)),
              ),

              _menu(
                context,
                AppColors.seed,
                Icons.local_drink_rounded,
                'Dnevnik hranjenja',
                () => _push(context, FeedingCalendarScreen(babyId: babyId)),
              ),

              _menu(
                context,
                AppColors.roseDark,
                Icons.favorite_border_rounded,
                'Praćenje zdravlja',
                () => _push(
                  context,
                  HealthTrackingScreen(
                    babyId: babyId,
                    babyName: babyName,
                    userId: parentProfileId,
                  ),
                ),
              ),

              _menu(
                context,
                AppColors.seed,
                Icons.nights_stay_rounded,
                'Dnevnik spavanja',
                () => _push(
                  context,
                  SleepLogOverviewScreen(babyId: babyId, babyName: babyName),
                ),
              ),

              _menu(
                context,
                AppColors.roseDark,
                Icons.baby_changing_station_rounded,
                'Praćenje pelena',
                () => _push(context, DiaperLogCalendarScreen(babyId: babyId)),
              ),

              _menu(
                context,
                AppColors.seed,
                Icons.emoji_events_rounded,
                'Dostignuća',
                () => _push(
                  context,
                  MilestoneScreen(babyId: babyId, babyName: babyName),
                ),
              ),

              _menu(
                context,
                AppColors.roseDark,
                Icons.chat_bubble_outline_rounded,
                'Chat',
                () => _push(
                  context,
                  ChatHomeScreen(currentUserId: parentProfileId),
                ),
              ),

              _menu(
                context,
                AppColors.seed,
                Icons.event_note_rounded,
                'Kalendar termina',
                () => _push(
                  context,
                  CalendarEventScreen(
                    babyId: babyId,
                    babyName: babyName,
                    userId: parentProfileId,
                  ),
                ),
              ),

              const SizedBox(height: AppSpacing.xl),

              _BackCard(onTap: () => Navigator.pop(context)),
            ],
          ),
        ),
      ),
    );
  }

  Widget _menu(
    BuildContext context,
    Color color,
    IconData icon,
    String label,
    VoidCallback onTap,
  ) {
    return Padding(
      padding: const EdgeInsets.only(bottom: AppSpacing.md),
      child: Card(
        elevation: 3,
        shape: RoundedRectangleBorder(
          borderRadius: BorderRadius.circular(AppRadius.lg),
        ),
        child: InkWell(
          onTap: onTap,
          borderRadius: BorderRadius.circular(AppRadius.lg),
          child: Ink(
            padding: const EdgeInsets.all(AppSpacing.lg),
            child: Row(
              children: [
                Icon(icon, color: color, size: 28),
                const SizedBox(width: AppSpacing.lg),
                Expanded(
                  child: Text(
                    label,
                    style: Theme.of(context).textTheme.titleMedium?.copyWith(
                      fontWeight: FontWeight.w700,
                      color: color,
                    ),
                  ),
                ),
                Icon(Icons.chevron_right_rounded, color: color),
              ],
            ),
          ),
        ),
      ),
    );
  }
}

/* ================= HEADER ================= */

class _HeaderCard extends StatelessWidget {
  final String babyName;
  final bool isGirl;

  const _HeaderCard({required this.babyName, required this.isGirl});

  @override
  Widget build(BuildContext context) {
    final headerColor = isGirl ? AppColors.babyPink : AppColors.babyBlue;

    return Card(
      elevation: 3,
      shape: RoundedRectangleBorder(
        borderRadius: BorderRadius.circular(AppRadius.xl),
      ),
      child: Ink(
        decoration: BoxDecoration(
          borderRadius: BorderRadius.circular(AppRadius.xl),
          gradient: LinearGradient(
            colors: [headerColor.withOpacity(.18), AppColors.card],
            begin: Alignment.topLeft,
            end: Alignment.bottomRight,
          ),
        ),
        child: Padding(
          padding: const EdgeInsets.all(AppSpacing.xl),
          child: Row(
            children: [
              Container(
                width: 64,
                height: 64,
                decoration: BoxDecoration(
                  color: headerColor,
                  shape: BoxShape.circle,
                ),
                child: const Icon(
                  Icons.child_care_rounded,
                  color: Colors.white,
                  size: 32,
                ),
              ),
              const SizedBox(width: AppSpacing.lg),
              Expanded(
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Text(
                      babyName,
                      style: Theme.of(context).textTheme.titleLarge?.copyWith(
                        fontWeight: FontWeight.w900,
                        color: headerColor,
                      ),
                    ),
                    const SizedBox(height: 4),
                    Text(
                      'Sve važne stvari o vašoj bebi na jednom mjestu',
                      style: Theme.of(context).textTheme.bodySmall?.copyWith(
                        color: AppColors.textSecondary,
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

/* ================= BACK CARD ================= */

class _BackCard extends StatelessWidget {
  final VoidCallback onTap;

  const _BackCard({required this.onTap});

  @override
  Widget build(BuildContext context) {
    return Card(
      elevation: 3,
      shape: RoundedRectangleBorder(
        borderRadius: BorderRadius.circular(AppRadius.lg),
      ),
      child: InkWell(
        onTap: onTap,
        borderRadius: BorderRadius.circular(AppRadius.lg),
        child: Ink(
          padding: const EdgeInsets.all(AppSpacing.lg),
          child: Row(
            children: [
              Container(
                width: 46,
                height: 46,
                decoration: BoxDecoration(
                  color: AppColors.roseDark.withOpacity(.12),
                  shape: BoxShape.circle,
                ),
                child: const Icon(
                  Icons.arrow_back_rounded,
                  color: AppColors.roseDark,
                ),
              ),
              const SizedBox(width: AppSpacing.lg),
              Expanded(
                child: Text(
                  'Povratak u mamin svijet',
                  style: Theme.of(context).textTheme.titleMedium?.copyWith(
                    fontWeight: FontWeight.w800,
                    color: AppColors.roseDark,
                  ),
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }
}
