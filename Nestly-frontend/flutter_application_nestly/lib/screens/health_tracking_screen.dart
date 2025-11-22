import 'dart:convert';
import 'dart:io' show Platform;

import 'package:flutter/foundation.dart' show kIsWeb;
import 'package:flutter/material.dart';
import 'package:flutter_application_nestly/layouts/nestly_toast.dart';
import 'package:http/http.dart' as http;
import 'package:table_calendar/table_calendar.dart';

import 'package:flutter_application_nestly/main.dart';

/// ===============================
/// CONFIG
/// ===============================

String _devBase() {
  if (kIsWeb) return 'http://localhost:5167';
  if (Platform.isAndroid) return 'http://10.0.2.2:5167';
  if (Platform.isIOS || Platform.isMacOS) return 'http://localhost:5167';
  return 'http://localhost:5167';
}

String get _apiBase =>
    const String.fromEnvironment('API_BASE', defaultValue: '').isNotEmpty
    ? const String.fromEnvironment('API_BASE')
    : _devBase();

Map<String, String> _headers([String? token]) => {
  'Content-Type': 'application/json',
  'Accept': 'application/json',
  if (token != null) 'Authorization': 'Bearer $token',
};

/// ===============================
/// MODELI ZA HEALTH ENTRY
/// ===============================

class HealthEntry {
  final int id;
  final int babyId;
  final DateTime entryDate;
  final double? temperatureC;
  final String? medicines;
  final String? doctorVisit;

  HealthEntry({
    required this.id,
    required this.babyId,
    required this.entryDate,
    this.temperatureC,
    this.medicines,
    this.doctorVisit,
  });

  factory HealthEntry.fromJson(Map<String, dynamic> json) {
    return HealthEntry(
      id: json['id'] as int,
      babyId: json['babyId'] as int,
      entryDate: DateTime.parse(json['entryDate'] as String),
      temperatureC: json['temperatureC'] == null
          ? null
          : (json['temperatureC'] as num).toDouble(),
      medicines: json['medicines'] as String?,
      doctorVisit: json['doctorVisit'] as String?,
    );
  }
}

class CreateHealthEntryRequest {
  final int babyId;
  final DateTime entryDate;
  final double? temperatureC;
  final String? medicines;
  final String? doctorVisit;

  CreateHealthEntryRequest({
    required this.babyId,
    required this.entryDate,
    this.temperatureC,
    this.medicines,
    this.doctorVisit,
  });

  Map<String, dynamic> toJson() => {
    'babyId': babyId,
    'entryDate': entryDate.toIso8601String(),
    'temperatureC': temperatureC,
    'medicines': medicines,
    'doctorVisit': doctorVisit,
  };
}

/// Draft modeli samo za UI
class _MedicationDraft {
  final String text;
  _MedicationDraft(this.text);
}

class _CheckupDraft {
  final String text;
  _CheckupDraft(this.text);
}

/// ===============================
/// SERVICE
/// ===============================

class HealthEntryApiService {
  String get _baseUrl => '$_apiBase/api/HealthEntry';

  Future<List<HealthEntry>> getForBabyInRange({
    required int babyId,
    required DateTime from,
    required DateTime to,
    String? token,
  }) async {
    final uri = Uri.parse(
      '$_baseUrl?BabyId=$babyId'
      '&DateFrom=${from.toIso8601String()}'
      '&DateTo=${to.toIso8601String()}',
    );

    final resp = await http.get(uri, headers: _headers(token));
    if (resp.statusCode != 200) {
      throw Exception(
        'Greška pri dohvaćanju podataka o zdravlju (${resp.statusCode}).',
      );
    }

    final List<dynamic> data = jsonDecode(resp.body) as List<dynamic>;
    return data
        .map((e) => HealthEntry.fromJson(e as Map<String, dynamic>))
        .toList();
  }

