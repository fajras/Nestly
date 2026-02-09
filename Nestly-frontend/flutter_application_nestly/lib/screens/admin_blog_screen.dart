import 'dart:convert';
import 'dart:io';

import 'package:flutter/foundation.dart' show kIsWeb;
import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;
import 'package:image_picker/image_picker.dart';
import 'package:cached_network_image/cached_network_image.dart';

import 'package:flutter_application_nestly/layouts/nestly_toast.dart';
import 'package:flutter_application_nestly/main.dart';

String _devBase() {
  if (kIsWeb) return 'http://localhost:5167';
  if (Platform.isAndroid) return 'http://10.0.2.2:5167';
  return 'http://localhost:5167';
}

String get _apiBase =>
    const String.fromEnvironment('API_BASE', defaultValue: '').isNotEmpty
    ? const String.fromEnvironment('API_BASE')
    : _devBase();

Map<String, String> _authHeader(String token) => {
  'Authorization': 'Bearer $token',
};

Map<String, String> _jsonHeaders(String token) => {
  'Authorization': 'Bearer $token',
  'Content-Type': 'application/json',
};

/// =======================
/// MODELS
/// =======================

class BlogPostRow {
  final int id;
  final String title;
  final String content;
  final String imageUrl;

  BlogPostRow({
    required this.id,
    required this.title,
    required this.content,
    required this.imageUrl,
  });

  factory BlogPostRow.fromJson(Map<String, dynamic> json) {
    final url = json['imageUrl'];
    return BlogPostRow(
      id: json['id'],
      title: json['title'],
      content: json['content'],
      imageUrl: url != null && url.isNotEmpty
          ? '$url?v=${DateTime.now().millisecondsSinceEpoch}'
          : '',
    );
  }
}

class BlogCategoryRow {
  final int id;
  final String name;

  BlogCategoryRow({required this.id, required this.name});

  factory BlogCategoryRow.fromJson(Map<String, dynamic> json) {
    return BlogCategoryRow(id: json['id'], name: json['name']);
  }
}

/// =======================
/// SERVICE
/// =======================

class BlogAdminService {
  Future<List<BlogPostRow>> getBlogs(String token) async {
    final res = await http.get(
      Uri.parse('$_apiBase/api/blogpost'),
      headers: _authHeader(token),
    );

    if (res.statusCode != 200) {
      throw Exception(res.body);
    }

    final List data = jsonDecode(res.body);
    return data.map((e) => BlogPostRow.fromJson(e)).toList();
  }

  Future<List<BlogCategoryRow>> getCategories(String token) async {
    final res = await http.get(
      Uri.parse('$_apiBase/api/blogpost/category'),
      headers: _authHeader(token),
    );

    if (res.statusCode != 200) {
      throw Exception(res.body);
    }

    final List data = jsonDecode(res.body);
    return data.map((e) => BlogCategoryRow.fromJson(e)).toList();
  }

  /// 1️⃣ Create blog WITHOUT image
  Future<int> createBlogWithoutImage({
    required String token,
    required String title,
    required String content,
    required List<int> categoryIds,
  }) async {
    final res = await http.post(
      Uri.parse('$_apiBase/api/blogpost'),
      headers: _jsonHeaders(token),
      body: jsonEncode({
        'title': title,
        'content': content,
        'authorId': 1,
        'categoryIds': categoryIds,
      }),
    );

    if (res.statusCode != 200 && res.statusCode != 201) {
      throw Exception(res.body);
    }

    final body = jsonDecode(res.body);
    return body['id'];
  }

