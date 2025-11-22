import 'package:flutter/material.dart';
import 'package:flutter_application_nestly/main.dart';

class BabyTimeHomeScreen extends StatelessWidget {
  final String babyName;

  const BabyTimeHomeScreen({super.key, required this.babyName});

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

      // FAB za dodavanje nove bebe
      floatingActionButton: FloatingActionButton.extended(
        onPressed: () {
          // TODO: ovdje otvoriš ekran za kreiranje novog baby profila
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
                childAspectRatio: 1.1,
                crossAxisSpacing: 18,
                mainAxisSpacing: 18,
                children: [
                  _BabyMenuItem(
                    icon: "📏",
                    label: "Praćenje rasta",
                    color: AppColors.babyBlue,
                    onTap: () {},
                  ),
                  _BabyMenuItem(
                    icon: "🍼",
                    label: "Dnevnik hranjenja",
                    color: AppColors.babyPink,
                    onTap: () {},
                  ),
                  _BabyMenuItem(
                    icon: "🌙",
                    label: "Dnevnik spavanja",
                    color: AppColors.babyBlue.withOpacity(0.9),
                    onTap: () {},
                  ),
                  _BabyMenuItem(
                    icon: "🍎",
                    label: "Plan ishrane",
                    color: AppColors.babyPink.withOpacity(0.9),
                    onTap: () {},
                  ),
                  _BabyMenuItem(
                    icon: "📅",
                    label: "Kalendar termina",
                    color: AppColors.babyBlue,
                    onTap: () {},
                  ),
                  _BabyMenuItem(
                    icon: "💬",
                    label: "Chat",
                    color: AppColors.babyPink,
                    onTap: () {},
                  ),
                  _BabyMenuItem(
                    icon: "⭐",
                    label: "Dostignuća",
                    color: AppColors.babyBlue.withOpacity(.85),
                    onTap: () {},
                  ),
                  _BabyMenuItem(
                    icon: "❤️",
                    label: "Praćenje zdravlja",
                    color: AppColors.babyPink.withOpacity(.85),
                    onTap: () {},
                  ),
                  _BabyMenuItem(
                    icon: "🧷",
                    label: "Praćenje pelena",
                    color: AppColors.babyBlue.withOpacity(.75),
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

/// TOP BANNER – oblaci, zvjezdice, meda + avatar bebe
class _BabyHeaderBanner extends StatelessWidget {
  final String babyName;

  const _BabyHeaderBanner({required this.babyName});

  @override
  Widget build(BuildContext context) {
    return Container(
      height: 170,
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
          // Ukrasni emoji oblaci / zvjezdice / meda
          Positioned(
            top: 12,
            left: 18,
            child: Text(
              "☁️",
              style: TextStyle(
                fontSize: 26,
                color: Colors.white.withOpacity(0.9),
              ),
            ),
          ),
          Positioned(
            top: 18,
            right: 24,
            child: Text(
              "⭐",
              style: TextStyle(
                fontSize: 22,
                color: Colors.white.withOpacity(0.9),
              ),
            ),
          ),
          Positioned(
            bottom: 12,
            right: 20,
            child: Text(
              "🧸",
              style: TextStyle(
                fontSize: 28,
                color: Colors.white.withOpacity(0.9),
              ),
            ),
          ),
          Positioned(
            bottom: 8,
            left: 40,
            child: Text(
              "☁️",
              style: TextStyle(
                fontSize: 22,
                color: Colors.white.withOpacity(0.9),
              ),
            ),
          ),

          // Sadržaj bannera
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
                        "Dobrodošli u BabyTime 💕\nPratite rast, rutine i zdravlje vaše bebe.",
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

/// Kružni avatar za bebu – pastelna pozadina + emoji
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
      width: 72,
      height: 72,
      decoration: BoxDecoration(
        shape: BoxShape.circle,
        gradient: LinearGradient(
          colors: [AppColors.babyPink, AppColors.babyBlue],
        ),
        boxShadow: [
          BoxShadow(
            color: AppColors.babyPink.withOpacity(.4),
            blurRadius: 10,
            offset: const Offset(0, 4),
          ),
        ],
      ),
      child: Center(
        child: Text(
          _initials(),
          style: const TextStyle(
            fontSize: 26,
            fontWeight: FontWeight.w800,
            color: Colors.white,
          ),
        ),
      ),
    );
  }
}

/// Jedna kartica u gridu – sa blagim bounce efektom na ikoni
class _BabyMenuItem extends StatefulWidget {
  final String icon;
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
      begin: 0.95,
      end: 1.03,
    ).animate(CurvedAnimation(parent: _controller, curve: Curves.easeInOut));

    _controller.repeat(reverse: true); // stalno lagano “diše”
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
    final color = widget.color;

    return InkWell(
      borderRadius: BorderRadius.circular(22),
      onTap: _handleTap,
      child: Ink(
        decoration: BoxDecoration(
          color: color.withOpacity(.12),
          borderRadius: BorderRadius.circular(22),
          border: Border.all(color: color.withOpacity(.5), width: 1),
          boxShadow: [
            BoxShadow(
              color: color.withOpacity(.2),
              blurRadius: 8,
              offset: const Offset(0, 4),
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
                child: Text(widget.icon, style: const TextStyle(fontSize: 36)),
              ),
              const SizedBox(height: 10),
              Text(
                widget.label,
                textAlign: TextAlign.center,
                style: Theme.of(context).textTheme.bodyMedium?.copyWith(
                  fontWeight: FontWeight.w700,
                  color: AppColors.textPrimary,
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }
}