  Future<HealthEntry> create({
    required CreateHealthEntryRequest request,
    String? token,
  }) async {
    final uri = Uri.parse(_baseUrl);
    final resp = await http.post(
      uri,
      headers: _headers(token),
      body: jsonEncode(request.toJson()),
    );

    if (resp.statusCode != 201 && resp.statusCode != 200) {
      throw Exception('Greška pri spremanju zapisa (${resp.statusCode}).');
    }

    final Map<String, dynamic> data =
        jsonDecode(resp.body) as Map<String, dynamic>;
    return HealthEntry.fromJson(data);
  }
}

/// ===============================
/// SCREEN
/// ===============================

class HealthTrackingScreen extends StatefulWidget {
  final int babyId;
  final String babyName;
  final int userId; // za buduće proširenje, sada se ne koristi
  final String? token;

  const HealthTrackingScreen({
    super.key,
    required this.babyId,
    required this.babyName,
    required this.userId,
    this.token,
  });

  @override
  State<HealthTrackingScreen> createState() => _HealthTrackingScreenState();
}

class _HealthTrackingScreenState extends State<HealthTrackingScreen> {
  final _service = HealthEntryApiService();

  bool _isLoading = true;
  bool _isSaving = false;

  List<HealthEntry> _entries = [];
  final Map<DateTime, List<HealthEntry>> _entriesByDay = {};

  DateTime _focusedDay = DateTime.now();
  DateTime _selectedDay = DateTime.now();

  final _tempCtrl = TextEditingController();
  final List<_MedicationDraft> _medDrafts = [];
  final List<_CheckupDraft> _checkupDrafts = [];

  @override
  void initState() {
    super.initState();
    _selectedDay = DateTime(
      DateTime.now().year,
      DateTime.now().month,
      DateTime.now().day,
    );
    _focusedDay = _selectedDay;
    _loadEntriesForMonth(_focusedDay);
  }

  @override
  void dispose() {
    _tempCtrl.dispose();
    super.dispose();
  }

  DateTime _firstDayOfMonth(DateTime date) =>
      DateTime(date.year, date.month, 1);

  DateTime _lastDayOfMonth(DateTime date) =>
      DateTime(date.year, date.month + 1, 0, 23, 59, 59);

  DateTime _dateOnly(DateTime dt) => DateTime(dt.year, dt.month, dt.day);

  Future<void> _loadEntriesForMonth(DateTime month) async {
    setState(() => _isLoading = true);
    try {
      final from = _firstDayOfMonth(month);
      final to = _lastDayOfMonth(month);

      final list = await _service.getForBabyInRange(
        babyId: widget.babyId,
        from: from,
        to: to,
        token: widget.token,
      );

      _entries = list;
      _entriesByDay.clear();

      for (final e in list) {
        final key = _dateOnly(e.entryDate);
        _entriesByDay.putIfAbsent(key, () => []);
        _entriesByDay[key]!.add(e);
      }

      setState(() => _isLoading = false);
    } catch (e) {
      setState(() => _isLoading = false);
      if (!mounted) return;
      NestlyToast.error(context, 'Greška pri učitavanju podataka: $e');
    }
  }

  List<HealthEntry> _getEntriesForDay(DateTime day) {
    return _entriesByDay[_dateOnly(day)] ?? [];
  }

  /// ==========================
  /// DODAVANJE DRAFTOVA
  /// ==========================

  Future<void> _addMedicationDraft() async {
    final ctrl = TextEditingController();

    final res = await showDialog<bool>(
      context: context,
      builder: (ctx) => AlertDialog(
        shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(18)),
        title: const Text('Dodaj lijek'),
        content: TextField(
          controller: ctrl,
          decoration: const InputDecoration(
            labelText: 'Naziv lijeka i doza (npr. Paracetamol 5 ml)',
          ),
        ),
        actions: [
          TextButton(
            onPressed: () => Navigator.pop(ctx, false),
            child: const Text('Otkaži'),
          ),
          ElevatedButton(
            onPressed: () => Navigator.pop(ctx, true),
            style: ElevatedButton.styleFrom(
              backgroundColor: AppColors.roseDark,
              foregroundColor: Colors.white,
            ),
            child: const Text('Dodaj'),
          ),
        ],
      ),
    );

