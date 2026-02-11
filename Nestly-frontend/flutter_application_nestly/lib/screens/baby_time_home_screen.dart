import 'package:flutter/material.dart';
import 'package:flutter_application_nestly/main.dart';

import 'package:flutter_application_nestly/screens/baby_growth_tracker_screen.dart'
    show BabyGrowthTrackerScreen;
import 'package:flutter_application_nestly/screens/calendar_event_screen.dart';
import 'package:flutter_application_nestly/screens/chat_home_screen.dart';
import 'package:flutter_application_nestly/screens/diaper_log_calendar_screen.dart';
import 'package:flutter_application_nestly/screens/health_tracking_screen.dart';
import 'package:flutter_application_nestly/screens/meal_plan_screen.dart'
    show MealRecommendationScreen;
import 'package:flutter_application_nestly/screens/milestone_screen.dart'
    show MilestoneScreen;
import 'package:flutter_application_nestly/screens/sleep_log_screen.dart'
    show SleepLogOverviewScreen;

class BabyTimeHomeScreen extends StatelessWidget {
  final String babyName;
  final int babyId;
  final int parentProfileId;

  const BabyTimeHomeScreen({
    super.key,
    required this.babyName,
    required this.babyId,
    required this.parentProfileId,
  });

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppColors.bg,
      appBar: _buildAppBar(context),
      floatingActionButton: _buildFab(),
      body: Padding(
        padding: const EdgeInsets.fromLTRB(24, 0, 24, 24),
        child: Column(
          children: [
            _BabyHeaderBanner(babyName: babyName),
            const SizedBox(height: 20),
            Expanded(child: _buildMenuGrid(context)),
          ],
        ),
      ),
    );
  }

  /* ==================== APP BAR ==================== */

  AppBar _buildAppBar(BuildContext context) {
    return AppBar(
      backgroundColor: Colors.transparent,
      elevation: 0,
      centerTitle: true,
      title: Text(
        'BabyTime',
        style: Theme.of(context).textTheme.titleLarge?.copyWith(
          color: AppColors.roseDark,
          fontWeight: FontWeight.w800,
        ),
      ),
    );
  }

  /* ==================== FAB ==================== */

  Widget _buildFab() {
    return FloatingActionButton.extended(
      onPressed: () {},
      backgroundColor: AppColors.roseDark,
      foregroundColor: Colors.white,
      icon: const Icon(Icons.add),
      label: const Text(
        'Dodaj bebu',
        style: TextStyle(fontWeight: FontWeight.w700),
      ),
    );
  }

  /* ==================== GRID ==================== */

  Widget _buildMenuGrid(BuildContext context) {
    return GridView.count(
      padding: const EdgeInsets.only(bottom: 8),
      physics: const BouncingScrollPhysics(),
      crossAxisCount: 2,
      childAspectRatio: 1.05,
      crossAxisSpacing: 18,
      mainAxisSpacing: 18,
      children: [
        _BabyMenuItem(
          icon: Icons.show_chart_rounded,
          label: 'Praćenje rasta',
          color: AppColors.babyBlue,
          onTap: () => _push(
            context,
            BabyGrowthTrackerScreen(babyId: babyId, babyName: babyName),
          ),
        ),
        _BabyMenuItem(
          icon: Icons.local_drink_rounded,
          label: 'Dnevnik hranjenja',
          color: AppColors.babyPink,
          onTap: () {},
        ),
        _BabyMenuItem(
          icon: Icons.nights_stay_rounded,
          label: 'Dnevnik spavanja',
          color: AppColors.babyBlue,
          onTap: () => _push(
            context,
            SleepLogOverviewScreen(babyId: babyId, babyName: babyName),
          ),
        ),
        _BabyMenuItem(
          icon: Icons.restaurant_rounded,
          label: 'Plan ishrane',
          color: AppColors.babyPink,
          onTap: () => _push(context, MealRecommendationScreen(babyId: babyId)),
        ),
        _BabyMenuItem(
          icon: Icons.event_note_rounded,
          label: 'Kalendar termina',
          color: AppColors.babyBlue,
          onTap: () => _push(
            context,
            CalendarEventScreen(
              babyId: babyId,
              babyName: babyName,
              userId: parentProfileId,
            ),
          ),
        ),
        _BabyMenuItem(
          icon: Icons.chat_bubble_outline_rounded,
          label: 'Chat',
          color: AppColors.babyPink,
          onTap: () =>
              _push(context, ChatHomeScreen(currentUserId: parentProfileId)),
        ),

        _BabyMenuItem(
          icon: Icons.emoji_events_rounded,
          label: 'Dostignuća',
          color: AppColors.babyBlue,
          onTap: () => _push(
            context,
            MilestoneScreen(babyId: babyId, babyName: babyName),
          ),
        ),
        _BabyMenuItem(
          icon: Icons.favorite_border_rounded,
          label: 'Praćenje zdravlja',
          color: AppColors.babyPink,
          onTap: () => _push(
            context,
            HealthTrackingScreen(
              babyId: babyId,
              babyName: babyName,
              userId: parentProfileId,
            ),
          ),
        ),
        _BabyMenuItem(
          icon: Icons.favorite_border_rounded,
          label: 'Praćenje pelena',
          color: AppColors.babyPink,
          onTap: () => _push(context, DiaperLogCalendarScreen(babyId: babyId)),
        ),
      ],
    );
  }

  void _push(BuildContext context, Widget screen) {
    Navigator.of(context).push(MaterialPageRoute(builder: (_) => screen));
  }
}

