import 'package:flutter/material.dart';
import 'package:flutter_application_nestly/main.dart';

class NestlyToast {
  static void success(
    BuildContext context,
    String message, {
    Color? accentColor,
  }) {
    final color = accentColor ?? AppColors.roseDark;

    _show(
      context,
      message: message,
      icon: Icons.check_circle_rounded,
      iconColor: Colors.white,
      chipBg: color,
      borderColor: color.withOpacity(.25),
      textColor: color,
    );
  }

  static void info(BuildContext context, String message, {Color? accentColor}) {
    final color = accentColor ?? AppColors.babyBlue;

    _show(
      context,
      message: message,
      icon: Icons.info_rounded,
      iconColor: Colors.white,
      chipBg: color,
      borderColor: color.withOpacity(.35),
      textColor: AppColors.textPrimary,
    );
  }

  static void warning(
    BuildContext context,
    String message, {
    Color? accentColor,
    Color? textColor,
  }) {
    final bg = accentColor ?? Colors.orange;

    _show(
      context,
      message: message,
      icon: Icons.warning_amber_rounded,
      iconColor: Colors.white,
      chipBg: bg,
      borderColor: bg.withOpacity(.35),
      textColor: textColor ?? AppColors.textPrimary,
    );
  }

  static void error(
    BuildContext context,
    String message, {
    Color? accentColor,
    Color? textColor,
  }) {
    final bg = accentColor ?? Colors.redAccent;

    _show(
      context,
      message: message,
      icon: Icons.error_rounded,
      iconColor: Colors.white,
      chipBg: bg,
      borderColor: bg.withOpacity(.35),
      textColor: textColor ?? AppColors.textPrimary,
    );
  }

  static void _show(
    BuildContext context, {
    required String message,
    required IconData icon,
    required Color iconColor,
    required Color chipBg,
    required Color borderColor,
    required Color textColor,
  }) {
    final overlay = navigatorKey.currentState?.overlay;

    if (overlay == null) return;

    late OverlayEntry entry;

    entry = OverlayEntry(
      builder: (context) => Positioned(
        bottom: 40,
        left: 16,
        right: 16,
        child: Material(
          color: Colors.transparent,
          child: Container(
            padding: const EdgeInsets.symmetric(horizontal: 12, vertical: 12),
            decoration: BoxDecoration(
              color: AppColors.card,
              borderRadius: BorderRadius.circular(16),
              border: Border.all(color: borderColor, width: 1),
              boxShadow: [
                BoxShadow(
                  color: Colors.black.withOpacity(.1),
                  blurRadius: 16,
                  offset: const Offset(0, 8),
                ),
              ],
            ),
            child: Row(
              children: [
                Container(
                  width: 34,
                  height: 34,
                  decoration: BoxDecoration(
                    color: chipBg,
                    shape: BoxShape.circle,
                  ),
                  child: Icon(icon, size: 20, color: iconColor),
                ),
                const SizedBox(width: 12),
                Expanded(
                  child: Text(
                    message,
                    style: TextStyle(
                      color: textColor,
                      fontWeight: FontWeight.w700,
                    ),
                  ),
                ),
                GestureDetector(
                  onTap: () => entry.remove(),
                  child: const Icon(
                    Icons.close_rounded,
                    size: 18,
                    color: AppColors.textSecondary,
                  ),
                ),
              ],
            ),
          ),
        ),
      ),
    );

    overlay.insert(entry);

    Future.delayed(const Duration(seconds: 2), () {
      if (entry.mounted) entry.remove();
    });
  }
}