  Future<void> uploadBlogImage({
    required String token,
    required int blogId,
    required File file,
  }) async {
    Exception? lastError;

    for (int attempt = 0; attempt < 3; attempt++) {
      try {
        final req = http.MultipartRequest(
          'POST',
          Uri.parse('$_apiBase/api/blogmedia/upload/$blogId'),
        );

        req.headers.addAll(_authHeader(token));

        req.files.add(
          await http.MultipartFile.fromPath(
            'file',
            file.path,
            filename: file.path.split('/').last,
          ),
        );

        final res = await req.send();

        if (res.statusCode == 200 || res.statusCode == 201) {
          return;
        }

        final body = await res.stream.bytesToString();
        throw Exception(body);
      } catch (e) {
        lastError = Exception(e.toString());
        await Future.delayed(const Duration(milliseconds: 300));
      }
    }

    throw lastError ?? Exception('Upload failed');
  }

  Future<void> deleteBlog(String token, int id) async {
    final res = await http.delete(
      Uri.parse('$_apiBase/api/blogpost/$id'),
      headers: _authHeader(token),
    );

    if (res.statusCode != 204) {
      throw Exception(res.body);
    }
  }
}

/// =======================
/// MAIN SCREEN
/// =======================

class DoctorAdminBlogScreen extends StatefulWidget {
  final String token;
  const DoctorAdminBlogScreen({super.key, required this.token});

  @override
  State<DoctorAdminBlogScreen> createState() => _DoctorAdminBlogScreenState();
}

class _DoctorAdminBlogScreenState extends State<DoctorAdminBlogScreen> {
  final _service = BlogAdminService();
  List<BlogPostRow> _blogs = [];
  bool _loading = true;

  @override
  void initState() {
    super.initState();
    _load();
  }

  Future<void> _load() async {
    setState(() => _loading = true);

    try {
      final data = await _service.getBlogs(widget.token);
      if (!mounted) return;
      setState(() => _blogs = data);
    } catch (_) {
      if (!mounted) return;
      NestlyToast.error(context, 'Greška pri učitavanju blogova');
    } finally {
      if (!mounted) return;
      setState(() => _loading = false);
    }
  }

  void _openEditor() {
    showModalBottomSheet(
      context: context,
      isScrollControlled: true,
      backgroundColor: Colors.transparent,
      builder: (_) => _BlogEditorSheet(token: widget.token, onSaved: _load),
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
          'Blog',
          style: TextStyle(fontSize: 26, fontWeight: FontWeight.w800),
        ),
        const SizedBox(height: AppSpacing.lg),
        Align(
          alignment: Alignment.centerRight,
          child: ElevatedButton.icon(
            onPressed: _openEditor,
            icon: const Icon(Icons.add),
            label: const Text('Novi blog'),
          ),
        ),
        const SizedBox(height: AppSpacing.lg),
        Expanded(
          child: ListView.separated(
            itemCount: _blogs.length,
            separatorBuilder: (_, __) => const SizedBox(height: 12),
            itemBuilder: (_, i) {
              final b = _blogs[i];
              return Card(
                child: ListTile(
                  leading: b.imageUrl.isNotEmpty
                      ? ClipRRect(
                          borderRadius: BorderRadius.circular(8),
                          child: CachedNetworkImage(
                            imageUrl: b.imageUrl,
                            width: 48,
                            height: 48,
                            fit: BoxFit.cover,
                            errorWidget: (_, __, ___) =>
                                const Icon(Icons.broken_image),
                          ),
                        )
                      : null,
                  title: Text(b.title),
                  subtitle: Text(
                    b.content,
                    maxLines: 2,
                    overflow: TextOverflow.ellipsis,
                  ),
                  trailing: IconButton(
                    icon: const Icon(Icons.delete),
                    onPressed: () async {
                      await _service.deleteBlog(widget.token, b.id);
                      _load();
                    },
                  ),
                ),
              );
            },
          ),
        ),
      ],
    );
  }
}

/// =======================
/// EDITOR SHEET
/// =======================

class _BlogEditorSheet extends StatefulWidget {
  final String token;
  final VoidCallback onSaved;

  const _BlogEditorSheet({required this.token, required this.onSaved});

  @override
  State<_BlogEditorSheet> createState() => _BlogEditorSheetState();
}

