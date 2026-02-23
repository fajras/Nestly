import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:flutter_application_nestly/network/api_client.dart';
import 'package:flutter_application_nestly/layouts/nestly_calendar.dart';
import 'package:flutter_application_nestly/layouts/nestly_toast.dart';
import 'package:flutter_application_nestly/main.dart';

class HealthEntry {
  final int id;
  final int babyId;
  final DateTime entryDate;
  final double? temperatureC;
  final String? medicines;
  final String? doctorVisit;

  const HealthEntry({
    required this.id,
    required this.babyId,
    required this.entryDate,
    this.temperatureC,
    this.medicines,
    this.doctorVisit,
  });

  factory HealthEntry.fromJson(Map<String, dynamic> json) {
    return HealthEntry(
      id: json['id'],
      babyId: json['babyId'],
      entryDate: DateTime.parse(json['entryDate']),
      temperatureC: json['temperatureC'] == null
          ? null
          : (json['temperatureC'] as num).toDouble(),
      medicines: json['medicines'],
      doctorVisit: json['doctorVisit'],
    );
  }
}

class CreateHealthEntryRequest {
  final int babyId;
  final DateTime entryDate;
  final double? temperatureC;
  final String? medicines;
  final String? doctorVisit;

  const CreateHealthEntryRequest({
    required this.babyId,
    required this.entryDate,
    this.temperatureC,
    this.medicines,
    this.doctorVisit,
  });

  Map<String, dynamic> toJson() => {
    'babyId': babyId,
    'entryDate': entryDate.toIso8601String(),
    if (temperatureC != null) 'temperatureC': temperatureC,
    if (medicines != null) 'medicines': medicines,
    if (doctorVisit != null) 'doctorVisit': doctorVisit,
  };
}

class HealthEntryApiService {
  Future<List<HealthEntry>> getForBabyInRange({
    required int babyId,
    required DateTime from,
    required DateTime to,
  }) async {
    final res = await ApiClient.get(
      '/api/HealthEntry'
      '?BabyId=$babyId'
      '&DateFrom=${from.toIso8601String()}'
      '&DateTo=${to.toIso8601String()}',
    );

    if (res.statusCode != 200) {
      throw Exception('Failed to load health entries');
    }

    final List data = jsonDecode(res.body);
    return data.map((e) => HealthEntry.fromJson(e)).toList();
  }

  Future<void> patch(int id, Map<String, dynamic> body) async {
    final res = await ApiClient.patch('/api/HealthEntry/$id', body: body);

    if (res.statusCode != 200) {
      throw Exception('Failed to update');
    }
  }

  Future<void> delete(int id) async {
    final res = await ApiClient.delete('/api/HealthEntry/$id');

    if (res.statusCode != 204) {
      throw Exception('Failed to delete');
    }
  }

  Future<void> create({required CreateHealthEntryRequest request}) async {
    final res = await ApiClient.post(
      '/api/HealthEntry',
      body: request.toJson(),
    );

    if (res.statusCode != 200 && res.statusCode != 201) {
      throw Exception('Failed to save health entry');
    }
  }
}

class HealthTrackingScreen extends StatefulWidget {
  final int babyId;
  final String babyName;
  final int userId;

  const HealthTrackingScreen({
    super.key,
    required this.babyId,
    required this.babyName,
    required this.userId,
  });

  @override
  State<HealthTrackingScreen> createState() => _HealthTrackingScreenState();
}

class _HealthTrackingScreenState extends State<HealthTrackingScreen> {
  final _service = HealthEntryApiService();

  DateTime _focusedDay = DateTime.now();
  DateTime? _selectedDay;
  HealthEntry? _editingEntry;
  bool _loading = true;
  bool _saving = false;

  final Map<DateTime, List<HealthEntry>> _entriesByDay = {};

  final _tempCtrl = TextEditingController();
  final _medCtrl = TextEditingController();
  final _checkCtrl = TextEditingController();

  final _tempFocus = FocusNode();

  @override
  void initState() {
    super.initState();
    _focusedDay = _dayOnly(DateTime.now());
    _selectedDay = _focusedDay;
    _loadMonth(_focusedDay);
  }

