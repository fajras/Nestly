import 'dart:convert';
import 'dart:io' show Platform;

import 'package:flutter/foundation.dart' show kIsWeb;
import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;

import 'package:flutter_application_nestly/main.dart';

Map<String, String> _headers() => {
  'Content-Type': 'application/json',
  'Accept': 'application/json',
};

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

String get _blogBase => '$_apiBase/api/BlogPost';

String _snippet(String text, {int max = 140}) {
  if (text.isEmpty) return '—';
  if (text.length <= max) return text;
  return text.substring(0, max).trimRight() + '...';
}

class BlogCategory {
  final int id;
  final String name;

  BlogCategory({required this.id, required this.name});

  factory BlogCategory.fromJson(Map<String, dynamic> json) => BlogCategory(
    id: json['id'] as int,
    name: (json['name'] ?? '').toString(),
  );
}

class BlogPostDto {
  final int id;
  final String title;
  final String content;
  final String? imageUrl;
  final int? authorId;
  final DateTime? createdAt;
  final List<int> categoryIds;

  BlogPostDto({
    required this.id,
    required this.title,
    required this.content,
    this.imageUrl,
    this.authorId,
    this.createdAt,
    required this.categoryIds,
  });

  factory BlogPostDto.fromJson(Map<String, dynamic> json) {
    List<int> _cat(dynamic v) {
      if (v is List) {
        return v
            .map((e) => int.tryParse(e.toString()))
            .whereType<int>()
            .toList();
      }
      return [];
    }

    return BlogPostDto(
      id: json['id'] as int,
      title: (json['title'] ?? '').toString(),
      content: (json['content'] ?? '').toString(),
      imageUrl: json['imageUrl']?.toString(),
      authorId: json['authorId'] as int?,
      createdAt: json['createdAt'] != null
          ? DateTime.tryParse(json['createdAt'].toString())
          : null,
      categoryIds: _cat(json['categoryIds']),
    );
  }
}

Future<List<BlogCategory>> fetchCategories() async {
  final res = await http
      .get(Uri.parse('$_blogBase/category'), headers: _headers())
      .timeout(const Duration(seconds: 10));

  if (res.statusCode != 200) {
    throw Exception('Greška pri učitavanju kategorija (${res.statusCode})');
  }

  final list = jsonDecode(res.body) as List;
  return list
      .map((e) => BlogCategory.fromJson(e as Map<String, dynamic>))
      .toList();
}

Future<List<BlogPostDto>> fetchPosts({int? categoryId}) async {
  Uri uri;
  if (categoryId != null) {
    uri = Uri.parse('$_blogBase/category/$categoryId');
  } else {
    uri = Uri.parse(_blogBase);
  }

  final res = await http
      .get(uri, headers: _headers())
      .timeout(const Duration(seconds: 10));

  if (res.statusCode != 200) {
    throw Exception('Greška pri učitavanju objava (${res.statusCode})');
  }

  final body = jsonDecode(res.body);
  if (body is List) {
    return body
        .map((e) => BlogPostDto.fromJson(e as Map<String, dynamic>))
        .toList();
  }
  if (body is Map<String, dynamic>) {
    return [BlogPostDto.fromJson(body)];
  }
  return [];
}

class BlogScreen extends StatefulWidget {
  const BlogScreen({super.key, required this.parentProfileId});

  final int parentProfileId;

  @override
  State<BlogScreen> createState() => _BlogScreenState();
}

class _BlogScreenState extends State<BlogScreen> {
  List<BlogCategory> _categories = [];
  List<BlogPostDto> _posts = [];

  int? _selectedCategoryId;
  bool _loading = true;
  String? _error;

  @override
  void initState() {
    super.initState();
    _loadAll();
  }

  Future<void> _loadAll() async {
    setState(() {
      _loading = true;
      _error = null;
    });

    try {
      final cats = await fetchCategories();
      final posts = await fetchPosts();

      if (!mounted) return;
      setState(() {
        _categories = cats;
        _posts = posts;
        _loading = false;
      });
    } catch (e) {
      if (!mounted) return;
      setState(() {
        _error = e.toString();
        _loading = false;
      });
    }
  }

  Future<void> _onCategorySelected(int? categoryId) async {
    setState(() {
      _selectedCategoryId = categoryId;
      _loading = true;
      _error = null;
    });

    try {
      final posts = await fetchPosts(categoryId: _selectedCategoryId);
      if (!mounted) return;
      setState(() {
        _posts = posts;
        _loading = false;
      });
    } catch (e) {
      if (!mounted) return;
      setState(() {
        _error = e.toString();
        _loading = false;
      });
    }
  }

  void _openDetail(BlogPostDto post) {
    Navigator.of(
      context,
    ).push(MaterialPageRoute(builder: (_) => BlogPostDetailScreen(post: post)));
  }

