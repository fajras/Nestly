import 'dart:convert';
import 'package:flutter_application_nestly/network/api_client.dart';
import 'package:flutter/material.dart';
import 'package:flutter_application_nestly/layouts/nestly_toast.dart';
import 'package:flutter_application_nestly/main.dart';

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

class WeeklyAdviceAdminService {
  Future<List<WeeklyAdviceRow>> getAll() async {
    final res = await ApiClient.get('/api/weeklyadvice');

    if (res.statusCode != 200) {
      throw Exception('Failed to load weekly advice');
    }

    final List data = jsonDecode(res.body);
    return data.map((e) => WeeklyAdviceRow.fromJson(e)).toList();
  }

  Future<void> updateAdvice({
    required int id,
    required String adviceText,
  }) async {
    final res = await ApiClient.patch(
      '/api/weeklyadvice/$id',
      body: {'adviceText': adviceText},
    );

    if (res.statusCode != 200) {
      throw Exception('Failed to update advice');
    }
  }
}

class DoctorAdminWeeklyAdviceScreen extends StatefulWidget {
  const DoctorAdminWeeklyAdviceScreen({super.key});

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
      final data = await _service.getAll();
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
          style: TextStyle(fontSize: 26, fontWeight: FontWeight.w700),
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
                      onSaved: _load,
                    );
                  },
                ),
        ),
      ],
    );
  }
}

class _WeeklyAdviceCard extends StatefulWidget {
  final WeeklyAdviceRow advice;
  final VoidCallback onSaved;

  const _WeeklyAdviceCard({required this.advice, required this.onSaved});

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
    final text = _controller.text.trim();

    if (text.isEmpty) {
      NestlyToast.error(context, 'Savjet ne može biti prazan');
      return;
    }

    if (text.length < 10) {
      NestlyToast.error(context, 'Savjet je prekratak');
      return;
    }

    setState(() => _saving = true);

    try {
      await _service.updateAdvice(id: widget.advice.id, adviceText: text);

      NestlyToast.success(
        context,
        'Savjet spremljen',
        accentColor: AppColors.seed,
      );

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
              style: const TextStyle(fontSize: 18, fontWeight: FontWeight.w700),
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