class _BlogEditorSheetState extends State<_BlogEditorSheet> {
  final _title = TextEditingController();
  final _content = TextEditingController();
  final _service = BlogAdminService();

  File? _image;
  List<BlogCategoryRow> _categories = [];
  final Set<int> _selectedCategoryIds = {};

  bool _loading = true;
  bool _saving = false;

  @override
  void initState() {
    super.initState();
    _loadCategories();
  }

  Future<void> _loadCategories() async {
    try {
      _categories = await _service.getCategories(widget.token);
    } finally {
      if (mounted) setState(() => _loading = false);
    }
  }

  Future<void> _pickImage() async {
    final picked = await ImagePicker().pickImage(
      source: ImageSource.gallery,
      imageQuality: 85,
    );
    if (picked != null) {
      setState(() => _image = File(picked.path));
    }
  }

  Future<void> _save() async {
    if (_title.text.isEmpty || _content.text.isEmpty) {
      NestlyToast.error(context, 'Naslov i sadržaj su obavezni');
      return;
    }

    setState(() => _saving = true);

    try {
      final blogId = await _service.createBlogWithoutImage(
        token: widget.token,
        title: _title.text,
        content: _content.text,
        categoryIds: _selectedCategoryIds.toList(),
      );
      await Future.delayed(const Duration(milliseconds: 400));
      if (_image != null) {
        await _service.uploadBlogImage(
          token: widget.token,
          blogId: blogId,
          file: _image!,
        );
      }

      if (!mounted) return;
      widget.onSaved();
      Navigator.pop(context);
      NestlyToast.success(context, 'Blog kreiran');
    } catch (_) {
      if (!mounted) return;
      NestlyToast.error(context, 'Greška pri spremanju');
    } finally {
      if (mounted) setState(() => _saving = false);
    }
  }

  @override
  Widget build(BuildContext context) {
    return Container(
      height: MediaQuery.of(context).size.height * .78,
      padding: const EdgeInsets.all(AppSpacing.lg),
      decoration: const BoxDecoration(
        color: Colors.white,
        borderRadius: BorderRadius.vertical(top: Radius.circular(24)),
      ),
      child: _loading
          ? const Center(child: CircularProgressIndicator())
          : SingleChildScrollView(
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  const Text(
                    'Novi blog članak',
                    style: TextStyle(fontSize: 22, fontWeight: FontWeight.w800),
                  ),
                  const SizedBox(height: AppSpacing.lg),
                  GestureDetector(
                    onTap: _pickImage,
                    child: Container(
                      height: 160,
                      decoration: BoxDecoration(
                        borderRadius: BorderRadius.circular(16),
                        color: AppColors.seed.withOpacity(.1),
                        image: _image != null
                            ? DecorationImage(
                                image: FileImage(_image!),
                                fit: BoxFit.cover,
                              )
                            : null,
                      ),
                      child: _image == null
                          ? const Center(
                              child: Icon(Icons.add_photo_alternate, size: 48),
                            )
                          : null,
                    ),
                  ),
                  const SizedBox(height: AppSpacing.lg),
                  TextField(
                    controller: _title,
                    decoration: const InputDecoration(labelText: 'Naslov'),
                  ),
                  const SizedBox(height: AppSpacing.md),
                  TextField(
                    controller: _content,
                    maxLines: 6,
                    decoration: const InputDecoration(labelText: 'Sadržaj'),
                  ),
                  const SizedBox(height: AppSpacing.lg),
                  Wrap(
                    spacing: 8,
                    children: _categories.map((c) {
                      final selected = _selectedCategoryIds.contains(c.id);
                      return FilterChip(
                        label: Text(c.name),
                        selected: selected,
                        onSelected: (v) {
                          setState(() {
                            v
                                ? _selectedCategoryIds.add(c.id)
                                : _selectedCategoryIds.remove(c.id);
                          });
                        },
                      );
                    }).toList(),
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
  }
}