  @override
  Widget build(BuildContext context) {
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
          onPressed: () => Navigator.pop(context),
        ),
        title: Text(
          'Blog',
          style: Theme.of(context).textTheme.titleLarge?.copyWith(
            fontWeight: FontWeight.w900,
            color: AppColors.roseDark,
          ),
        ),
        centerTitle: true,
      ),
      body: RefreshIndicator(
        color: AppColors.roseDark,
        onRefresh: _loadAll,
        child: SingleChildScrollView(
          physics: const AlwaysScrollableScrollPhysics(),
          padding: const EdgeInsets.all(AppSpacing.xl),
          child: Center(
            child: ConstrainedBox(
              constraints: const BoxConstraints(maxWidth: 720),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.stretch,
                children: [
                  _CategoryFilterBar(
                    categories: _categories,
                    selectedId: _selectedCategoryId,
                    onSelected: _onCategorySelected,
                  ),
                  const SizedBox(height: AppSpacing.lg),

                  if (_loading)
                    const Center(
                      child: Padding(
                        padding: EdgeInsets.all(32),
                        child: CircularProgressIndicator(
                          color: AppColors.roseDark,
                        ),
                      ),
                    )
                  else if (_error != null)
                    Padding(
                      padding: const EdgeInsets.all(AppSpacing.lg),
                      child: Text(
                        'Greška: $_error',
                        textAlign: TextAlign.center,
                        style: Theme.of(context).textTheme.bodyMedium?.copyWith(
                          color: Colors.red[700],
                        ),
                      ),
                    )
                  else if (_posts.isEmpty)
                    Padding(
                      padding: const EdgeInsets.all(AppSpacing.lg),
                      child: Text(
                        'Nema objava za odabranu kategoriju.',
                        textAlign: TextAlign.center,
                        style: Theme.of(context).textTheme.bodyMedium?.copyWith(
                          color: AppColors.textSecondary,
                        ),
                      ),
                    )
                  else
                    Column(
                      children: _posts
                          .map(
                            (p) => Padding(
                              padding: const EdgeInsets.only(
                                bottom: AppSpacing.md,
                              ),
                              child: _BlogPostCard(
                                post: p,
                                onTap: () => _openDetail(p),
                              ),
                            ),
                          )
                          .toList(),
                    ),
                ],
              ),
            ),
          ),
        ),
      ),
    );
  }
}

class _CategoryFilterBar extends StatelessWidget {
  const _CategoryFilterBar({
    required this.categories,
    required this.selectedId,
    required this.onSelected,
  });

  final List<BlogCategory> categories;
  final int? selectedId;
  final ValueChanged<int?> onSelected;

  @override
  Widget build(BuildContext context) {
    return SingleChildScrollView(
      scrollDirection: Axis.horizontal,
      child: Row(
        children: [
          _CategoryChip(
            label: 'Sve',
            selected: selectedId == null,
            onTap: () => onSelected(null),
          ),
          const SizedBox(width: 6),
          for (final c in categories) ...[
            _CategoryChip(
              label: c.name,
              selected: selectedId == c.id,
              onTap: () => onSelected(c.id),
            ),
            const SizedBox(width: 6),
          ],
        ],
      ),
    );
  }
}

class _CategoryChip extends StatelessWidget {
  const _CategoryChip({
    required this.label,
    required this.selected,
    required this.onTap,
  });

  final String label;
  final bool selected;
  final VoidCallback onTap;

  @override
  Widget build(BuildContext context) {
    return InkWell(
      borderRadius: BorderRadius.circular(999),
      onTap: onTap,
      child: Ink(
        padding: const EdgeInsets.symmetric(horizontal: 14, vertical: 8),
        decoration: BoxDecoration(
          borderRadius: BorderRadius.circular(999),
          color: selected ? AppColors.roseDark : Colors.white,
          border: Border.all(
            color: selected
                ? AppColors.roseDark
                : AppColors.babyBlue.withOpacity(.4),
          ),
        ),
        child: Text(
          label,
          style: Theme.of(context).textTheme.labelMedium?.copyWith(
            fontWeight: FontWeight.w700,
            color: selected ? Colors.white : AppColors.roseDark,
          ),
        ),
      ),
    );
  }
}

class _BlogPostCard extends StatelessWidget {
  const _BlogPostCard({required this.post, required this.onTap});

  final BlogPostDto post;
  final VoidCallback onTap;