  @override
  void dispose() {
    _tempCtrl.dispose();
    _medCtrl.dispose();
    _checkCtrl.dispose();
    _tempFocus.dispose();
    super.dispose();
  }

  DateTime _dayOnly(DateTime d) => DateTime(d.year, d.month, d.day);

  String _fmt(DateTime d) {
    return '${d.day.toString().padLeft(2, '0')}.'
        '${d.month.toString().padLeft(2, '0')}.'
        '${d.year}.';
  }

  Future<void> _deleteEntry(int id) async {
    final confirm = await showDialog<bool>(
      context: context,
      builder: (_) => AlertDialog(
        title: const Text('Obrisati zapis?'),
        content: const Text('Da li ste sigurni?'),
        actions: [
          TextButton(
            onPressed: () => Navigator.pop(context, false),
            child: const Text('Odustani'),
          ),
          TextButton(
            onPressed: () => Navigator.pop(context, true),
            child: const Text('Obriši', style: TextStyle(color: Colors.red)),
          ),
        ],
      ),
    );

    if (confirm != true) return;

    try {
      await _service.delete(id);
      await _loadMonth(_focusedDay);
      NestlyToast.success(context, 'Zapis obrisan.');
    } catch (_) {
      NestlyToast.error(context, 'Greška pri brisanju.');
    }
  }

