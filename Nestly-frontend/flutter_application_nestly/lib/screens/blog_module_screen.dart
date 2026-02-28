import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:flutter_application_nestly/network/api_client.dart';
import 'package:cached_network_image/cached_network_image.dart';
import 'package:flutter_application_nestly/main.dart';

String _snippet(String text, {int max = 140}) {
  if (text.isEmpty) return '—';
  if (text.length <= max) return text;
  return '${text.substring(0, max).trimRight()}...';
}

class BlogCategory {
  final int id;
  final String name;

  const BlogCategory({required this.id, required this.name});

  factory BlogCategory.fromJson(Map<String, dynamic> json) =>
      BlogCategory(id: json['id'], name: json['name'] ?? '');
}

class BlogPostDto {
  final int id;
  final String title;
  final String content;
  final String? imageUrl;
  final DateTime? createdAt;
  final List<int> categoryIds;

  const BlogPostDto({
    required this.id,
    required this.title,
    required this.content,
    this.imageUrl,
    this.createdAt,
    required this.categoryIds,
  });

  factory BlogPostDto.fromJson(Map<String, dynamic> json) {
    final raw = json['categoryIds'];
    return BlogPostDto(
      id: json['id'],
      title: json['title'] ?? '',
      content: json['content'] ?? '',
      imageUrl: json['imageUrl'],
      createdAt: json['createdAt'] != null
          ? DateTime.tryParse(json['createdAt'])
          : null,
      categoryIds: raw is List
          ? raw.map((e) => int.tryParse(e.toString())).whereType<int>().toList()
          : const [],
    );
  }
}

class BlogScreen extends StatefulWidget {
  const BlogScreen({super.key, required this.parentProfileId});
  final int parentProfileId;

  @override
  State<BlogScreen> createState() => _BlogScreenState();
}

class _BlogScreenState extends State<BlogScreen> {
  List<BlogCategory> _categories = const [];
  List<BlogPostDto> _posts = const [];
  int? _selectedCategoryId;
  bool _loading = true;
  String? _error;

  @override
  void initState() {
    super.initState();
    _init();
  }

  Future<void> _init() async {
    setState(() {
      _loading = true;
      _error = null;
      _selectedCategoryId = null;
    });

    try {
      final results = await Future.wait([
        fetchCategories(),
        fetchRecommended(take: 10),
      ]);

      if (!mounted) return;

      setState(() {
        _categories = results[0] as List<BlogCategory>;
        _posts = results[1] as List<BlogPostDto>;
        _loading = false;
      });
    } catch (e) {
      if (!mounted) return;
      setState(() {
        _error = 'Greška pri učitavanju bloga';
        _loading = false;
      });
    }
  }

  Future<void> _selectCategory(int? id) async {
    setState(() {
      _selectedCategoryId = id;
      _loading = true;
    });

    if (id == null) {
      final rec = await fetchRecommended(take: 10);

      if (!mounted) return;

      setState(() {
        _posts = rec;
        _loading = false;
      });

      return;
    }

    final posts = await fetchPosts(categoryId: id);

    if (!mounted) return;

    setState(() {
      _posts = posts;
      _loading = false;
    });
  }