  @override
  Widget build(BuildContext context) {
    final subtitle = _snippet(post.content);

    return Card(
      elevation: 3,
      shape: RoundedRectangleBorder(
        borderRadius: BorderRadius.circular(AppRadius.lg),
      ),
      child: InkWell(
        borderRadius: BorderRadius.circular(AppRadius.lg),
        onTap: onTap,
        child: Ink(
          padding: const EdgeInsets.all(AppSpacing.lg),
          child: Row(
            children: [
              if (post.imageUrl != null && post.imageUrl!.isNotEmpty)
                ClipRRect(
                  borderRadius: BorderRadius.circular(14),
                  child: Image.network(
                    post.imageUrl!,
                    width: 80,
                    height: 80,
                    fit: BoxFit.cover,
                    loadingBuilder: (context, child, loadingProgress) {
                      if (loadingProgress == null) {
                        return child;
                      }

                      final expected = loadingProgress.expectedTotalBytes;
                      final loaded = loadingProgress.cumulativeBytesLoaded;

                      return Container(
                        width: 80,
                        height: 80,
                        alignment: Alignment.center,
                        color: AppColors.babyPink.withOpacity(.1),
                        child: SizedBox(
                          width: 22,
                          height: 22,
                          child: CircularProgressIndicator(
                            strokeWidth: 2.5,
                            color: AppColors.roseDark,
                            value: expected != null ? loaded / expected : null,
                          ),
                        ),
                      );
                    },
                    errorBuilder: (_, __, ___) => _fallbackIcon(),
                  ),
                )
              else
                _fallbackIcon(),
              const SizedBox(width: AppSpacing.lg),
              Expanded(
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Text(
                      post.title,
                      maxLines: 2,
                      overflow: TextOverflow.ellipsis,
                      style: Theme.of(context).textTheme.titleMedium?.copyWith(
                        fontWeight: FontWeight.w800,
                        color: AppColors.roseDark,
                      ),
                    ),
                    const SizedBox(height: 6),
                    Text(
                      subtitle,
                      maxLines: 3,
                      overflow: TextOverflow.ellipsis,
                      style: Theme.of(context).textTheme.bodySmall?.copyWith(
                        color: AppColors.textSecondary,
                        height: 1.4,
                      ),
                    ),
                    if (post.createdAt != null) ...[
                      const SizedBox(height: 4),
                      Text(
                        '${post.createdAt!.day.toString().padLeft(2, '0')}.${post.createdAt!.month.toString().padLeft(2, '0')}.${post.createdAt!.year}.',
                        style: Theme.of(context).textTheme.labelSmall?.copyWith(
                          color: AppColors.textSecondary.withOpacity(0.7),
                        ),
                      ),
                    ],
                  ],
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }

  Widget _fallbackIcon() => Container(
    width: 80,
    height: 80,
    decoration: BoxDecoration(
      color: AppColors.babyPink.withOpacity(.18),
      borderRadius: BorderRadius.circular(14),
    ),
    child: const Icon(
      Icons.article_rounded,
      color: AppColors.roseDark,
      size: 34,
    ),
  );
}

class BlogPostDetailScreen extends StatelessWidget {
  const BlogPostDetailScreen({super.key, required this.post});
  final BlogPostDto post;

  @override
  Widget build(BuildContext context) {
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
          onPressed: () => Navigator.pop(context),
        ),
        title: Text(
          post.title,
          maxLines: 1,
          overflow: TextOverflow.ellipsis,
          style: Theme.of(context).textTheme.titleLarge?.copyWith(
            fontWeight: FontWeight.w800,
            color: AppColors.roseDark,
          ),
        ),
        centerTitle: true,
      ),
      body: SingleChildScrollView(
        padding: const EdgeInsets.all(AppSpacing.xl),
        child: Center(
          child: ConstrainedBox(
            constraints: const BoxConstraints(maxWidth: 720),
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.stretch,
              children: [
                if (post.imageUrl != null && post.imageUrl!.isNotEmpty)
                  ClipRRect(
                    borderRadius: BorderRadius.circular(AppRadius.xl),
                    child: Image.network(
                      post.imageUrl!,
                      fit: BoxFit.cover,
                      loadingBuilder: (context, child, loadingProgress) {
                        if (loadingProgress == null) {
                          return child;
                        }

                        final expected = loadingProgress.expectedTotalBytes;
                        final loaded = loadingProgress.cumulativeBytesLoaded;

                        return Container(
                          height: 220,
                          alignment: Alignment.center,
                          color: AppColors.babyBlue.withOpacity(.1),
                          child: SizedBox(
                            width: 32,
                            height: 32,
                            child: CircularProgressIndicator(
                              strokeWidth: 3,
                              color: AppColors.roseDark,
                              value: expected != null
                                  ? loaded / expected
                                  : null,
                            ),
                          ),
                        );
                      },
                      errorBuilder: (_, __, ___) => const SizedBox.shrink(),
                    ),
                  ),

                const SizedBox(height: AppSpacing.lg),
                Text(
                  post.title,
                  style: Theme.of(context).textTheme.headlineSmall?.copyWith(
                    fontWeight: FontWeight.w900,
                    color: AppColors.roseDark,
                  ),
                ),
                if (post.createdAt != null) ...[
                  const SizedBox(height: 4),
                  Text(
                    '${post.createdAt!.day.toString().padLeft(2, '0')}.${post.createdAt!.month.toString().padLeft(2, '0')}.${post.createdAt!.year}.',
                    style: Theme.of(context).textTheme.labelMedium?.copyWith(
                      color: AppColors.textSecondary.withOpacity(0.8),
                    ),
                  ),
                ],
                const SizedBox(height: AppSpacing.xl),
                Text(
                  post.content,
                  style: Theme.of(context).textTheme.bodyLarge?.copyWith(
                    color: AppColors.textPrimary,
                    height: 1.6,
                  ),
                ),
              ],
            ),
          ),
        ),
      ),
    );
  }
}
