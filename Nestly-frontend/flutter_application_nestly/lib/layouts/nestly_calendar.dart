import 'package:flutter/material.dart';
import 'package:table_calendar/table_calendar.dart';
import '../main.dart';

typedef CalendarEventLoader = List<Object> Function(DateTime day);

class NestlyCalendar extends StatelessWidget {
  const NestlyCalendar({
    super.key,
    required this.focusedDay,
    required this.selectedDay,
    required this.onDaySelected,
    required this.eventLoader,
    this.onPageChanged,
    this.firstDay,
    this.lastDay,
    this.markerIcon,
    this.accentColor,
  });

  final DateTime focusedDay;
  final DateTime? selectedDay;

  final void Function(DateTime selected, DateTime focused) onDaySelected;
  final void Function(DateTime focused)? onPageChanged;

  final CalendarEventLoader eventLoader;

  final DateTime? firstDay;
  final DateTime? lastDay;
  final IconData? markerIcon;

  /// 🎨 opcionalna boja
  /// ako se ne pošalje → koristi roseDark
  final Color? accentColor;

  bool _isSameDay(DateTime? a, DateTime b) {
    if (a == null) return false;
    return a.year == b.year && a.month == b.month && a.day == b.day;
  }

  @override
  Widget build(BuildContext context) {
    final color = accentColor ?? AppColors.roseDark;

    return TableCalendar(
      focusedDay: focusedDay,
      firstDay: firstDay ?? DateTime.utc(2024, 1, 1),
      lastDay: lastDay ?? DateTime.utc(2026, 12, 31),
      locale: 'bs_BA',
      calendarFormat: CalendarFormat.month,

      selectedDayPredicate: (day) => _isSameDay(selectedDay, day),

      headerStyle: HeaderStyle(
        formatButtonVisible: false,
        titleCentered: true,
        titleTextStyle: TextStyle(fontWeight: FontWeight.bold, color: color),
        leftChevronIcon: Icon(Icons.chevron_left_rounded, color: color),
        rightChevronIcon: Icon(Icons.chevron_right_rounded, color: color),
      ),

      calendarStyle: CalendarStyle(
        outsideDaysVisible: false,
        selectedDecoration: BoxDecoration(color: color, shape: BoxShape.circle),
        todayDecoration: BoxDecoration(
          color: color.withOpacity(.4),
          shape: BoxShape.circle,
        ),
      ),

      eventLoader: eventLoader,

      onDaySelected: onDaySelected,

      onPageChanged: onPageChanged,

      calendarBuilders: CalendarBuilders(
        markerBuilder: (context, day, events) {
          if (events.isEmpty) return null;

          return Align(
            alignment: Alignment.bottomCenter,
            child: Padding(
              padding: const EdgeInsets.only(bottom: 4),
              child: Icon(markerIcon ?? Icons.circle, size: 14, color: color),
            ),
          );
        },
      ),
    );
  }
}