  Future<void> _openDetail(BlogPostDto post) async {
    try {
      final fullPost = await fetchPostById(post.id);

      if (!mounted) return;

      Navigator.of(context).push(
        MaterialPageRoute(builder: (_) => BlogPostDetailScreen(post: fullPost)),
      );
    } catch (e) {
      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(content: Text('Greška pri učitavanju članka')),
      );
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppColors.bg,
      appBar: AppBar(
        backgroundColor: Colors.transparent,
        elevation: 0,
        leading: BackButton(color: AppColors.roseDark),
        title: Text(
          'Blog',
          style: Theme.of(context).textTheme.titleLarge?.copyWith(
            fontWeight: FontWeight.w700,
            color: AppColors.roseDark,
          ),
        ),
        centerTitle: true,
      ),
      body: RefreshIndicator(
        onRefresh: _init,
        color: AppColors.roseDark,
        child: ListView.builder(
          physics: const AlwaysScrollableScrollPhysics(),
          padding: const EdgeInsets.all(AppSpacing.xl),
          itemCount: _posts.length + 3,
          itemBuilder: (context, index) {
            if (index == 0) {
              return _CategoryFilterBar(
                categories: _categories,
                selectedId: _selectedCategoryId,
                onSelected: _selectCategory,
              );
            }

            if (index == 1) {
              if (_loading) {
                return const Padding(
                  padding: EdgeInsets.all(32),
                  child: Center(
                    child: CircularProgressIndicator(color: AppColors.roseDark),
                  ),
                );
              }

              if (_error != null) {
                return Padding(
                  padding: const EdgeInsets.all(AppSpacing.lg),
                  child: Text(
                    _error!,
                    textAlign: TextAlign.center,
                    style: const TextStyle(color: Colors.red),
                  ),
                );
              }

              if (_posts.isEmpty) {
                return const Padding(
                  padding: EdgeInsets.all(32),
                  child: Center(
                    child: Text(
                      'Trenutno nema blog objava.',
                      style: TextStyle(color: AppColors.textSecondary),
                    ),
                  ),
                );
              }

              return const SizedBox.shrink();
            }

            final postIndex = index - 2;

            if (postIndex < 0 || postIndex >= _posts.length) {
              return const SizedBox.shrink();
            }

            final post = _posts[postIndex];

            return RepaintBoundary(
              child: Padding(
                padding: const EdgeInsets.only(bottom: AppSpacing.md),
                child: _BlogPostCard(
                  post: post,
                  onTap: () => _openDetail(post),
                ),
              ),
            );
          },
        ),
      ),
    );
  }
}

class _CategoryFilterBar extends StatelessWidget {
  final List<BlogCategory> categories;
  final int? selectedId;
  final ValueChanged<int?> onSelected;

  const _CategoryFilterBar({
    required this.categories,
    required this.selectedId,
    required this.onSelected,
  });

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
  final String label;
  final bool selected;
  final VoidCallback onTap;

  const _CategoryChip({
    required this.label,
    required this.selected,
    required this.onTap,
  });

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
  final BlogPostDto post;
  final VoidCallback onTap;

  const _BlogPostCard({required this.post, required this.onTap});

  @override
  Widget build(BuildContext context) {
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
              _PostImage(imageUrl: post.imageUrl),
              const SizedBox(width: AppSpacing.lg),
              Expanded(child: _PostText(post: post)),
            ],
          ),
        ),
      ),
    );
  }
}

class _PostImage extends StatelessWidget {
  final String? imageUrl;

  const _PostImage({this.imageUrl});

  @override
  Widget build(BuildContext context) {
    if (imageUrl == null || imageUrl!.isEmpty) {
      return _fallbackIcon();
    }

    return ClipRRect(
      borderRadius: BorderRadius.circular(14),
      child: CachedNetworkImage(
        imageUrl: imageUrl!,
        width: 80,
        height: 80,
        fit: BoxFit.cover,
        memCacheWidth: 160,
        memCacheHeight: 160,

        placeholder: (_, __) => _skeleton(),
        errorWidget: (_, __, ___) => _fallbackIcon(),
      ),
    );
  }

  Widget _skeleton() => Container(
    width: 80,
    height: 80,
    decoration: BoxDecoration(
      color: AppColors.babyBlue.withOpacity(.2),
      borderRadius: BorderRadius.circular(14),
    ),
  );

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

class _PostText extends StatelessWidget {
  final BlogPostDto post;

  const _PostText({required this.post});

  @override
  Widget build(BuildContext context) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          post.title,
          maxLines: 2,
          overflow: TextOverflow.ellipsis,
          style: Theme.of(context).textTheme.titleMedium?.copyWith(
            fontWeight: FontWeight.w700,
            color: AppColors.roseDark,
          ),
        ),
        const SizedBox(height: 6),
        Text(
          _snippet(post.content),
          maxLines: 3,
          overflow: TextOverflow.ellipsis,
          style: Theme.of(context).textTheme.bodySmall?.copyWith(
            color: AppColors.textSecondary,
            height: 1.4,
          ),
        ),
      ],
    );
  }
}

