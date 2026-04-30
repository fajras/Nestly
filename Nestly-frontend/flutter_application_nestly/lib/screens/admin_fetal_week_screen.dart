import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:flutter_application_nestly/network/api_client.dart';
import 'package:flutter_application_nestly/layouts/nestly_toast.dart';
import 'package:flutter_application_nestly/main.dart';

class FetalWeekRow {
  final int id;
  final String babyDevelopment;
  final String motherChanges;

  FetalWeekRow({
    required this.id,
    required this.babyDevelopment,
    required this.motherChanges,
  });

  factory FetalWeekRow.fromJson(Map<String, dynamic> json) {
    return FetalWeekRow(
      id: json['id'],
      babyDevelopment: json['babyDevelopment'] ?? '',
      motherChanges: json['motherChanges'] ?? '',
    );
  }
}

class FetalWeekService {
  Future<List<FetalWeekRow>> getAll() async {
    int page = 1;
    const pageSize = 100;

    List<FetalWeekRow> result = [];

    while (true) {
      final res = await ApiClient.get(
        '/api/FetalDevelopmentWeek?page=$page&pageSize=$pageSize',
      );

      if (res.statusCode != 200) {
        throw Exception('Greška pri učitavanju');
      }

      final data = jsonDecode(res.body);
      final List items = data['items'] ?? [];

      if (items.isEmpty) break;

      result.addAll(items.map((e) => FetalWeekRow.fromJson(e)).toList());

      if (items.length < pageSize) break;
      page++;
    }

    return result;
  }

  Future<void> patch(int id, Map<String, dynamic> body) async {
    final res = await ApiClient.patch(
      '/api/FetalDevelopmentWeek/$id',
      body: body,
    );

    if (res.statusCode != 200) {
      throw Exception('Update failed');
    }
  }
}

class AdminFetalDevelopmentScreen extends StatefulWidget {
  const AdminFetalDevelopmentScreen({super.key});

  @override
  State<AdminFetalDevelopmentScreen> createState() =>
      _AdminFetalDevelopmentScreenState();
}

class _AdminFetalDevelopmentScreenState
    extends State<AdminFetalDevelopmentScreen> {
  final _service = FetalWeekService();

  List<FetalWeekRow> _data = [];
  bool _loading = true;

  @override
  void initState() {
    super.initState();
    _load();
  }

  Future<void> _load() async {
    setState(() => _loading = true);

    try {
      final res = await _service.getAll();
      if (!mounted) return;
      setState(() => _data = res);
    } catch (_) {
      NestlyToast.error(context, 'Greška pri učitavanju');
    } finally {
      if (mounted) setState(() => _loading = false);
    }
  }

  void _openEdit(FetalWeekRow item) {
    showModalBottomSheet(
      context: context,
      isScrollControlled: true,
      backgroundColor: Colors.transparent,
      builder: (_) => _EditSheet(item: item, onSaved: _load),
    );
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
          'Razvoj fetusa',
          style: TextStyle(fontSize: 26, fontWeight: FontWeight.w700),
        ),
        const SizedBox(height: AppSpacing.lg),

        Expanded(
          child: ListView.separated(
            itemCount: _data.length,
            separatorBuilder: (_, __) => const SizedBox(height: 12),
            itemBuilder: (_, i) {
              final w = _data[i];

              return Card(
                child: ListTile(
                  title: Text('Sedmica ${w.id}'),
                  subtitle: Text(w.babyDevelopment),
                  onTap: () => _openEdit(w),
                ),
              );
            },
          ),
        ),
      ],
    );
  }
}

class _EditSheet extends StatefulWidget {
  final FetalWeekRow item;
  final VoidCallback onSaved;

  const _EditSheet({required this.item, required this.onSaved});

  @override
  State<_EditSheet> createState() => _EditSheetState();
}

class _EditSheetState extends State<_EditSheet> {
  final _baby = TextEditingController();
  final _mother = TextEditingController();
  final _image = TextEditingController();

  final _service = FetalWeekService();

  bool _saving = false;

  @override
  void initState() {
    super.initState();

    _baby.text = widget.item.babyDevelopment;
    _mother.text = widget.item.motherChanges;
  }

  Future<void> _save() async {
    final baby = _baby.text.trim();
    final mother = _mother.text.trim();
    final image = _image.text.trim();

    if (baby.isEmpty || mother.isEmpty) {
      NestlyToast.error(context, 'Sva polja moraju biti popunjena');
      return;
    }

    setState(() => _saving = true);

    try {
      await _service.patch(widget.item.id, {
        'babyDevelopment': baby,
        'motherChanges': mother,
        'imageUrl': image,
      });

      if (!mounted) return;

      widget.onSaved();
      Navigator.pop(context);

      NestlyToast.success(
        Navigator.of(context, rootNavigator: true).context,
        'Uspješno ažurirano',
        accentColor: AppColors.seed,
      );
    } catch (_) {
      NestlyToast.error(
        Navigator.of(context, rootNavigator: true).context,
        'Greška pri spremanju',
      );
    } finally {
      if (mounted) setState(() => _saving = false);
    }
  }

  @override
  Widget build(BuildContext context) {
    return DraggableScrollableSheet(
      initialChildSize: 0.9,
      builder: (_, controller) {
        return Container(
          padding: const EdgeInsets.all(AppSpacing.lg),
          decoration: const BoxDecoration(
            color: Colors.white,
            borderRadius: BorderRadius.vertical(top: Radius.circular(28)),
          ),
          child: SingleChildScrollView(
            controller: controller,
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                TextField(
                  controller: _baby,
                  maxLines: 5,
                  decoration: const InputDecoration(labelText: 'Razvoj bebe'),
                ),

                const SizedBox(height: 16),

                TextField(
                  controller: _mother,
                  maxLines: 5,
                  decoration: const InputDecoration(
                    labelText: 'Promjene kod majke',
                  ),
                ),

                const SizedBox(height: 24),

                Row(
                  children: [
                    Expanded(
                      child: OutlinedButton(
                        onPressed: () => Navigator.pop(context),
                        child: const Text('Otkaži'),
                      ),
                    ),
                    const SizedBox(width: 12),
                    Expanded(
                      child: ElevatedButton(
                        onPressed: _saving ? null : _save,
                        child: _saving
                            ? const CircularProgressIndicator(strokeWidth: 2)
                            : const Text('Spremi'),
                      ),
                    ),
                  ],
                ),
              ],
            ),
          ),
        );
      },
    );
  }
}
