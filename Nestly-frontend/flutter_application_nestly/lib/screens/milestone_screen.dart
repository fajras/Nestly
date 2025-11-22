import 'dart:convert';
import 'dart:io' show Platform;

import 'package:flutter/foundation.dart' show kIsWeb;
import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;

import 'package:flutter_application_nestly/main.dart';
import 'package:flutter_application_nestly/layouts/nestly_toast.dart';

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

String _formatDate(DateTime d) =>
    '${d.day.toString().padLeft(2, '0')}.${d.month.toString().padLeft(2, '0')}.${d.year}';

class MilestoneEntry {
  final int id;
  final int babyId;
  final String title;
  final DateTime achievedDate;
  final String? notes;

  MilestoneEntry({
    required this.id,
    required this.babyId,
    required this.title,
    required this.achievedDate,
    this.notes,
  });

  factory MilestoneEntry.fromJson(Map<String, dynamic> json) {
    return MilestoneEntry(
      id: json['id'] as int,
      babyId: json['babyId'] as int,
      title: json['title'] as String,
      achievedDate: DateTime.parse(json['achievedDate'] as String),
      notes: json['notes'] as String?,
    );
  }
}

class CreateMilestoneRequest {
  final int babyId;
  final String title;
  final DateTime achievedDate;
  final String? notes;

  CreateMilestoneRequest({
    required this.babyId,
    required this.title,
    required this.achievedDate,
    this.notes,
  });

  Map<String, dynamic> toJson() => {
    'babyId': babyId,
    'title': title,
    'achievedDate': achievedDate.toIso8601String(),
    'notes': notes,
    'createdAt': DateTime.now().toIso8601String(),
  };
}

class MilestoneApiService {
  String get _baseUrl => '$_apiBase/api/Milestone';

  Future<List<MilestoneEntry>> getForBaby({required int babyId}) async {
    final uri = Uri.parse('$_baseUrl?BabyId=$babyId');
    final resp = await http.get(uri);

    if (resp.statusCode != 200) {
      throw Exception('Greška pri dohvaćanju dostignuća (${resp.statusCode})');
    }

    final List<dynamic> data = jsonDecode(resp.body) as List<dynamic>;
    final list = data
        .map((e) => MilestoneEntry.fromJson(e as Map<String, dynamic>))
        .toList();

    list.sort((a, b) => a.achievedDate.compareTo(b.achievedDate));
    return list;
  }

