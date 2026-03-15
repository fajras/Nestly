import 'dart:convert';
import 'dart:io';
import 'package:flutter/material.dart';
import 'package:image_picker/image_picker.dart';
import 'package:cached_network_image/cached_network_image.dart';
import 'package:flutter_application_nestly/network/api_client.dart';
import 'package:flutter_application_nestly/layouts/nestly_toast.dart';
import 'package:flutter_application_nestly/main.dart';

class BlogPostRow {
  final int id;
  final String title;
  final String content;
  final String imageUrl;
  final List<int> categoryIds;
  BlogPostRow({
    required this.id,
    required this.title,
    required this.content,
    required this.imageUrl,
    required this.categoryIds,
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
      categoryIds:
          (json['categoryIds'] as List?)?.map((e) => e as int).toList() ?? [],
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

class BlogAdminService {
  Future<List<BlogPostRow>> getBlogs() async {
    try {
      final res = await ApiClient.get('/api/blogpost');

      if (res.statusCode != 200) {
        throw Exception("Failed to load blogs.");
      }

      final List data = jsonDecode(res.body);
      return data.map((e) => BlogPostRow.fromJson(e)).toList();
    } catch (_) {
      throw Exception("Unable to retrieve blogs.");
    }
  }

  Future<List<BlogCategoryRow>> getCategories() async {
    try {
      final res = await ApiClient.get('/api/BlogCategory');

      if (res.statusCode != 200) {
        throw Exception("Failed to load blog categories.");
      }

      final List data = jsonDecode(res.body);
      return data.map((e) => BlogCategoryRow.fromJson(e)).toList();
    } catch (_) {
      throw Exception("Unable to retrieve blog categories.");
    }
  }

  Future<void> updateBlog({
    required int id,
    String? title,
    String? content,
  }) async {
    final res = await ApiClient.patch(
      '/api/blogpost/$id',
      body: {
        if (title != null) 'title': title,
        if (content != null) 'content': content,
      },
    );

    if (res.statusCode != 200) {
      throw Exception('Failed to update blog');
    }
  }

  Future<int> createBlogWithoutImage({
    required String title,
    required String content,
    required int phase,
    int? weekFrom,
    int? weekTo,
    required List<int> categoryIds,
  }) async {
    final res = await ApiClient.post(
      '/api/blogpost',
      body: {
        'title': title,
        'content': content,
        'authorId': 1,
        'phase': phase,
        'weekFrom': weekFrom,
        'weekTo': weekTo,
        'categoryIds': categoryIds,
      },
    );

    if (res.statusCode != 200 && res.statusCode != 201) {
      throw Exception('Failed to create blog');
    }

    final body = jsonDecode(res.body);
    return body['id'];
  }

  Future<void> uploadBlogImage({
    required int blogId,
    required File file,
  }) async {
    try {
      await ApiClient.multipart('/api/blogmedia/upload/$blogId', file: file);
    } catch (_) {
      throw Exception('Failed to upload blog image');
    }
  }

  Future<void> deleteBlog(int id) async {
    final res = await ApiClient.delete('/api/blogpost/$id');

    if (res.statusCode != 204) {
      throw Exception('Failed to delete blog');
    }
  }
}

class DoctorAdminBlogScreen extends StatefulWidget {
  const DoctorAdminBlogScreen({super.key});

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
      final data = await _service.getBlogs();
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
      builder: (_) => _BlogEditorSheet(onSaved: _load),
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
          style: TextStyle(fontSize: 26, fontWeight: FontWeight.w700),
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
                  trailing: Row(
                    mainAxisSize: MainAxisSize.min,
                    children: [
                      IconButton(
                        icon: const Icon(Icons.edit),
                        onPressed: () {
                          showModalBottomSheet(
                            context: context,
                            isScrollControlled: true,
                            backgroundColor: Colors.transparent,
                            builder: (_) =>
                                _BlogEditorSheet(onSaved: _load, blog: b),
                          );
                        },
                      ),

                      if (b.id > 12)
                        IconButton(
                          icon: const Icon(Icons.delete),
                          onPressed: () async {
                            final confirm = await showDialog<bool>(
                              context: context,
                              builder: (context) {
                                return AlertDialog(
                                  title: const Text('Potvrda brisanja'),
                                  content: const Text(
                                    'Da li ste sigurni da želite obrisati ovaj blog članak?',
                                  ),
                                  actions: [
                                    TextButton(
                                      onPressed: () =>
                                          Navigator.pop(context, false),
                                      child: const Text('Otkaži'),
                                    ),
                                    ElevatedButton(
                                      onPressed: () =>
                                          Navigator.pop(context, true),
                                      child: const Text('Obriši'),
                                    ),
                                  ],
                                );
                              },
                            );

                            if (confirm == true) {
                              try {
                                await _service.deleteBlog(b.id);
                                await _load();

                                NestlyToast.success(
                                  Navigator.of(
                                    context,
                                    rootNavigator: true,
                                  ).context,
                                  'Blog uspješno obrisan',
                                  accentColor: AppColors.seed,
                                );
                              } catch (_) {
                                NestlyToast.error(
                                  Navigator.of(
                                    context,
                                    rootNavigator: true,
                                  ).context,
                                  'Brisanje bloga nije uspjelo',
                                );
                              }
                            }
                          },
                        )
                      else
                        const Tooltip(
                          message: 'Sistemski blog postovi se ne mogu brisati',
                          child: Icon(Icons.lock_outline, color: Colors.grey),
                        ),
                    ],
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

class _BlogEditorSheet extends StatefulWidget {
  final VoidCallback onSaved;
  final BlogPostRow? blog;

  const _BlogEditorSheet({required this.onSaved, this.blog});

  @override
  State<_BlogEditorSheet> createState() => _BlogEditorSheetState();
}

class _BlogEditorSheetState extends State<_BlogEditorSheet> {
  final _title = TextEditingController();
  final _content = TextEditingController();
  final _service = BlogAdminService();
  int _phase = 1;
  final _weekFrom = TextEditingController();
  final _weekTo = TextEditingController();

  File? _image;
  List<BlogCategoryRow> _categories = [];
  final Set<int> _selectedCategoryIds = {};

  bool _loading = true;
  bool _saving = false;

  @override
  void initState() {
    super.initState();
    _loadCategories();

    if (widget.blog != null) {
      _title.text = widget.blog!.title;
      _content.text = widget.blog!.content;
      _selectedCategoryIds.addAll(widget.blog!.categoryIds);
    }
  }

  @override
  void dispose() {
    _title.dispose();
    _content.dispose();
    _weekFrom.dispose();
    _weekTo.dispose();
    super.dispose();
  }

  Future<void> _loadCategories() async {
    try {
      _categories = await _service.getCategories();
    } catch (_) {
      if (!mounted) return;

      NestlyToast.error(
        Navigator.of(context, rootNavigator: true).context,
        'Greška pri učitavanju kategorija',
      );
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

  String? _validateForm() {
    final title = _title.text.trim();
    final content = _content.text.trim();

    if (title.isEmpty || content.isEmpty) {
      return 'Sva polja moraju biti popunjena';
    }

    if (title.length < 5) {
      return 'Naslov je prekratak';
    }

    if (content.length < 20) {
      return 'Sadržaj mora imati barem 20 karaktera';
    }

    if (widget.blog == null && _image == null) {
      return 'Slika je obavezna';
    }

    if (_selectedCategoryIds.isEmpty) {
      return 'Odaberite barem jednu kategoriju';
    }

    return null;
  }

  Future<void> _save() async {
    final title = _title.text.trim();
    final content = _content.text.trim();
    final weekFromText = _weekFrom.text.trim();
    final weekToText = _weekTo.text.trim();

    int? wf;
    int? wt;

    if (weekFromText.isNotEmpty) {
      wf = int.tryParse(weekFromText);
      if (wf == null) {
        NestlyToast.error(
          Navigator.of(context, rootNavigator: true).context,
          'Početna sedmica mora biti broj',
        );
        return;
      }
    }

    if (weekToText.isNotEmpty) {
      wt = int.tryParse(weekToText);
      if (wt == null) {
        NestlyToast.error(
          Navigator.of(context, rootNavigator: true).context,
          'Završna sedmica mora biti broj',
        );
        return;
      }
    }

    if (wf != null && wf <= 0) {
      NestlyToast.error(
        Navigator.of(context, rootNavigator: true).context,
        'Sedmica mora biti veća od nule',
      );
      return;
    }

    if (wt != null && wt <= 0) {
      NestlyToast.error(
        Navigator.of(context, rootNavigator: true).context,
        'Sedmica mora biti veća od nule',
      );
      return;
    }

    if (wf != null && wt != null && wf > wt) {
      NestlyToast.error(
        Navigator.of(context, rootNavigator: true).context,
        'Početna sedmica ne može biti veća od završne',
      );
      return;
    }

    final validationError = _validateForm();

    if (validationError != null) {
      NestlyToast.error(context, validationError);
      return;
    }

    setState(() => _saving = true);
    try {
      if (widget.blog != null) {
        await _service.updateBlog(
          id: widget.blog!.id,
          title: title,
          content: content,
        );
      } else {
        final blogId = await _service.createBlogWithoutImage(
          title: title,
          content: content,
          phase: _phase,
          weekFrom: wf,
          weekTo: wt,
          categoryIds: _selectedCategoryIds.toList(),
        );

        if (_image != null) {
          await _service.uploadBlogImage(blogId: blogId, file: _image!);
        }
      }

      if (!mounted) return;

      widget.onSaved();
      Navigator.pop(context);

      NestlyToast.success(
        Navigator.of(context, rootNavigator: true).context,
        widget.blog == null
            ? 'Blog uspješno kreiran'
            : 'Blog uspješno ažuriran',
        accentColor: AppColors.seed,
      );
    } catch (_) {
      if (!mounted) return;

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
      minChildSize: 0.7,
      maxChildSize: 0.95,
      expand: false,
      builder: (context, scrollController) {
        return Container(
          padding: const EdgeInsets.all(AppSpacing.lg),
          decoration: const BoxDecoration(
            color: Colors.white,
            borderRadius: BorderRadius.vertical(top: Radius.circular(28)),
          ),
          child: SingleChildScrollView(
            controller: scrollController,
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Center(
                  child: Column(
                    children: [
                      Text(
                        widget.blog == null
                            ? 'Novi blog članak'
                            : 'Uredi blog članak',
                        style: const TextStyle(
                          fontSize: 22,
                          fontWeight: FontWeight.w700,
                        ),
                      ),
                      SizedBox(height: 4),
                      Text(
                        'Kreirajte edukativni sadržaj za korisnice',
                        style: TextStyle(
                          color: AppColors.textSecondary,
                          fontSize: 14,
                        ),
                      ),
                    ],
                  ),
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
                          : widget.blog?.imageUrl.isNotEmpty == true
                          ? DecorationImage(
                              image: CachedNetworkImageProvider(
                                widget.blog!.imageUrl,
                              ),
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
                const SizedBox(height: AppSpacing.lg),

                const Text(
                  'Period',
                  style: TextStyle(fontWeight: FontWeight.w600),
                ),

                DropdownButtonFormField<int>(
                  value: _phase,
                  decoration: const InputDecoration(),
                  items: const [
                    DropdownMenuItem(value: 1, child: Text('Trudnoća')),
                    DropdownMenuItem(value: 2, child: Text('Njega bebe')),
                  ],
                  onChanged: (v) => setState(() => _phase = v ?? 1),
                ),

                const SizedBox(height: AppSpacing.md),

                TextField(
                  controller: _weekFrom,
                  keyboardType: TextInputType.number,
                  decoration: const InputDecoration(
                    labelText: 'Od sedmice (opcionalno)',
                  ),
                ),

                const SizedBox(height: AppSpacing.md),

                TextField(
                  controller: _weekTo,
                  keyboardType: TextInputType.number,
                  decoration: const InputDecoration(
                    labelText: 'Do sedmice (opcionalno)',
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
