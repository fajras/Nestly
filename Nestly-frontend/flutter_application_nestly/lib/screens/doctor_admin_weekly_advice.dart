import 'dart:convert';
import 'dart:io' show Platform;

import 'package:flutter/foundation.dart' show kIsWeb;
import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;

import 'package:flutter_application_nestly/layouts/nestly_toast.dart';
import 'package:flutter_application_nestly/main.dart';

/// =======================
/// API BASE
/// =======================

String _devBase() {
  if (kIsWeb) return 'http://localhost:5167';
  if (Platform.isAndroid) return 'http://10.0.2.2:5167';
  return 'http://localhost:5167';
}

String get _apiBase =>
    const String.fromEnvironment('API_BASE', defaultValue: '').isNotEmpty
    ? const String.fromEnvironment('API_BASE')
    : _devBase();

Map<String, String> _headers(String token) => {
  'Content-Type': 'application/json',
  'Accept': 'application/json',
  'Authorization': 'Bearer $token',
};

/// =======================
/// MODEL
/// =======================

class WeeklyAdviceRow {
  final int id;
  final int weekNumber;
  final String adviceText;

  WeeklyAdviceRow({
    required this.id,
    required this.weekNumber,
    required this.adviceText,
  });

  factory WeeklyAdviceRow.fromJson(Map<String, dynamic> json) {
    return WeeklyAdviceRow(
      id: json['id'],
      weekNumber: json['weekNumber'],
      adviceText: json['adviceText'],
    );
  }
}

/// =======================
/// SERVICE
/// =======================

class WeeklyAdviceAdminService {
  Future<List<WeeklyAdviceRow>> getAll(String token) async {
    final res = await http.get(
      Uri.parse('$_apiBase/api/weeklyadvice'),
      headers: _headers(token),
    );

    if (res.statusCode != 200) {
      throw Exception('Failed to load weekly advice');
    }

    final List data = jsonDecode(res.body);
    return data.map((e) => WeeklyAdviceRow.fromJson(e)).toList();
  }

  Future<void> updateAdvice({
    required String token,
    required int id,
    required String adviceText,
  }) async {
    final res = await http.patch(
      Uri.parse('$_apiBase/api/weeklyadvice/$id'),
      headers: _headers(token),
      body: jsonEncode({'adviceText': adviceText}),
    );

    if (res.statusCode != 200) {
      throw Exception('Failed to update advice');
    }
  }
}

/// =======================
/// SCREEN
/// =======================

class DoctorAdminWeeklyAdviceScreen extends StatefulWidget {
  final String token;
  const DoctorAdminWeeklyAdviceScreen({super.key, required this.token});

  @override
  State<DoctorAdminWeeklyAdviceScreen> createState() =>
      _DoctorAdminWeeklyAdviceScreenState();
}

class _DoctorAdminWeeklyAdviceScreenState
    extends State<DoctorAdminWeeklyAdviceScreen> {
  final _service = WeeklyAdviceAdminService();
  final _searchController = TextEditingController();

  List<WeeklyAdviceRow> _all = [];
  List<WeeklyAdviceRow> _filtered = [];
  bool _loading = true;

  @override
  void initState() {
    super.initState();
    _load();
  }

  Future<void> _load() async {
    setState(() => _loading = true);

    try {
      final data = await _service.getAll(widget.token);
      data.sort((a, b) => a.weekNumber.compareTo(b.weekNumber));

      setState(() {
        _all = data;
        _filtered = data;
        _searchController.clear();
      });
    } catch (e) {
      NestlyToast.error(context, 'Greška pri učitavanju savjeta');
    } finally {
      if (mounted) setState(() => _loading = false);
    }
  }

  void _onSearch(String value) {
    final q = value.toLowerCase();

    setState(() {
      _filtered = _all.where((x) {
        return x.adviceText.toLowerCase().contains(q) ||
            x.weekNumber.toString().contains(q);
      }).toList();
    });
  }

  @override
  Widget build(BuildContext context) {
    if (_loading) {
      return const Center(child: CircularProgressIndicator());
    }

    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        const Text(
          'Sedmični savjeti',
          style: TextStyle(fontSize: 26, fontWeight: FontWeight.w800),
        ),

        const SizedBox(height: AppSpacing.xl),

        TextField(
          controller: _searchController,
          onChanged: _onSearch,
          decoration: const InputDecoration(
            hintText: 'Pretraga po sedmici ili tekstu',
            prefixIcon: Icon(Icons.search),
          ),
        ),

        const SizedBox(height: AppSpacing.lg),

        Expanded(
          child: _filtered.isEmpty
              ? const Center(
                  child: Text(
                    'Nema savjeta',
                    style: TextStyle(
                      color: AppColors.textSecondary,
                      fontSize: 16,
                    ),
                  ),
                )
              : ListView.separated(
                  itemCount: _filtered.length,
                  separatorBuilder: (_, __) =>
                      const SizedBox(height: AppSpacing.md),
                  itemBuilder: (context, i) {
                    return _WeeklyAdviceCard(
                      advice: _filtered[i],
                      token: widget.token,
                      onSaved: _load,
                    );
                  },
                ),
        ),
      ],
    );
  }
}

