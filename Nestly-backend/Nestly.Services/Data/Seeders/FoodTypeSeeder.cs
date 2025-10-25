using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nestly.Model.Entity;

namespace Nestly.Services.Data.Seeders
{
    public static class FoodTypeSeeder
    {
        public static void SeedData(this EntityTypeBuilder<FoodType> entity)
        {
            entity.HasData(
                new FoodType { Id = 1, Name = "Rižine pahuljice" },
                new FoodType { Id = 2, Name = "Zobene pahuljice" },
                new FoodType { Id = 3, Name = "Kukuruzne pahuljice (bebe)" },
                new FoodType { Id = 4, Name = "Pšenična kaša (bebe)" },

                new FoodType { Id = 10, Name = "Mrkva" },
                new FoodType { Id = 11, Name = "Tikvica" },
                new FoodType { Id = 12, Name = "Batat (slatki krompir)" },
                new FoodType { Id = 13, Name = "Bundeva" },
                new FoodType { Id = 14, Name = "Brokula" },
                new FoodType { Id = 15, Name = "Karfiol" },
                new FoodType { Id = 16, Name = "Grašak" },
                new FoodType { Id = 17, Name = "Špinat termički obrađen" },

                new FoodType { Id = 20, Name = "Jabuka" },
                new FoodType { Id = 21, Name = "Kruška" },
                new FoodType { Id = 22, Name = "Banana" },
                new FoodType { Id = 23, Name = "Breskva" },
                new FoodType { Id = 24, Name = "Šljiva" },
                new FoodType { Id = 25, Name = "Borovnica" },
                new FoodType { Id = 26, Name = "Jagoda" },
                new FoodType { Id = 27, Name = "Citrusi (npr. narandža)" },

                new FoodType { Id = 30, Name = "Piletina" },
                new FoodType { Id = 31, Name = "Puretina" },
                new FoodType { Id = 32, Name = "Govedina" },
                new FoodType { Id = 33, Name = "Jaja (dobro termički)" },
                new FoodType { Id = 34, Name = "Jogurt (punomasni, obični)" },
                new FoodType { Id = 35, Name = "Svježi sir (cottage)" },
                new FoodType { Id = 36, Name = "Sir blagog okusa" },

                new FoodType { Id = 40, Name = "Leća" },
                new FoodType { Id = 41, Name = "Grah (crni, crveni)" },
                new FoodType { Id = 42, Name = "Slanutak" },

                new FoodType { Id = 45, Name = "Kikiriki maslac (razrijeđen)" },
                new FoodType { Id = 46, Name = "Badem maslac (razrijeđen)" },
                new FoodType { Id = 47, Name = "Orah (mljeven/maslac)" },

                new FoodType { Id = 50, Name = "Bijela riba" },
                new FoodType { Id = 51, Name = "Losos" },
                new FoodType { Id = 52, Name = "Tunjevina (povremeno)" },

                new FoodType { Id = 60, Name = "Integralni hljeb (meke kriške)" },
                new FoodType { Id = 61, Name = "Tjestenina male forme" },
                new FoodType { Id = 62, Name = "Kinoa" },
                new FoodType { Id = 63, Name = "Smeđa riža" },

                new FoodType { Id = 70, Name = "Krastavac (meke štapiće)" },
                new FoodType { Id = 71, Name = "Paradajz kuhan" },
                new FoodType { Id = 72, Name = "Kukuruz" },

                new FoodType { Id = 80, Name = "Grožđe (četvrtine)" },
                new FoodType { Id = 81, Name = "Malina" },

                new FoodType { Id = 90, Name = "Med" }
            );
        }
    }
}