Future<List<BlogCategory>> fetchCategories() async {
  final res = await ApiClient.get('/api/BlogPost/category');

  if (res.statusCode != 200) {
    throw Exception('Failed to load categories');
  }

  final List data = jsonDecode(res.body);
  return data.map((e) => BlogCategory.fromJson(e)).toList();
}

Future<List<BlogPostDto>> fetchPosts({int? categoryId}) async {
  final path = categoryId == null
      ? '/api/BlogPost'
      : '/api/BlogPost/category/$categoryId';

  final res = await ApiClient.get(path);

  if (res.statusCode != 200) {
    throw Exception('Failed to load posts');
  }

  final body = jsonDecode(res.body);
  return body is List
      ? body.map((e) => BlogPostDto.fromJson(e)).toList()
      : const [];
}

class BlogPostDetailScreen extends StatefulWidget {
  final BlogPostDto post;

  const BlogPostDetailScreen({super.key, required this.post});

  @override
  State<BlogPostDetailScreen> createState() => _BlogPostDetailScreenState();
}

class _BlogPostDetailScreenState extends State<BlogPostDetailScreen> {
  late DateTime _openedAt;
  @override
  void initState() {
    super.initState();
    _openedAt = DateTime.now();
  }

  @override
  void dispose() {
    final seconds = DateTime.now().difference(_openedAt).inSeconds;

    logBlogInteraction(
      postId: widget.post.id,
      eventType: 1,
      spentSeconds: seconds,
    );

    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    final post = widget.post;

    return Scaffold(
      backgroundColor: AppColors.bg,
      appBar: AppBar(
        backgroundColor: Colors.transparent,
        elevation: 0,
        leading: BackButton(color: AppColors.roseDark),
        title: Text(
          post.title,
          maxLines: 1,
          overflow: TextOverflow.ellipsis,
          style: Theme.of(context).textTheme.titleLarge?.copyWith(
            fontWeight: FontWeight.w700,
            color: AppColors.roseDark,
          ),
        ),
        centerTitle: true,
      ),
      body: SingleChildScrollView(
        padding: const EdgeInsets.all(AppSpacing.xl),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.stretch,
          children: [
            if (post.imageUrl != null && post.imageUrl!.isNotEmpty)
              ClipRRect(
                borderRadius: BorderRadius.circular(AppRadius.xl),
                child: CachedNetworkImage(
                  imageUrl: post.imageUrl!,
                  fit: BoxFit.cover,
                ),
              ),
            const SizedBox(height: AppSpacing.lg),
            Text(
              post.title,
              style: Theme.of(context).textTheme.headlineSmall?.copyWith(
                fontWeight: FontWeight.w700,
                color: AppColors.roseDark,
              ),
            ),
            const SizedBox(height: AppSpacing.xl),
            Text(
              post.content,
              style: Theme.of(
                context,
              ).textTheme.bodyLarge?.copyWith(height: 1.6),
            ),
          ],
        ),
      ),
    );
  }
}

Future<List<BlogPostDto>> fetchRecommended({int take = 5}) async {
  final res = await ApiClient.get('/api/recommendations/blog?take=$take');

  if (res.statusCode != 200) {
    throw Exception('Failed to load recommendations');
  }

  final List data = jsonDecode(res.body);
  return data.map((e) => BlogPostDto.fromJson(e)).toList();
}

Future<void> logBlogInteraction({
  required int postId,
  required int eventType,
  required int spentSeconds,
}) async {
  await ApiClient.post(
    '/api/recommendations/log',
    body: {
      "postId": postId,
      "eventType": eventType,
      "spentSeconds": spentSeconds,
    },
  );
}

Future<BlogPostDto> fetchPostById(int id) async {
  final res = await ApiClient.get('/api/BlogPost/$id');

  if (res.statusCode != 200) {
    throw Exception('Failed to load post');
  }

  return BlogPostDto.fromJson(jsonDecode(res.body));
}
