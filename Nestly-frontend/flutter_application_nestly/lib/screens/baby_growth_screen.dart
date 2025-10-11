import 'package:flutter/material.dart';
import 'package:flutter_application_nestly/main.dart';

class BabyGrowthScreen extends StatelessWidget {
  const BabyGrowthScreen({
    super.key,
    required this.week,
    required this.remainingDays,
  });

  final int week;
  final int remainingDays;

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
          'Sedmica $week',
          style: Theme.of(context).textTheme.titleLarge?.copyWith(
            fontWeight: FontWeight.w800,
            color: AppColors.roseDark,
          ),
        ),
        centerTitle: true,
      ),
      body: SingleChildScrollView(
        padding: const EdgeInsets.all(AppSpacing.xl),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.center,
          children: [
            Text(
              'Preostalo $remainingDays dana',
              style: Theme.of(context).textTheme.bodyMedium?.copyWith(
                color: AppColors.textSecondary,
                fontWeight: FontWeight.w500,
              ),
            ),
            const SizedBox(height: AppSpacing.lg),

            // 👶 Slika bebe
            Container(
              width: 180,
              height: 180,
              decoration: BoxDecoration(
                color: AppColors.babyBlue.withOpacity(0.15),
                shape: BoxShape.circle,
              ),
              child: Center(
                child: Image.network(
                  // privremena slika dok ne dodaš backend
                  'https://cdn-icons-png.flaticon.com/512/4140/4140048.png',
                  width: 100,
                  height: 100,
                  color: AppColors.roseDark,
                ),
              ),
            ),
            const SizedBox(height: AppSpacing.xl),

            // 📘 Sekcija: Razvoj bebe
            _InfoCard(
              title: 'Razvoj bebe',
              description:
                  'Vaša beba je sada veličine ploda kukuruza šećerca i teži oko 500g. Njeni organi se i dalje razvijaju i beba počinje reagovati na zvukove.',
            ),

            const SizedBox(height: AppSpacing.lg),

            // 💗 Sekcija: Promjene kod majke
            _InfoCard(
              title: 'Promjene kod majke',
              description:
                  'Možete osjećati pokrete i udarce dok se beba kreće. Takođe, mogu se pojaviti blage kontrakcije kao priprema tijela.',
            ),
          ],
        ),
      ),
    );
  }
}

class _InfoCard extends StatelessWidget {
  const _InfoCard({required this.title, required this.description});

  final String title;
  final String description;

  @override
  Widget build(BuildContext context) {
    return Card(
      color: Colors.white,
      elevation: 2,
      shape: RoundedRectangleBorder(
        borderRadius: BorderRadius.circular(AppRadius.lg),
      ),
      child: Padding(
        padding: const EdgeInsets.all(AppSpacing.lg),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Text(
              title,
              style: Theme.of(context).textTheme.titleMedium?.copyWith(
                fontWeight: FontWeight.w800,
                color: AppColors.roseDark,
              ),
            ),
            const SizedBox(height: AppSpacing.sm),
            Text(
              description,
              style: Theme.of(context).textTheme.bodyMedium?.copyWith(
                color: AppColors.textSecondary,
                height: 1.5,
              ),
            ),
          ],
        ),
      ),
    );
  }
}
