import 'package:flutter/material.dart';
import 'package:flutter_application_nestly/main.dart';

/// ---------------------------------------
/// DETALJ EKRAN (“Savjeti za sedmicu X”) – bez hero slike
/// ---------------------------------------
class _AdviceDetailScreen extends StatelessWidget {
  const _AdviceDetailScreen({required this.week, required this.api});
  final int week;
  final _AdviceApi api;

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
            color: AppColors.textPrimary,
          ),
          onPressed: () => Navigator.pop(context),
        ),
        title: Text(
          'Savjeti',
          style: Theme.of(context).textTheme.titleLarge?.copyWith(
            fontWeight: FontWeight.w800,
            color: AppColors.textPrimary,
          ),
        ),
        centerTitle: true,
      ),
      body: FutureBuilder<_AdviceDetail>(
        future: api.getDetail(week), // ⬅️ TODO: API
        builder: (context, snap) {
          if (!snap.hasData) {
            return const Center(
              child: CircularProgressIndicator(color: AppColors.roseDark),
            );
          }
          final data = snap.data!;
          return SingleChildScrollView(
            padding: const EdgeInsets.all(AppSpacing.xl),
            child: Center(
              child: ConstrainedBox(
                constraints: const BoxConstraints(maxWidth: 560),
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.stretch,
                  children: [
                    // Header kartica (u stilu dashboard headera)
                    Card(
                      color: AppColors.card,
                      elevation: 2,
                      shape: RoundedRectangleBorder(
                        borderRadius: BorderRadius.circular(AppRadius.xl),
                      ),
                      child: Padding(
                        padding: const EdgeInsets.all(AppSpacing.xl),
                        child: Column(
                          crossAxisAlignment: CrossAxisAlignment.start,
                          children: [
                            Text(
                              'Savjeti za sedmicu $week',
                              style: Theme.of(context).textTheme.headlineSmall
                                  ?.copyWith(
                                    fontWeight: FontWeight.w900,
                                    color: AppColors.roseDark,
                                  ),
                            ),
                            const SizedBox(height: AppSpacing.sm),
                            Text(
                              'Korisni naputci za bebin razvoj i vaše dobrobit.',
                              style: Theme.of(context).textTheme.bodyMedium
                                  ?.copyWith(color: AppColors.textSecondary),
                            ),
                            const SizedBox(height: AppSpacing.md),
                            Divider(
                              height: 20,
                              thickness: 0.7,
                              color: Colors.black.withOpacity(.06),
                            ),
                            const SizedBox(height: AppSpacing.sm),
                            Row(
                              children: [
                                Container(
                                  padding: const EdgeInsets.symmetric(
                                    horizontal: 12,
                                    vertical: 6,
                                  ),
                                  decoration: BoxDecoration(
                                    color: AppColors.babyBlue.withOpacity(0.22),
                                    borderRadius: BorderRadius.circular(12),
                                  ),
                                  child: Text(
                                    'Sedmica $week',
                                    style: Theme.of(context)
                                        .textTheme
                                        .labelLarge
                                        ?.copyWith(
                                          fontWeight: FontWeight.w700,
                                          color: AppColors.roseDark,
                                        ),
                                  ),
                                ),
                              ],
                            ),
                          ],
                        ),
                      ),
                    ),

                    const SizedBox(height: AppSpacing.xl),

                    // Sekcije
                    _SectionCard(title: 'Razvoj bebe', items: data.babyTips),
                    const SizedBox(height: AppSpacing.lg),

                    _SectionCard(
                      title: 'Promjene kod majke',
                      items: data.momTips,
                    ),
                    const SizedBox(height: AppSpacing.lg),

                    _SectionCard(title: 'Savjeti', items: data.genericTips),
                  ],
                ),
              ),
            ),
          );
        },
      ),
    );
  }
}

class _SectionCard extends StatelessWidget {
  const _SectionCard({required this.title, required this.items});
  final String title;
  final List<String> items;

  @override
  Widget build(BuildContext context) {
    return Card(
      color: Colors.white,
      elevation: 2,
      shape: RoundedRectangleBorder(
        borderRadius: BorderRadius.circular(AppRadius.xl),
      ),
      child: Padding(
        padding: const EdgeInsets.all(AppSpacing.xl),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            // naslov sekcije
            Row(
              children: [
                const Icon(
                  Icons.local_florist_rounded,
                  size: 20,
                  color: AppColors.roseDark,
                ),
                const SizedBox(width: AppSpacing.sm),
                Text(
                  title,
                  style: Theme.of(context).textTheme.titleMedium?.copyWith(
                    fontWeight: FontWeight.w800,
                    color: AppColors.roseDark,
                  ),
                ),
              ],
            ),
            const SizedBox(height: AppSpacing.md),

            // stavke
            ...items.mapIndexed((i, t) {
              return Padding(
                padding: EdgeInsets.only(
                  bottom: i == items.length - 1 ? 0 : 10,
                ),
                child: Row(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    const SizedBox(width: 2),
                    Container(
                      width: 8,
                      height: 8,
                      margin: const EdgeInsets.only(top: 7),
                      decoration: const BoxDecoration(
                        color: AppColors.roseDark,
                        shape: BoxShape.circle,
                      ),
                    ),
                    const SizedBox(width: AppSpacing.md),
                    Expanded(
                      child: Text(
                        t,
                        style: Theme.of(context).textTheme.bodyMedium?.copyWith(
                          color: AppColors.textPrimary,
                          height: 1.5,
                        ),
                      ),
                    ),
                  ],
                ),
              );
            }),
          ],
        ),
      ),
    );
  }
}

/// Mala pomoćna ekstenzija da lako dobijemo index u map() gore
extension _MapIndexed<E> on Iterable<E> {
  Iterable<T> mapIndexed<T>(T Function(int index, E e) f) sync* {
    var i = 0;
    for (final e in this) {
      yield f(i++, e);
    }
  }
}
