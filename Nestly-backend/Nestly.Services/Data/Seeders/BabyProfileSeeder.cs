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
                    BirthDate = new DateTime(2024, 12, 18),
                    PregnancyId = 1
                }
            );
        }
    }
}
