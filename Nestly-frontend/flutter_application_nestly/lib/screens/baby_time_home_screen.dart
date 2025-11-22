import 'package:flutter/material.dart';
import 'package:flutter_application_nestly/main.dart';
import 'package:flutter_application_nestly/screens/baby_growth_tracker_screen.dart'
    show BabyGrowthScreen, BabyGrowthTrackerScreen;

class BabyTimeHomeScreen extends StatelessWidget {
  final String babyName;
  final int babyId;

  const BabyTimeHomeScreen({
    super.key,
    required this.babyName,
    required this.babyId,
  });

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppColors.bg,
      appBar: AppBar(
        backgroundColor: Colors.transparent,
        elevation: 0,
        centerTitle: true,
        title: Text(
          "BabyTime",
          style: Theme.of(context).textTheme.titleLarge?.copyWith(
            color: AppColors.roseDark,
            fontWeight: FontWeight.w800,
          ),
        ),
      ),
      floatingActionButton: FloatingActionButton.extended(
        onPressed: () {
          // TODO: otvori ekran za dodavanje nove bebe
        },
        backgroundColor: AppColors.roseDark,
        foregroundColor: Colors.white,
        icon: const Icon(Icons.add),
        label: const Text(
          "Dodaj bebu",
          style: TextStyle(fontWeight: FontWeight.w700),
        ),
      ),
      body: Padding(
        padding: const EdgeInsets.fromLTRB(24, 0, 24, 24),
        child: Column(
          children: [
            _BabyHeaderBanner(babyName: babyName),
            const SizedBox(height: 20),
            Expanded(
              child: GridView.count(
                crossAxisCount: 2,
                childAspectRatio: 1.05,
                crossAxisSpacing: 18,
                mainAxisSpacing: 18,
                children: [
                  _BabyMenuItem(
                    icon: Icons.show_chart_rounded,
                    label: "Praćenje rasta",
                    color: AppColors.babyBlue,
                    onTap: () {
                      Navigator.of(context).push(
                        MaterialPageRoute(
                          builder: (_) => BabyGrowthTrackerScreen(
                            babyId: babyId,
                            babyName: babyName,
                          ),
                        ),
                      );
                    },
                  ),

                  _BabyMenuItem(
                    icon: Icons.local_drink_rounded,
                    label: "Dnevnik hranjenja",
                    color: AppColors.babyPink,
                    onTap: () {},
                  ),
                  _BabyMenuItem(
                    icon: Icons.nights_stay_rounded,
                    label: "Dnevnik spavanja",
                    color: AppColors.babyBlue,
                    onTap: () {},
                  ),
                  _BabyMenuItem(
                    icon: Icons.restaurant_rounded,
                    label: "Plan ishrane",
                    color: AppColors.babyPink,
                    onTap: () {},
                  ),
                  _BabyMenuItem(
                    icon: Icons.event_note_rounded,
                    label: "Kalendar termina",
                    color: AppColors.babyBlue,
                    onTap: () {},
                  ),
                  _BabyMenuItem(
                    icon: Icons.chat_bubble_outline_rounded,
                    label: "Chat",
                    color: AppColors.babyPink,
                    onTap: () {},
                  ),
                  _BabyMenuItem(
                    icon: Icons.emoji_events_rounded,
                    label: "Dostignuća",
                    color: AppColors.babyBlue,
                    onTap: () {},
                  ),
                  _BabyMenuItem(
                    icon: Icons.favorite_border_rounded,
                    label: "Praćenje zdravlja",
                    color: AppColors.babyPink,
                    onTap: () {},
                  ),
                ],
              ),
            ),
          ],
        ),
      ),
    );
  }
}

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
          // lagani ukras – jedan oblak i jedna zvjezdica, ne prešareno
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
                        "Sve važne stvari o vašoj bebi na jednom mjestu.",
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

class _BabyAvatar extends StatelessWidget {
  final String babyName;

  const _BabyAvatar({required this.babyName});

  String _initials() {
    final trimmed = babyName.trim();
    if (trimmed.isEmpty) return "👶";
    final parts = trimmed.split(" ");
    if (parts.length == 1) return parts.first.characters.first.toUpperCase();
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
    );
    _scale = Tween<double>(
      begin: 0.97,
      end: 1.02,
    ).animate(CurvedAnimation(parent: _controller, curve: Curves.easeInOut));

    _controller.repeat(reverse: true);
  }

  @override
  void dispose() {
    _controller.dispose();
    super.dispose();
  }

  void _handleTap() {
    widget.onTap();
  }

  @override
  Widget build(BuildContext context) {
    final Color accent = widget.color;

    return InkWell(
      borderRadius: BorderRadius.circular(22),
      onTap: _handleTap,
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