  Future<MilestoneEntry> create({
    required CreateMilestoneRequest request,
    String? token,
  }) async {
    final uri = Uri.parse(_baseUrl);
    final resp = await http.post(
      uri,
      headers: _headers(token),
      body: jsonEncode(request.toJson()),
    );

    if (resp.statusCode != 201 && resp.statusCode != 200) {
      throw Exception('Greška pri spremanju dostignuća (${resp.statusCode})');
    }

    final Map<String, dynamic> data =
        jsonDecode(resp.body) as Map<String, dynamic>;
    return MilestoneEntry.fromJson(data);
  }
}

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

  bool _isLoading = true;
  bool _isSaving = false;

  List<MilestoneEntry> _items = [];

  final _titleCtrl = TextEditingController();
  final _notesCtrl = TextEditingController();
  DateTime _selectedDate = DateTime.now();

  @override
  void initState() {
    super.initState();
    _loadData();
  }

  @override
  void dispose() {
    _titleCtrl.dispose();
    _notesCtrl.dispose();
    super.dispose();
  }

  Future<void> _loadData() async {
    setState(() => _isLoading = true);
    try {
      final list = await _service.getForBaby(babyId: widget.babyId);
      if (!mounted) return;
      setState(() {
        _items = list;
        _isLoading = false;
      });
    } catch (e) {
      if (!mounted) return;
      setState(() => _isLoading = false);
      NestlyToast.error(context, 'Greška pri učitavanju dostignuća: $e');
    }
  }

  Future<void> _pickDate() async {
    final now = DateTime.now();
    final picked = await showDatePicker(
      context: context,
      initialDate: _selectedDate,
      firstDate: DateTime(now.year - 5, 1, 1),
      lastDate: now,
      helpText: 'Odaberi datum dostignuća',
    );
    if (picked != null) {
      setState(() => _selectedDate = picked);
    }
  }

  IconData _iconForTitle(String title) {
    final t = title.toLowerCase();
    if (t.contains('zub')) return Icons.health_and_safety_rounded;
    if (t.contains('smijeh') || t.contains('smije') || t.contains('smiješ'))
      return Icons.emoji_emotions_rounded;
    if (t.contains('hod') || t.contains('walk')) return Icons.directions_walk;
    if (t.contains('riječ') || t.contains('rijec') || t.contains('word')) {
      return Icons.chat_bubble_outline_rounded;
    }
    return Icons.star_rounded;
  }

  Future<void> _save() async {
    final title = _titleCtrl.text.trim();
    final notes = _notesCtrl.text.trim().isEmpty
        ? null
        : _notesCtrl.text.trim();

    if (title.isEmpty) {
      NestlyToast.warning(context, 'Unesite naziv dostignuća.');
      return;
    }

    setState(() => _isSaving = true);

    try {
      final created = await _service.create(
        request: CreateMilestoneRequest(
          babyId: widget.babyId,
          title: title,
          achievedDate: _selectedDate,
          notes: notes,
        ),
      );

      if (!mounted) return;

      setState(() {
        _items.add(created);
        _items.sort((a, b) => a.achievedDate.compareTo(b.achievedDate));
        _isSaving = false;
        _titleCtrl.clear();
        _notesCtrl.clear();
        _selectedDate = DateTime.now();
      });

      NestlyToast.success(context, 'Dostignuće je uspješno sačuvano.');
    } catch (e) {
      if (!mounted) return;
      setState(() => _isSaving = false);
      NestlyToast.error(context, 'Greška pri spremanju: $e');
    }
  }

  InputDecoration _fieldDecoration(String label, {String? hint}) =>
      InputDecoration(
        labelText: label,
        hintText: hint,
        filled: true,
        fillColor: Colors.white,
        border: OutlineInputBorder(borderRadius: BorderRadius.circular(16)),
        enabledBorder: OutlineInputBorder(
          borderRadius: BorderRadius.circular(16),
          borderSide: BorderSide(color: AppColors.seed.withOpacity(0.35)),
        ),
      );

  @override
  Widget build(BuildContext context) {
    final size = MediaQuery.sizeOf(context);

    return Scaffold(
      backgroundColor: AppColors.bg,
      appBar: AppBar(
        backgroundColor: Colors.transparent,
        elevation: 0,
        leading: IconButton(
          icon: const Icon(
            Icons.arrow_back_ios_new_rounded,
            color: AppColors.seed,
          ),
          onPressed: () => Navigator.of(context).pop(),
        ),
        centerTitle: true,
        title: Text(
          "Dostignuća",
          style: Theme.of(context).textTheme.titleMedium?.copyWith(
            fontWeight: FontWeight.w800,
            color: AppColors.seed,
          ),
        ),
      ),
      body: _isLoading
          ? const Center(child: CircularProgressIndicator())
          : SafeArea(
              child: Center(
                child: SingleChildScrollView(
                  padding: const EdgeInsets.fromLTRB(
                    AppSpacing.xl,
                    0,
                    AppSpacing.xl,
                    AppSpacing.xl,
                  ),
                  child: ConstrainedBox(
                    constraints: const BoxConstraints(maxWidth: 520),
                    child: Column(
                      crossAxisAlignment: CrossAxisAlignment.stretch,
                      children: [
                        _HeaderInstruction(babyName: widget.babyName),
                        const SizedBox(height: AppSpacing.lg),
                        _buildListCard(),
                        const SizedBox(height: AppSpacing.lg),
                        _buildFormCard(size),
                      ],
                    ),
                  ),
                ),
              ),
            ),
    );
  }

  Widget _buildListCard() {
    return Container(
      padding: const EdgeInsets.all(12),
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
      child: _items.isEmpty
          ? Padding(
              padding: const EdgeInsets.all(12),
              child: Text(
                'Još niste dodali dostignuća.\n'
                'Unesite prvo dostignuće u formi ispod i ono će se pojaviti ovdje.',
                textAlign: TextAlign.center,
                style: Theme.of(context).textTheme.bodyMedium?.copyWith(
                  color: AppColors.textSecondary,
                ),
              ),
            )
          : Column(
              children: [
                ...List.generate(_items.length, (index) {
                  final m = _items[index];
                  final isLast = index == _items.length - 1;
                  return Column(
                    children: [
                      _MilestoneRow(entry: m, icon: _iconForTitle(m.title)),
                      if (!isLast)
                        Divider(
                          height: 1,
                          color: AppColors.seed.withOpacity(0.08),
                        ),
                    ],
                  );
                }),
              ],
            ),
    );
  }

  Widget _buildFormCard(Size size) {
    final dateLabel = _formatDate(_selectedDate);

    return Container(
      padding: const EdgeInsets.all(16),
      decoration: BoxDecoration(
        color: AppColors.bg.withOpacity(0.95),
        borderRadius: BorderRadius.circular(24),
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Text(
            "Dodaj novo dostignuće",
            style: Theme.of(context).textTheme.bodyMedium?.copyWith(
              fontWeight: FontWeight.w700,
              color: AppColors.seed,
            ),
          ),
          const SizedBox(height: 10),
          TextField(
            controller: _titleCtrl,
            decoration: _fieldDecoration(
              "Naziv dostignuća",
              hint: "npr. Prvi zubić",
            ),
          ),
          const SizedBox(height: 12),
          GestureDetector(
            onTap: _pickDate,
            child: AbsorbPointer(
              child: TextField(
                decoration: _fieldDecoration("Datum", hint: "dd.mm.gggg"),
                controller: TextEditingController(text: dateLabel),
              ),
            ),
          ),
          const SizedBox(height: 12),
          TextField(
            controller: _notesCtrl,
            maxLines: 3,
            decoration: _fieldDecoration(
              "Bilješka (opcionalno)",
              hint: "npr. kako se beba ponašala, posebni detalji...",
            ),
          ),
          const SizedBox(height: 16),
          SizedBox(
            width: double.infinity,
            child: ElevatedButton(
              onPressed: _isSaving ? null : _save,
              style: ElevatedButton.styleFrom(
                backgroundColor: AppColors.seed,
                foregroundColor: Colors.white,
                padding: const EdgeInsets.symmetric(vertical: 14),
                shape: RoundedRectangleBorder(
                  borderRadius: BorderRadius.circular(18),
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
                      "Sačuvaj dostignuće",
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

class _HeaderInstruction extends StatelessWidget {
  const _HeaderInstruction({required this.babyName});

  final String babyName;

  @override
  Widget build(BuildContext context) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          'Posebni trenuci $babyName',
          style: Theme.of(context).textTheme.titleLarge?.copyWith(
            fontWeight: FontWeight.w700,
            color: AppColors.seed,
          ),
        ),
      ],
    );
  }
}

class _MilestoneRow extends StatelessWidget {
  const _MilestoneRow({required this.entry, required this.icon});

  final MilestoneEntry entry;
  final IconData icon;

  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: const EdgeInsets.symmetric(vertical: 8, horizontal: 4),
      child: Row(
        children: [
          Container(
            width: 38,
            height: 38,
            decoration: BoxDecoration(
              color: AppColors.seed.withOpacity(0.08),
              shape: BoxShape.circle,
            ),
            child: Icon(icon, size: 20, color: AppColors.seed),
          ),
          const SizedBox(width: 10),
          Expanded(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(
                  entry.title,
                  style: Theme.of(context).textTheme.bodyMedium?.copyWith(
                    fontWeight: FontWeight.w700,
                    color: AppColors.textPrimary,
                  ),
                ),
                if (entry.notes != null && entry.notes!.isNotEmpty)
                  Padding(
                    padding: const EdgeInsets.only(top: 2),
                    child: Text(
                      entry.notes!,
                      maxLines: 2,
                      overflow: TextOverflow.ellipsis,
                      style: Theme.of(context).textTheme.bodySmall?.copyWith(
                        color: AppColors.textSecondary,
                      ),
                    ),
                  ),
              ],
            ),
          ),
          const SizedBox(width: 8),
          Container(
            padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 6),
            decoration: BoxDecoration(
              color: AppColors.seed.withOpacity(0.06),
              borderRadius: BorderRadius.circular(999),
            ),
            child: Text(
              _formatDate(entry.achievedDate),
              style: Theme.of(context).textTheme.labelSmall?.copyWith(
                color: AppColors.seed,
                fontWeight: FontWeight.w600,
              ),
            ),
          ),
        ],
      ),
    );
  }
}
