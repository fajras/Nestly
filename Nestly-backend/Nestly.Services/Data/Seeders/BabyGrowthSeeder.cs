using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nestly.Model.Entity;

namespace Nestly.Services.Data.Seeders
{
    public static class BabyGrowthSeeder
    {
        public static void SeedData(this EntityTypeBuilder<BabyGrowth> entity)
        {
            entity.HasData(
                new BabyGrowth { Id = 9001, BabyId = 1, WeekNumber = 1, WeightKg = 3.4m, HeightCm = 50.0m, HeadCircumferenceCm = 34.5m },
                new BabyGrowth { Id = 9002, BabyId = 1, WeekNumber = 2, WeightKg = 3.7m, HeightCm = 51.5m, HeadCircumferenceCm = 35.0m },
                new BabyGrowth { Id = 9003, BabyId = 1, WeekNumber = 4, WeightKg = 4.4m, HeightCm = 54.5m, HeadCircumferenceCm = 36.2m },
                new BabyGrowth { Id = 9004, BabyId = 1, WeekNumber = 6, WeightKg = 5.0m, HeightCm = 57.0m, HeadCircumferenceCm = 37.2m },
                new BabyGrowth { Id = 9005, BabyId = 1, WeekNumber = 8, WeightKg = 5.5m, HeightCm = 59.0m, HeadCircumferenceCm = 38.0m },
                new BabyGrowth { Id = 9006, BabyId = 1, WeekNumber = 10, WeightKg = 6.0m, HeightCm = 60.5m, HeadCircumferenceCm = 38.7m },
                new BabyGrowth { Id = 9007, BabyId = 1, WeekNumber = 13, WeightKg = 6.6m, HeightCm = 62.5m, HeadCircumferenceCm = 39.5m },
                new BabyGrowth { Id = 9008, BabyId = 1, WeekNumber = 17, WeightKg = 7.3m, HeightCm = 65.0m, HeadCircumferenceCm = 40.5m },
                new BabyGrowth { Id = 9009, BabyId = 1, WeekNumber = 21, WeightKg = 7.9m, HeightCm = 67.0m, HeadCircumferenceCm = 41.3m },
                new BabyGrowth { Id = 9010, BabyId = 1, WeekNumber = 26, WeightKg = 8.4m, HeightCm = 68.5m, HeadCircumferenceCm = 42.0m },
                new BabyGrowth { Id = 5001, BabyId = 2, WeekNumber = 1, WeightKg = 3.58m, HeightCm = 50.55m, HeadCircumferenceCm = 34.62m },
                new BabyGrowth { Id = 5002, BabyId = 2, WeekNumber = 2, WeightKg = 3.76m, HeightCm = 51.1m, HeadCircumferenceCm = 34.74m },
                new BabyGrowth { Id = 5003, BabyId = 2, WeekNumber = 4, WeightKg = 4.12m, HeightCm = 52.2m, HeadCircumferenceCm = 34.98m },
                new BabyGrowth { Id = 5004, BabyId = 3, WeekNumber = 1, WeightKg = 3.58m, HeightCm = 50.55m, HeadCircumferenceCm = 34.62m },
                new BabyGrowth { Id = 5005, BabyId = 3, WeekNumber = 8, WeightKg = 4.84m, HeightCm = 54.4m, HeadCircumferenceCm = 35.46m },
                new BabyGrowth { Id = 5006, BabyId = 3, WeekNumber = 17, WeightKg = 6.46m, HeightCm = 59.35m, HeadCircumferenceCm = 36.54m },
                new BabyGrowth { Id = 5007, BabyId = 4, WeekNumber = 1, WeightKg = 3.58m, HeightCm = 50.55m, HeadCircumferenceCm = 34.62m },
                new BabyGrowth { Id = 5008, BabyId = 4, WeekNumber = 17, WeightKg = 6.46m, HeightCm = 59.35m, HeadCircumferenceCm = 36.54m },
                new BabyGrowth { Id = 5009, BabyId = 4, WeekNumber = 35, WeightKg = 9.7m, HeightCm = 69.25m, HeadCircumferenceCm = 38.7m },
                new BabyGrowth { Id = 5010, BabyId = 5, WeekNumber = 1, WeightKg = 3.58m, HeightCm = 50.55m, HeadCircumferenceCm = 34.62m },
                new BabyGrowth { Id = 5011, BabyId = 5, WeekNumber = 28, WeightKg = 8.44m, HeightCm = 65.4m, HeadCircumferenceCm = 37.86m },
                new BabyGrowth { Id = 5012, BabyId = 5, WeekNumber = 57, WeightKg = 13.66m, HeightCm = 81.35m, HeadCircumferenceCm = 41.34m },
                new BabyGrowth { Id = 5013, BabyId = 6, WeekNumber = 1, WeightKg = 3.58m, HeightCm = 50.55m, HeadCircumferenceCm = 34.62m },
                new BabyGrowth { Id = 5014, BabyId = 6, WeekNumber = 41, WeightKg = 10.78m, HeightCm = 72.55m, HeadCircumferenceCm = 39.42m },
                new BabyGrowth { Id = 5015, BabyId = 6, WeekNumber = 82, WeightKg = 18.16m, HeightCm = 95.1m, HeadCircumferenceCm = 44.34m },
                new BabyGrowth { Id = 5016, BabyId = 7, WeekNumber = 1, WeightKg = 3.58m, HeightCm = 50.55m, HeadCircumferenceCm = 34.62m },
                new BabyGrowth { Id = 5017, BabyId = 7, WeekNumber = 65, WeightKg = 15.1m, HeightCm = 85.75m, HeadCircumferenceCm = 42.3m },
                new BabyGrowth { Id = 5018, BabyId = 7, WeekNumber = 130, WeightKg = 26.8m, HeightCm = 121.5m, HeadCircumferenceCm = 50.1m }
            );
        }
    }
}