/* ==================== HEADER ==================== */

class _BabyHeaderBanner extends StatelessWidget {
  final String babyName;

  const _BabyHeaderBanner({required this.babyName});

  @override
  Widget build(BuildContext context) {
    return Container(
      height: 160,
      decoration: BoxDecoration(
        borderRadius: BorderRadius.circular(28),
        gradient: LinearGradient(
          begin: Alignment.topLeft,
          end: Alignment.bottomRight,
          colors: [
            AppColors.babyBlue.withOpacity(0.35),
            AppColors.babyPink.withOpacity(0.35),
          ],
        ),
      ),
      child: Stack(
        children: [
          Positioned(
            top: 14,
            left: 18,
            child: Icon(
              Icons.cloud_rounded,
              size: 26,
              color: Colors.white.withOpacity(0.85),
            ),
          ),
          Positioned(
            top: 20,
            right: 24,
            child: Icon(
              Icons.star_rounded,
              size: 22,
              color: Colors.white.withOpacity(0.9),
            ),
          ),
          Padding(
            padding: const EdgeInsets.symmetric(horizontal: 24),
            child: Row(
              children: [
                _BabyAvatar(babyName: babyName),
                const SizedBox(width: 18),
                Expanded(
                  child: Column(
                    mainAxisAlignment: MainAxisAlignment.center,
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Text(
                        babyName,
                        style: Theme.of(context).textTheme.titleMedium
                            ?.copyWith(
                              color: AppColors.textPrimary,
                              fontWeight: FontWeight.w800,
                            ),
                      ),
                      const SizedBox(height: 6),
                      Text(
                        'Sve važne stvari o vašoj bebi na jednom mjestu.',
                        style: Theme.of(context).textTheme.bodySmall?.copyWith(
                          color: AppColors.textSecondary,
                          height: 1.3,
                        ),
                      ),
                    ],
                  ),
                ),
              ],
            ),
          ),
        ],
      ),
    );
  }
}

/* ==================== AVATAR ==================== */

class _BabyAvatar extends StatelessWidget {
  final String babyName;

  const _BabyAvatar({required this.babyName});

  String _initials() {
    final trimmed = babyName.trim();
    if (trimmed.isEmpty) return '👶';

    final parts = trimmed.split(' ');
    if (parts.length == 1) {
      return parts.first.characters.first.toUpperCase();
    }

    return (parts[0].characters.first + parts[1].characters.first)
        .toUpperCase();
  }

  @override
  Widget build(BuildContext context) {
    return Container(
      width: 70,
      height: 70,
      decoration: BoxDecoration(
        shape: BoxShape.circle,
        gradient: LinearGradient(colors: [AppColors.babyPink, AppColors.seed]),
        boxShadow: [
          BoxShadow(
            color: AppColors.babyPink.withOpacity(.35),
            blurRadius: 10,
            offset: const Offset(0, 4),
          ),
        ],
      ),
      child: Center(
        child: Text(
          _initials(),
          style: const TextStyle(
            fontSize: 24,
            fontWeight: FontWeight.w800,
            color: Colors.white,
          ),
        ),
      ),
    );
  }
}

/* ==================== MENU ITEM ==================== */

class _BabyMenuItem extends StatefulWidget {
  final IconData icon;
  final String label;
  final Color color;
  final VoidCallback onTap;

  const _BabyMenuItem({
    required this.icon,
    required this.label,
    required this.color,
    required this.onTap,
  });

  @override
  State<_BabyMenuItem> createState() => _BabyMenuItemState();
}

class _BabyMenuItemState extends State<_BabyMenuItem>
    with SingleTickerProviderStateMixin {
  late final AnimationController _controller;
  late final Animation<double> _scale;

  @override
  void initState() {
    super.initState();

    _controller = AnimationController(
      vsync: this,
      duration: const Duration(milliseconds: 900),
    )..repeat(reverse: true);

    _scale = Tween<double>(
      begin: 0.97,
      end: 1.02,
    ).animate(CurvedAnimation(parent: _controller, curve: Curves.easeInOut));
  }

  @override
  void dispose() {
    _controller.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    final accent = widget.color;

    return InkWell(
      borderRadius: BorderRadius.circular(22),
      onTap: widget.onTap,
      child: Ink(
        decoration: BoxDecoration(
          color: AppColors.card,
          borderRadius: BorderRadius.circular(22),
          border: Border.all(color: accent.withOpacity(0.45), width: 1.1),
          boxShadow: [
            BoxShadow(
              color: accent.withOpacity(0.10),
              blurRadius: 6,
              offset: const Offset(0, 3),
            ),
          ],
        ),
        child: Padding(
          padding: const EdgeInsets.all(14),
          child: Column(
            mainAxisAlignment: MainAxisAlignment.center,
            children: [
              ScaleTransition(
                scale: _scale,
                child: Container(
                  width: 46,
                  height: 46,
                  decoration: BoxDecoration(
                    shape: BoxShape.circle,
                    color: accent,
                  ),
                  child: Icon(widget.icon, size: 24, color: Colors.white),
                ),
              ),
              const SizedBox(height: 10),
              Text(
                widget.label,
                textAlign: TextAlign.center,
                style: Theme.of(context).textTheme.bodyMedium?.copyWith(
                  fontWeight: FontWeight.w700,
                  color: accent,
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }
}