  List<Widget> _buildEntriesForSelectedDay() {
    if (_selectedDay == null) return [];

    final entries = _forDay(_selectedDay!);

    if (entries.isEmpty) {
      return [
        const Text(
          'Nema zapisa za ovaj dan.',
          style: TextStyle(color: AppColors.textSecondary),
        ),
      ];
    }

    return entries.map((e) {
      return Container(
        margin: const EdgeInsets.only(bottom: 12),
        padding: const EdgeInsets.all(AppSpacing.lg),
        decoration: BoxDecoration(
          color: Colors.white,
          borderRadius: BorderRadius.circular(AppRadius.lg),
          boxShadow: [
            BoxShadow(
              color: Colors.black.withOpacity(.05),
              blurRadius: 8,
              offset: const Offset(0, 3),
            ),
          ],
        ),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Row(
              children: [
                Expanded(
                  child: Text(
                    _fmt(e.entryDate),
                    style: const TextStyle(
                      fontWeight: FontWeight.w700,
                      color: AppColors.roseDark,
                    ),
                  ),
                ),
                IconButton(
                  icon: const Icon(Icons.edit, color: AppColors.roseDark),
                  onPressed: () {
                    setState(() {
                      _editingEntry = e;
                      _selectedDay = _dayOnly(e.entryDate);
                      _tempCtrl.text = e.temperatureC?.toString() ?? '';
                      _medCtrl.text = e.medicines ?? '';
                      _checkCtrl.text = e.doctorVisit ?? '';
                    });
                  },
                ),
                IconButton(
                  icon: const Icon(Icons.delete, color: Colors.red),
                  onPressed: () => _deleteEntry(e.id),
                ),
              ],
            ),
            if (e.temperatureC != null)
              Text('Temperatura: ${e.temperatureC} °C'),
            if (e.medicines != null) Text('Lijekovi: ${e.medicines}'),
            if (e.doctorVisit != null) Text('Pregled: ${e.doctorVisit}'),
          ],
        ),
      );
    }).toList();
  }

  DateTime _monthStart(DateTime d) => DateTime(d.year, d.month, 1);
  DateTime _monthEnd(DateTime d) =>
      DateTime(d.year, d.month + 1, 0, 23, 59, 59);

  Future<void> _loadMonth(DateTime month) async {
    setState(() => _loading = true);

    try {
      final list = await _service.getForBabyInRange(
        babyId: widget.babyId,
        from: _monthStart(month),
        to: _monthEnd(month),
      );

      _entriesByDay.clear();
      for (final e in list) {
        final key = _dayOnly(e.entryDate);
        _entriesByDay.putIfAbsent(key, () => []).add(e);
      }
    } catch (_) {
      NestlyToast.error(context, 'Greška pri učitavanju');
    }

    if (mounted) setState(() => _loading = false);
  }

  List<HealthEntry> _forDay(DateTime day) =>
      _entriesByDay[_dayOnly(day)] ?? const [];
  Future<void> _save() async {
    if (_selectedDay == null) {
      NestlyToast.info(context, 'Odaberite dan u kalendaru.');
      return;
    }

    double? temp;

    if (_tempCtrl.text.isNotEmpty) {
      temp = double.tryParse(_tempCtrl.text.replaceAll(',', '.'));
      if (temp == null) {
        NestlyToast.info(context, 'Temperatura nije validna.');
        return;
      }
    }

    if (temp != null && (temp < 30 || temp > 45)) {
      NestlyToast.info(context, 'Temperatura nije realna.');
      return;
    }

    if (temp == null &&
        _medCtrl.text.trim().isEmpty &&
        _checkCtrl.text.trim().isEmpty) {
      NestlyToast.info(context, 'Unesite barem jedan podatak.');
      return;
    }

    setState(() => _saving = true);

    try {
      if (_editingEntry == null) {
        // CREATE
        await _service.create(
          request: CreateHealthEntryRequest(
            babyId: widget.babyId,
            entryDate: _selectedDay!,
            temperatureC: temp,
            medicines: _medCtrl.text.trim().isEmpty
                ? null
                : _medCtrl.text.trim(),
            doctorVisit: _checkCtrl.text.trim().isEmpty
                ? null
                : _checkCtrl.text.trim(),
          ),
        );

        NestlyToast.success(context, 'Zapis sačuvan.');
      } else {
        final patchBody = <String, dynamic>{};

        if (temp != null) patchBody['temperatureC'] = temp;
        if (_medCtrl.text.trim().isNotEmpty) {
          patchBody['medicines'] = _medCtrl.text.trim();
        }
        if (_checkCtrl.text.trim().isNotEmpty) {
          patchBody['doctorVisit'] = _checkCtrl.text.trim();
        }

        await _service.patch(_editingEntry!.id, patchBody);

        NestlyToast.success(context, 'Zapis ažuriran.');
      }

      _tempCtrl.clear();
      _medCtrl.clear();
      _checkCtrl.clear();
      _editingEntry = null;

      await _loadMonth(_focusedDay);
    } catch (_) {
      NestlyToast.error(context, 'Greška pri spremanju.');
    } finally {
      if (mounted) setState(() => _saving = false);
    }
  }

  InputDecoration _fieldDecoration({
    required String label,
    required IconData icon,
    String? hint,
  }) {
    return InputDecoration(
      labelText: label,
      hintText: hint,
      prefixIcon: Icon(icon),
      filled: true,
      fillColor: AppColors.babyPink.withOpacity(.15),

      border: OutlineInputBorder(
        borderRadius: BorderRadius.circular(AppRadius.lg),
        borderSide: BorderSide.none,
      ),

      enabledBorder: OutlineInputBorder(
        borderRadius: BorderRadius.circular(AppRadius.lg),
        borderSide: BorderSide.none,
      ),

      focusedBorder: OutlineInputBorder(
        borderRadius: BorderRadius.circular(AppRadius.lg),
        borderSide: const BorderSide(color: AppColors.roseDark, width: 1.6),
      ),

      prefixIconColor: AppColors.roseDark,

      floatingLabelStyle: const TextStyle(
        color: AppColors.roseDark,
        fontWeight: FontWeight.w600,
      ),
    );
  }

  Widget _form() {
    return Card(
      shape: RoundedRectangleBorder(
        borderRadius: BorderRadius.circular(AppRadius.xl),
      ),
      child: Padding(
        padding: const EdgeInsets.all(AppSpacing.lg),
        child: Column(
          mainAxisSize: MainAxisSize.min,
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Text(
              _editingEntry == null
                  ? 'Novi zapis zdravlja'
                  : 'Uređivanje zapisa zdravlja',
              style: Theme.of(context).textTheme.bodyLarge?.copyWith(
                fontWeight: FontWeight.w700,
                color: AppColors.roseDark,
              ),
            ),

            const SizedBox(height: AppSpacing.md),

            TextField(
              controller: _tempCtrl,
              cursorColor: AppColors.roseDark,
              keyboardType: const TextInputType.numberWithOptions(
                decimal: true,
              ),
              decoration: _fieldDecoration(
                label: 'Temperatura',
                icon: Icons.thermostat_rounded,
                hint: '36.8 °C',
              ),
            ),

            const SizedBox(height: AppSpacing.md),

            TextField(
              controller: _medCtrl,
              cursorColor: AppColors.roseDark,
              decoration: _fieldDecoration(
                label: 'Lijekovi',
                icon: Icons.medical_services_rounded,
                hint: 'npr. Paracetamol 5 ml',
              ),
            ),

            const SizedBox(height: AppSpacing.md),

            TextField(
              controller: _checkCtrl,
              cursorColor: AppColors.roseDark,
              decoration: _fieldDecoration(
                label: 'Pregledi',
                icon: Icons.vaccines_rounded,
                hint: 'npr. Kontrola kod pedijatra',
              ),
            ),

            const SizedBox(height: AppSpacing.lg),

            SizedBox(
              width: double.infinity,
              child: ElevatedButton(
                onPressed: _saving ? null : _save,
                style: ElevatedButton.styleFrom(
                  backgroundColor: AppColors.roseDark,
                  foregroundColor: Colors.white,
                  padding: const EdgeInsets.symmetric(vertical: 14),
                  shape: RoundedRectangleBorder(
                    borderRadius: BorderRadius.circular(AppRadius.lg),
                  ),
                ),
                child: _saving
                    ? const SizedBox(
                        width: 18,
                        height: 18,
                        child: CircularProgressIndicator(
                          strokeWidth: 2,
                          valueColor: AlwaysStoppedAnimation<Color>(
                            AppColors.roseDark,
                          ),
                        ),
                      )
                    : Text(
                        _editingEntry == null ? 'Sačuvaj' : 'Sačuvaj izmjene',
                        style: TextStyle(
                          fontWeight: FontWeight.w700,
                          fontSize: 16,
                        ),
                      ),
              ),
            ),
            if (_editingEntry != null) ...[
              const SizedBox(height: 10),
              SizedBox(
                width: double.infinity,
                child: OutlinedButton(
                  onPressed: () {
                    setState(() {
                      _editingEntry = null;
                      _tempCtrl.clear();
                      _medCtrl.clear();
                      _checkCtrl.clear();
                    });
                  },
                  style: OutlinedButton.styleFrom(
                    foregroundColor: AppColors.roseDark,
                    side: const BorderSide(color: AppColors.roseDark),
                  ),
                  child: const Text(
                    'Odustani uređivanje',
                    style: TextStyle(fontWeight: FontWeight.w600),
                  ),
                ),
              ),
            ],
          ],
        ),
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppColors.bg,
      appBar: AppBar(
        backgroundColor: Colors.transparent,
        elevation: 0,
        iconTheme: const IconThemeData(color: AppColors.roseDark),
        title: Text(
          'Praćenje zdravlja',
          style: Theme.of(context).textTheme.titleLarge?.copyWith(
            fontWeight: FontWeight.w700,
            color: AppColors.roseDark,
          ),
        ),
        centerTitle: true,
      ),
      body: _loading
          ? const Center(
              child: CircularProgressIndicator(
                valueColor: AlwaysStoppedAnimation<Color>(AppColors.roseDark),
              ),
            )
          : Column(
              children: [
                NestlyCalendar(
                  focusedDay: _focusedDay,
                  selectedDay: _selectedDay,
                  accentColor: AppColors.roseDark,
                  markerIcon: Icons.favorite_rounded,
                  eventLoader: (day) => _forDay(day),
                  onDaySelected: (selected, focused) {
                    setState(() {
                      _selectedDay = _dayOnly(selected);
                      _focusedDay = _dayOnly(focused);
                    });
                  },
                ),
                const SizedBox(height: AppSpacing.lg),
                Expanded(
                  child: ListView(
                    padding: const EdgeInsets.symmetric(
                      horizontal: AppSpacing.lg,
                    ),
                    children: [
                      _form(),
                      const SizedBox(height: AppSpacing.lg),
                      ..._buildEntriesForSelectedDay(),
                    ],
                  ),
                ),
              ],
            ),
    );
  }
}
