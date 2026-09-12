using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nestly.Model.Entity;

namespace Nestly.Services.Data.Seeders
{
    public static class BabyProfileSeeder
    {
        public static void SeedData(this EntityTypeBuilder<BabyProfile> entity)
        {
            entity.HasData(
                new BabyProfile
                {
                    Id = 1,
                    ParentProfileId = 1,
                    BabyName = "Emma",
                    Gender = "Female",
                    BirthDate = new DateTime(2026, 3, 10),
                    PregnancyId = 1
                },
                new BabyProfile
                {
                    Id = 2,
                    ParentProfileId = 6,
                    BabyName = "Faris",
                    Gender = "Male",
                    BirthDate = new DateTime(2026, 8, 10),
                    PregnancyId = null
                },
                new BabyProfile
                {
                    Id = 3,
                    ParentProfileId = 7,
                    BabyName = "Amar",
                    Gender = "Male",
                    BirthDate = new DateTime(2026, 5, 10),
                    PregnancyId = null
                },
                new BabyProfile
                {
                    Id = 4,
                    ParentProfileId = 8,
                    BabyName = "Lamija",
                    Gender = "Female",
                    BirthDate = new DateTime(2026, 1, 5),
                    PregnancyId = null
                },
                new BabyProfile
                {
                    Id = 5,
                    ParentProfileId = 9,
                    BabyName = "Hana",
                    Gender = "Female",
                    BirthDate = new DateTime(2025, 8, 3),
                    PregnancyId = null
                },
                new BabyProfile
                {
                    Id = 6,
                    ParentProfileId = 10,
                    BabyName = "Adnan",
                    Gender = "Male",
                    BirthDate = new DateTime(2025, 2, 4),
                    PregnancyId = null
                },
                new BabyProfile
                {
                    Id = 7,
                    ParentProfileId = 11,
                    BabyName = "Tarik",
                    Gender = "Male",
                    BirthDate = new DateTime(2024, 3, 6),
                    PregnancyId = null
                }
            );
        }
    }
}
