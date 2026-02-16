import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:flutter_application_nestly/network/api_client.dart';
import 'package:flutter_application_nestly/main.dart';
import 'package:flutter_application_nestly/layouts/nestly_toast.dart';

String _formatDate(DateTime d) =>
    '${d.day.toString().padLeft(2, '0')}.${d.month.toString().padLeft(2, '0')}.${d.year}.';

class MilestoneEntry {
  final int id;
  final int babyId;
  final String title;
  final DateTime achievedDate;
  final String? notes;

  const MilestoneEntry({
    required this.id,
    required this.babyId,
    required this.title,
    required this.achievedDate,
    this.notes,
  });

  factory MilestoneEntry.fromJson(Map<String, dynamic> json) {
    return MilestoneEntry(
      id: json['id'],
      babyId: json['babyId'],
      title: json['title'],
      achievedDate: DateTime.parse(json['achievedDate']),
      notes: json['notes'],
    );
  }
}

class CreateMilestoneRequest {
  final int babyId;
  final String title;
  final DateTime achievedDate;
  final String? notes;

  const CreateMilestoneRequest({
    required this.babyId,
    required this.title,
    required this.achievedDate,
    this.notes,
  });

  Map<String, dynamic> toJson() => {
    'babyId': babyId,
    'title': title,
    'achievedDate': achievedDate.toIso8601String(),
    if (notes != null) 'notes': notes,
  };
}

/// =============================================================
/// SERVICE
/// =============================================================

class MilestoneApiService {
  Future<List<MilestoneEntry>> getForBaby(int babyId) async {
    final res = await ApiClient.get('/api/Milestone?BabyId=$babyId');

    if (res.statusCode != 200) {
      throw Exception('Failed to load milestones');
    }

    final decoded = jsonDecode(res.body) as List;

    final list = decoded.map((e) => MilestoneEntry.fromJson(e)).toList()
      ..sort((a, b) => a.achievedDate.compareTo(b.achievedDate));

    return list;
  }

  Future<MilestoneEntry> create(CreateMilestoneRequest request) async {
    final res = await ApiClient.post('/api/Milestone', body: request.toJson());

    if (res.statusCode != 200 && res.statusCode != 201) {
      throw Exception('Failed to save milestone');
    }

    return MilestoneEntry.fromJson(jsonDecode(res.body));
  }
}

/// =============================================================
/// SCREEN
/// =============================================================

class MilestoneScreen extends StatefulWidget {
  final int babyId;
  final String babyName;

  const MilestoneScreen({
    super.key,
    required this.babyId,
    required this.babyName,
  });

  @override
  State<MilestoneScreen> createState() => _MilestoneScreenState();
}

class _MilestoneScreenState extends State<MilestoneScreen> {
  final _service = MilestoneApiService();

  bool _loading = true;
  bool _saving = false;

  List<MilestoneEntry> _items = [];

  final _titleCtrl = TextEditingController();
  final _notesCtrl = TextEditingController();
  final _dateCtrl = TextEditingController();

  DateTime _selectedDate = DateTime.now();

  @override
  void initState() {
    super.initState();
    _syncDate();
    _load();
  }

  void _syncDate() {
    _dateCtrl.text = _formatDate(_selectedDate);
  }

  Future<void> _load() async {
    try {
      final data = await _service.getForBaby(widget.babyId);
      setState(() {
        _items = data;
        _loading = false;
      });
    } catch (_) {
      setState(() => _loading = false);
      NestlyToast.error(context, 'Greška pri učitavanju');
    }
  }

  Future<void> _pickDate() async {
    final picked = await showDatePicker(
      context: context,
      initialDate: _selectedDate,
      firstDate: DateTime(DateTime.now().year - 5),
      lastDate: DateTime.now(),
    );

    if (picked != null) {
      _selectedDate = picked;
      _syncDate();
      setState(() {});
    }
  }