/// =======================
/// CARD
/// =======================

class _WeeklyAdviceCard extends StatefulWidget {
  final WeeklyAdviceRow advice;
  final String token;
  final VoidCallback onSaved;

  const _WeeklyAdviceCard({
    required this.advice,
    required this.token,
    required this.onSaved,
  });

  @override
  State<_WeeklyAdviceCard> createState() => _WeeklyAdviceCardState();
}

class _WeeklyAdviceCardState extends State<_WeeklyAdviceCard> {
  bool _editing = false;
  bool _saving = false;
  late TextEditingController _controller;
  final _service = WeeklyAdviceAdminService();

  @override
  void initState() {
    super.initState();
    _controller = TextEditingController(text: widget.advice.adviceText);
  }

  Future<void> _save() async {
    if (_controller.text.trim().isEmpty) return;

    setState(() => _saving = true);

    try {
      await _service.updateAdvice(
        token: widget.token,
        id: widget.advice.id,
        adviceText: _controller.text,
      );

      NestlyToast.success(context, 'Savjet spremljen');
      widget.onSaved();
      setState(() => _editing = false);
    } catch (e) {
      NestlyToast.error(context, 'Greška pri spremanju');
    } finally {
      if (mounted) setState(() => _saving = false);
    }
  }

  @override
  Widget build(BuildContext context) {
    return Card(
      child: Padding(
        padding: const EdgeInsets.all(AppSpacing.lg),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Text(
              'Sedmica ${widget.advice.weekNumber}',
              style: const TextStyle(fontSize: 18, fontWeight: FontWeight.w800),
            ),

            const SizedBox(height: AppSpacing.md),

            if (_editing)
              TextField(
                controller: _controller,
                maxLines: 4,
                decoration: const InputDecoration(hintText: 'Unesite savjet'),
              )
            else
              Text(
                widget.advice.adviceText,
                style: const TextStyle(fontSize: 15),
              ),

            const SizedBox(height: AppSpacing.md),

            Row(
              mainAxisAlignment: MainAxisAlignment.end,
              children: [
                if (_editing) ...[
                  ElevatedButton(
                    onPressed: _saving ? null : _save,
                    child: _saving
                        ? const SizedBox(
                            width: 16,
                            height: 16,
                            child: CircularProgressIndicator(strokeWidth: 2),
                          )
                        : const Text('Spremi'),
                  ),
                  const SizedBox(width: AppSpacing.sm),
                  TextButton(
                    onPressed: () => setState(() => _editing = false),
                    child: const Text('Otkaži'),
                  ),
                ] else
                  TextButton.icon(
                    onPressed: () => setState(() => _editing = true),
                    icon: const Icon(Icons.edit),
                    label: const Text('Uredi'),
                  ),
              ],
            ),
          ],
        ),
      ),
    );
  }
}
