using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nestly.Model.Entity;

namespace Nestly.Services.Data.Seeders
{
    public static class MealRecommendationSeeder
    {
        public static void SeedData(this EntityTypeBuilder<MealRecommendation> entity)
        {
            entity.HasData(
                new MealRecommendation { Id = 1, WeekNumber = 24, FoodTypeId = 1 },   // rižine pahuljice
                new MealRecommendation { Id = 2, WeekNumber = 24, FoodTypeId = 10 },  // mrkva
                new MealRecommendation { Id = 3, WeekNumber = 25, FoodTypeId = 11 },  // tikvica
                new MealRecommendation { Id = 4, WeekNumber = 26, FoodTypeId = 20 },  // jabuka
                new MealRecommendation { Id = 5, WeekNumber = 26, FoodTypeId = 21 },  // kruška
                new MealRecommendation { Id = 6, WeekNumber = 27, FoodTypeId = 22 },  // banana
                new MealRecommendation { Id = 7, WeekNumber = 27, FoodTypeId = 2 },  // zobene pahuljice
                new MealRecommendation { Id = 8, WeekNumber = 28, FoodTypeId = 12 },  // batat
                new MealRecommendation { Id = 9, WeekNumber = 28, FoodTypeId = 13 },  // bundeva

                new MealRecommendation { Id = 10, WeekNumber = 29, FoodTypeId = 14 },  // brokula
                new MealRecommendation { Id = 11, WeekNumber = 29, FoodTypeId = 15 },  // karfiol
                new MealRecommendation { Id = 12, WeekNumber = 30, FoodTypeId = 34 },  // jogurt (plain)
                new MealRecommendation { Id = 13, WeekNumber = 31, FoodTypeId = 33 },  // jaja (dobro term.)
                new MealRecommendation { Id = 14, WeekNumber = 32, FoodTypeId = 45 },  // kikiriki maslac (razrijeđen)

                new MealRecommendation { Id = 15, WeekNumber = 32, FoodTypeId = 30 },  // piletina
                new MealRecommendation { Id = 16, WeekNumber = 34, FoodTypeId = 31 },  // puretina
                new MealRecommendation { Id = 17, WeekNumber = 35, FoodTypeId = 40 },  // leća
                new MealRecommendation { Id = 18, WeekNumber = 36, FoodTypeId = 42 },  // slanutak

                new MealRecommendation { Id = 19, WeekNumber = 36, FoodTypeId = 16 },  // grašak
                new MealRecommendation { Id = 20, WeekNumber = 37, FoodTypeId = 61 },  // tjestenina male forme
                new MealRecommendation { Id = 21, WeekNumber = 38, FoodTypeId = 23 },  // breskva
                new MealRecommendation { Id = 22, WeekNumber = 39, FoodTypeId = 24 },  // šljiva

                new MealRecommendation { Id = 23, WeekNumber = 40, FoodTypeId = 32 },  // govedina
                new MealRecommendation { Id = 24, WeekNumber = 42, FoodTypeId = 35 },  // svježi sir
                new MealRecommendation { Id = 25, WeekNumber = 44, FoodTypeId = 25 },  // borovnica

                new MealRecommendation { Id = 26, WeekNumber = 52, FoodTypeId = 27 },  // citrusi
                new MealRecommendation { Id = 27, WeekNumber = 52, FoodTypeId = 71 },  // paradajz kuhan
                new MealRecommendation { Id = 28, WeekNumber = 54, FoodTypeId = 50 },  // bijela riba
                new MealRecommendation { Id = 29, WeekNumber = 56, FoodTypeId = 60 },  // integralni hljeb

                new MealRecommendation { Id = 30, WeekNumber = 58, FoodTypeId = 26 },  // jagoda
                new MealRecommendation { Id = 31, WeekNumber = 60, FoodTypeId = 46 },  // badem maslac
                new MealRecommendation { Id = 32, WeekNumber = 62, FoodTypeId = 41 },  // grah
                new MealRecommendation { Id = 33, WeekNumber = 64, FoodTypeId = 62 },  // kinoa

                new MealRecommendation { Id = 34, WeekNumber = 68, FoodTypeId = 51 },  // losos
                new MealRecommendation { Id = 35, WeekNumber = 70, FoodTypeId = 63 },  // smeđa riža
                new MealRecommendation { Id = 36, WeekNumber = 72, FoodTypeId = 70 },  // krastavac (meke štapiće)

                new MealRecommendation { Id = 37, WeekNumber = 76, FoodTypeId = 36 },  // sir blagi
                new MealRecommendation { Id = 38, WeekNumber = 80, FoodTypeId = 72 },  // kukuruz
                new MealRecommendation { Id = 39, WeekNumber = 82, FoodTypeId = 80 },  // grožđe (u četvrtine)

                new MealRecommendation { Id = 40, WeekNumber = 90, FoodTypeId = 90 },  // med
                new MealRecommendation { Id = 41, WeekNumber = 92, FoodTypeId = 47 },  // orah (mljeven/maslac)
                new MealRecommendation { Id = 42, WeekNumber = 100, FoodTypeId = 52 }, // tunjevina povremeno
                new MealRecommendation { Id = 43, WeekNumber = 104, FoodTypeId = 81 }  // malina
            );
        }
    }
}