    if (res == true && ctrl.text.trim().isNotEmpty) {
      setState(() {
        _medDrafts.add(_MedicationDraft(ctrl.text.trim()));
      });
      NestlyToast.success(context, 'Lijek je dodan u listu za ovaj dan.');
    }
  }

  Future<void> _addCheckupDraft() async {
    final ctrl = TextEditingController();

    final res = await showDialog<bool>(
      context: context,
      builder: (ctx) => AlertDialog(
        shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(18)),
        title: const Text('Dodaj pregled'),
        content: TextField(
          controller: ctrl,
          decoration: const InputDecoration(
            labelText: 'Naziv pregleda (npr. kontrola kod pedijatra)',
          ),
        ),
        actions: [
          TextButton(
            onPressed: () => Navigator.pop(ctx, false),
            child: const Text('Otkaži'),
          ),
          ElevatedButton(
            onPressed: () => Navigator.pop(ctx, true),
            style: ElevatedButton.styleFrom(
              backgroundColor: AppColors.roseDark,
              foregroundColor: Colors.white,
            ),
            child: const Text('Dodaj'),
          ),
        ],
      ),
    );

    if (res == true && ctrl.text.trim().isNotEmpty) {
      setState(() {
        _checkupDrafts.add(_CheckupDraft(ctrl.text.trim()));
      });
      NestlyToast.success(context, 'Pregled je dodan u listu za ovaj dan.');
    }
  }

  /// ==========================
  /// SPAŠAVANJE
  /// ==========================

  Future<void> _saveEntry() async {
    double? temp;

    if (_tempCtrl.text.trim().isNotEmpty) {
      final normalized = _tempCtrl.text.trim().replaceAll(',', '.');
      final parsed = double.tryParse(normalized);
      if (parsed == null) {
        NestlyToast.info(context, 'Unesite ispravnu temperaturu (npr. 37.2).');
        return;
      }
      temp = parsed;
    }

    if (temp == null && _medDrafts.isEmpty && _checkupDrafts.isEmpty) {
      NestlyToast.info(
        context,
        'Dodajte barem temperaturu, lijek ili pregled da biste sačuvali zapis.',
      );
      return;
    }

    setState(() => _isSaving = true);

    final medicines = _medDrafts.isEmpty
        ? null
        : _medDrafts.map((m) => m.text).join(', ');
    final doctorVisit = _checkupDrafts.isEmpty
        ? null
        : _checkupDrafts.map((c) => c.text).join(', ');

    try {
      await _service.create(
        request: CreateHealthEntryRequest(
          babyId: widget.babyId,
          entryDate: _selectedDay,
          temperatureC: temp,
          medicines: medicines,
          doctorVisit: doctorVisit,
        ),
        token: widget.token,
      );

      if (!mounted) return;

      _tempCtrl.clear();
      _medDrafts.clear();
      _checkupDrafts.clear();

      await _loadEntriesForMonth(_focusedDay);

      NestlyToast.success(
        context,
        'Zapis zdravlja je uspješno sačuvan i vidljiv u listi iznad.',
      );
    } catch (e) {
      if (!mounted) return;
      NestlyToast.error(context, 'Greška pri spremanju zapisa: $e');
    } finally {
      if (mounted) setState(() => _isSaving = false);
    }
  }

  /// ==========================
  /// UI
  /// ==========================

  InputDecoration _fieldDecoration(String label) => InputDecoration(
    labelText: label,
    filled: true,
    fillColor: Colors.white,
    border: OutlineInputBorder(borderRadius: BorderRadius.circular(14)),
    enabledBorder: OutlineInputBorder(
      borderRadius: BorderRadius.circular(14),
      borderSide: BorderSide(color: AppColors.babyBlue.withOpacity(0.35)),
    ),
  );

  @override
  Widget build(BuildContext context) {
    final entriesForDay = _getEntriesForDay(_selectedDay);
    final dateLabel =
        "${_selectedDay.day.toString().padLeft(2, '0')}.${_selectedDay.month.toString().padLeft(2, '0')}.${_selectedDay.year}.";

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
          onPressed: () => Navigator.of(context).pop(),
        ),
        centerTitle: true,
        title: Text(
          "Praćenje zdravlja",
          style: Theme.of(context).textTheme.titleMedium?.copyWith(
            fontWeight: FontWeight.w800,
            color: AppColors.roseDark,
          ),
        ),
      ),
      body: _isLoading
          ? const Center(child: CircularProgressIndicator())
          : Padding(
              padding: const EdgeInsets.fromLTRB(16, 0, 16, 16),
              child: Column(
                children: [
                  _buildCalendar(),
                  const SizedBox(height: 12),
                  Expanded(
                    child: SingleChildScrollView(
                      child: Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: [
                          Text(
                            "Zapisi za $dateLabel",
                            style: Theme.of(context).textTheme.bodyMedium
                                ?.copyWith(
                                  fontWeight: FontWeight.w700,
                                  color: AppColors.roseDark,
                                ),
                          ),
                          const SizedBox(height: 8),
                          _buildEntryList(entriesForDay),
                          const SizedBox(height: 16),
                          _buildFormCard(),
                        ],
                      ),
                    ),
                  ),
                ],
              ),
            ),
    );
  }

  Widget _buildCalendar() {
    return Container(
      decoration: BoxDecoration(
        color: Colors.white,
        borderRadius: BorderRadius.circular(24),
        boxShadow: [
          BoxShadow(
            color: Colors.black12.withOpacity(0.05),
            blurRadius: 8,
            offset: const Offset(0, 3),
          ),
        ],
      ),
      child: TableCalendar<HealthEntry>(
        firstDay: DateTime.utc(2020, 1, 1),
        lastDay: DateTime.utc(2100, 12, 31),
        focusedDay: _focusedDay,
        calendarFormat: CalendarFormat.month,
        startingDayOfWeek: StartingDayOfWeek.monday,
        selectedDayPredicate: (day) => isSameDay(day, _selectedDay),
        eventLoader: _getEntriesForDay,
        headerStyle: HeaderStyle(
          formatButtonVisible: false,
          titleCentered: true,
          titleTextStyle: const TextStyle(
            fontWeight: FontWeight.w700,
            fontSize: 18,
            color: AppColors.roseDark,
          ),
          leftChevronIcon: const Icon(
            Icons.chevron_left_rounded,
            color: AppColors.roseDark,
          ),
          rightChevronIcon: const Icon(
            Icons.chevron_right_rounded,
            color: AppColors.roseDark,
          ),
        ),
        calendarStyle: CalendarStyle(
          todayDecoration: BoxDecoration(
            color: AppColors.babyPink.withOpacity(0.9),
            shape: BoxShape.circle,
          ),
          selectedDecoration: const BoxDecoration(
            color: AppColors.roseDark,
            shape: BoxShape.circle,
          ),
          selectedTextStyle: const TextStyle(color: Colors.white),
          todayTextStyle: const TextStyle(color: Colors.white),
          markerDecoration: const BoxDecoration(
            color: AppColors.roseDark,
            shape: BoxShape.circle,
          ),
          markersMaxCount: 3,
        ),
        daysOfWeekStyle: const DaysOfWeekStyle(
          weekdayStyle: TextStyle(
            fontWeight: FontWeight.w600,
            color: AppColors.textSecondary,
          ),
          weekendStyle: TextStyle(
            fontWeight: FontWeight.w600,
            color: AppColors.textSecondary,
          ),
        ),
        onDaySelected: (selectedDay, focusedDay) {
          setState(() {
            _selectedDay = _dateOnly(selectedDay);
            _focusedDay = focusedDay;
          });
        },
        onPageChanged: (focusedDay) {
          _focusedDay = focusedDay;
          _loadEntriesForMonth(focusedDay);
        },
      ),
    );
  }

  Widget _buildEntryList(List<HealthEntry> entries) {
    if (entries.isEmpty) {
      return const Text(
        "Još nema zapisa za ovaj dan.",
        style: TextStyle(fontSize: 13),
      );
    }

    return Column(children: entries.map(_entryTile).toList());
  }

  Widget _entryTile(HealthEntry e) {
    final tempText = e.temperatureC == null
        ? '-'
        : "${e.temperatureC!.toStringAsFixed(1)} °C";

    return Container(
      margin: const EdgeInsets.only(bottom: 8),
      padding: const EdgeInsets.all(12),
      decoration: BoxDecoration(
        color: Colors.white,
        borderRadius: BorderRadius.circular(16),
        border: Border.all(color: AppColors.babyBlue.withOpacity(0.25)),
      ),
      child: Row(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          const Icon(
            Icons.favorite_border_rounded,
            color: AppColors.roseDark,
            size: 22,
          ),
          const SizedBox(width: 10),
          Expanded(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(
                  tempText,
                  style: const TextStyle(
                    fontWeight: FontWeight.w700,
                    fontSize: 14,
                  ),
                ),
                if (e.medicines != null && e.medicines!.isNotEmpty) ...[
                  const SizedBox(height: 2),
                  Text(
                    "Lijekovi: ${e.medicines}",
                    style: const TextStyle(
                      fontSize: 13,
                      color: AppColors.textSecondary,
                    ),
                  ),
                ],
                if (e.doctorVisit != null && e.doctorVisit!.isNotEmpty) ...[
                  const SizedBox(height: 2),
                  Text(
                    "Pregledi: ${e.doctorVisit}",
                    style: const TextStyle(
                      fontSize: 13,
                      color: AppColors.textSecondary,
                    ),
                  ),
                ],
              ],
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildFormCard() {
    return Container(
      padding: const EdgeInsets.all(16),
      decoration: BoxDecoration(
        color: AppColors.bg.withOpacity(0.9),
        borderRadius: BorderRadius.circular(22),
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Text(
            "Novi zapis zdravlja",
            style: Theme.of(context).textTheme.bodyMedium?.copyWith(
              fontWeight: FontWeight.w700,
              color: AppColors.roseDark,
            ),
          ),
          const SizedBox(height: 6),
          Text(
            "Odaberite dan u kalendaru, zatim dodajte temperaturu, lijekove "
            "i/ili preglede. Nakon što sačuvate, zapis će biti prikazan u listi "
            "iznad za taj datum.",
            style: Theme.of(
              context,
            ).textTheme.bodySmall?.copyWith(color: AppColors.textSecondary),
          ),
          const SizedBox(height: 14),

          // Temperatura
          Row(
            children: [
              Container(
                width: 32,
                height: 32,
                decoration: BoxDecoration(
                  color: AppColors.babyPink.withOpacity(0.8),
                  shape: BoxShape.circle,
                ),
                child: const Icon(
                  Icons.thermostat_rounded,
                  size: 18,
                  color: Colors.white,
                ),
              ),
              const SizedBox(width: 10),
              const Expanded(
                child: Text(
                  "Temperatura",
                  style: TextStyle(
                    fontWeight: FontWeight.w600,
                    color: AppColors.textPrimary,
                  ),
                ),
              ),
              SizedBox(
                width: 90,
                child: TextField(
                  controller: _tempCtrl,
                  keyboardType: const TextInputType.numberWithOptions(
                    decimal: true,
                  ),
                  decoration: InputDecoration(
                    isDense: true,
                    contentPadding: const EdgeInsets.symmetric(
                      horizontal: 8,
                      vertical: 8,
                    ),
                    suffixText: "°C",
                    border: OutlineInputBorder(
                      borderRadius: BorderRadius.circular(12),
                    ),
                  ),
                ),
              ),
            ],
          ),
          const SizedBox(height: 10),

          // Lijekovi
          Row(
            children: [
              Container(
                width: 32,
                height: 32,
                decoration: BoxDecoration(
                  color: AppColors.roseDark.withOpacity(0.12),
                  shape: BoxShape.circle,
                ),
                child: const Icon(
                  Icons.medical_services_rounded,
                  size: 18,
                  color: AppColors.roseDark,
                ),
              ),
              const SizedBox(width: 10),
              const Expanded(
                child: Text(
                  "Lijekovi",
                  style: TextStyle(
                    fontWeight: FontWeight.w600,
                    color: AppColors.textPrimary,
                  ),
                ),
              ),
              TextButton(
                onPressed: _addMedicationDraft,
                style: TextButton.styleFrom(
                  backgroundColor: AppColors.roseDark,
                  foregroundColor: Colors.white,
                  padding: const EdgeInsets.symmetric(
                    horizontal: 16,
                    vertical: 8,
                  ),
                  shape: RoundedRectangleBorder(
                    borderRadius: BorderRadius.circular(16),
                  ),
                ),
                child: const Text("Dodaj"),
              ),
            ],
          ),
          if (_medDrafts.isNotEmpty) ...[
            const SizedBox(height: 6),
            Wrap(
              spacing: 6,
              runSpacing: 4,
              children: _medDrafts
                  .map(
                    (m) => Chip(
                      label: Text(m.text),
                      backgroundColor: Colors.white,
                      deleteIcon: const Icon(Icons.close_rounded, size: 16),
                      onDeleted: () {
                        setState(() => _medDrafts.remove(m));
                      },
                    ),
                  )
                  .toList(),
            ),
          ],
          const SizedBox(height: 10),

          // Pregledi
          Row(
            children: [
              Container(
                width: 32,
                height: 32,
                decoration: BoxDecoration(
                  color: AppColors.babyBlue.withOpacity(0.18),
                  shape: BoxShape.circle,
                ),
                child: const Icon(
                  Icons.vaccines_rounded,
                  size: 18,
                  color: AppColors.babyBlue,
                ),
              ),
              const SizedBox(width: 10),
              const Expanded(
                child: Text(
                  "Pregledi",
                  style: TextStyle(
                    fontWeight: FontWeight.w600,
                    color: AppColors.textPrimary,
                  ),
                ),
              ),
              TextButton(
                onPressed: _addCheckupDraft,
                style: TextButton.styleFrom(
                  backgroundColor: AppColors.roseDark,
                  foregroundColor: Colors.white,
                  padding: const EdgeInsets.symmetric(
                    horizontal: 16,
                    vertical: 8,
                  ),
                  shape: RoundedRectangleBorder(
                    borderRadius: BorderRadius.circular(16),
                  ),
                ),
                child: const Text("Dodaj"),
              ),
            ],
          ),
          if (_checkupDrafts.isNotEmpty) ...[
            const SizedBox(height: 6),
            Wrap(
              spacing: 6,
              runSpacing: 4,
              children: _checkupDrafts
                  .map(
                    (c) => Chip(
                      label: Text(c.text),
                      backgroundColor: Colors.white,
                      deleteIcon: const Icon(Icons.close_rounded, size: 16),
                      onDeleted: () {
                        setState(() => _checkupDrafts.remove(c));
                      },
                    ),
                  )
                  .toList(),
            ),
          ],
          const SizedBox(height: 18),

          SizedBox(
            width: double.infinity,
            child: ElevatedButton(
              onPressed: _isSaving ? null : _saveEntry,
              style: ElevatedButton.styleFrom(
                backgroundColor: AppColors.roseDark,
                foregroundColor: Colors.white,
                padding: const EdgeInsets.symmetric(vertical: 14),
                shape: RoundedRectangleBorder(
                  borderRadius: BorderRadius.circular(16),
                ),
              ),
              child: _isSaving
                  ? const SizedBox(
                      height: 18,
                      width: 18,
                      child: CircularProgressIndicator(
                        strokeWidth: 2,
                        valueColor: AlwaysStoppedAnimation<Color>(Colors.white),
                      ),
                    )
                  : const Text(
                      "Sačuvaj",
                      style: TextStyle(
                        fontWeight: FontWeight.w700,
                        fontSize: 16,
                      ),
                    ),
            ),
          ),
        ],
      ),
    );
  }
}