  Future<void> _save() async {
    if (_saving || _titleCtrl.text.trim().isEmpty) return;

    setState(() => _saving = true);

    try {
      final created = await _service.create(
        CreateMilestoneRequest(
          babyId: widget.babyId,
          title: _titleCtrl.text.trim(),
          achievedDate: _selectedDate,
          notes: _notesCtrl.text.trim().isEmpty ? null : _notesCtrl.text.trim(),
        ),
      );

      setState(() {
        _items.add(created);
        _items.sort((a, b) => a.achievedDate.compareTo(b.achievedDate));
        _titleCtrl.clear();
        _notesCtrl.clear();
        _selectedDate = DateTime.now();
        _syncDate();
        _saving = false;
      });

      NestlyToast.success(
        context,
        'Dostignuće je sačuvano',
        accentColor: AppColors.seed,
      );
    } catch (_) {
      setState(() => _saving = false);
      NestlyToast.error(context, 'Greška pri spremanju');
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppColors.bg,
      appBar: AppBar(
        backgroundColor: Colors.transparent,
        elevation: 0,
        centerTitle: true,
        title: Text(
          'Dostignuća',
          style: Theme.of(context).textTheme.titleLarge?.copyWith(
            fontWeight: FontWeight.w900,
            color: AppColors.seed,
          ),
        ),

        leading: IconButton(
          icon: const Icon(Icons.arrow_back_ios_new_rounded),
          color: AppColors.seed,
          onPressed: () => Navigator.pop(context),
        ),
      ),
      body: _loading
          ? const Center(child: CircularProgressIndicator())
          : SingleChildScrollView(
              padding: const EdgeInsets.all(AppSpacing.lg),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.stretch,
                children: [
                  const SizedBox(height: AppSpacing.lg),
                  _buildListCard(),
                  const SizedBox(height: AppSpacing.lg),
                  _buildFormCard(),
                ],
              ),
            ),
    );
  }

  /// ================= UI =================

  Widget _buildListCard() {
    if (_items.isEmpty) {
      return Center(
        child: Text(
          'Još nema dodanih dostignuća',
          style: Theme.of(
            context,
          ).textTheme.bodyMedium?.copyWith(color: AppColors.textSecondary),
        ),
      );
    }

    return Card(
      shape: RoundedRectangleBorder(
        borderRadius: BorderRadius.circular(AppRadius.xl),
      ),
      child: Column(
        children: _items.map((e) {
          return ListTile(
            leading: CircleAvatar(
              backgroundColor: AppColors.seed.withOpacity(0.1),
              child: const Icon(Icons.star_rounded, color: AppColors.seed),
            ),
            title: Text(
              e.title,
              style: const TextStyle(fontWeight: FontWeight.w700),
            ),
            subtitle: e.notes != null ? Text(e.notes!) : null,
            trailing: Text(
              _formatDate(e.achievedDate),
              style: const TextStyle(color: AppColors.seed),
            ),
          );
        }).toList(),
      ),
    );
  }

  Widget _buildFormCard() {
    InputDecoration deco(String label) => InputDecoration(
      labelText: label,
      filled: true,
      fillColor: Colors.white,
      border: OutlineInputBorder(
        borderRadius: BorderRadius.circular(AppRadius.lg),
      ),
    );

    return Card(
      shape: RoundedRectangleBorder(
        borderRadius: BorderRadius.circular(AppRadius.xl),
      ),
      child: Padding(
        padding: const EdgeInsets.all(AppSpacing.lg),
        child: Column(
          children: [
            TextField(controller: _titleCtrl, decoration: deco('Naziv')),
            const SizedBox(height: 12),
            GestureDetector(
              onTap: _pickDate,
              child: AbsorbPointer(
                child: TextField(
                  controller: _dateCtrl,
                  decoration: deco('Datum'),
                ),
              ),
            ),
            const SizedBox(height: 12),
            TextField(
              controller: _notesCtrl,
              maxLines: 3,
              decoration: deco('Bilješka'),
            ),
            const SizedBox(height: 16),
            SizedBox(
              width: double.infinity,
              child: ElevatedButton(
                onPressed: _saving ? null : _save,
                style: ElevatedButton.styleFrom(
                  backgroundColor: AppColors.seed,
                  foregroundColor: Colors.white,
                  padding: const EdgeInsets.symmetric(vertical: 14),
                  shape: RoundedRectangleBorder(
                    borderRadius: BorderRadius.circular(AppRadius.lg),
                  ),
                ),
                child: _saving
                    ? const CircularProgressIndicator(color: Colors.white)
                    : const Text(
                        'Sačuvaj',
                        style: TextStyle(fontWeight: FontWeight.w700),
                      ),
              ),
            ),
          ],
        ),
      ),
    );
  }
}
