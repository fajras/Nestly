using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nestly.Model.Entity;

namespace Nestly.Services.Data.Seeders
{
    public static class PregnancySeeder
    {
        public static void SeedData(this EntityTypeBuilder<Pregnancy> entity)
        {
            var lmp = new DateTime(2025, 6, 3);
            var lmp2 = new DateTime(2026, 6, 27);
            var lmp3 = new DateTime(2026, 4, 6);
            var lmp4 = new DateTime(2026, 1, 1);
            var lmp5 = new DateTime(2025, 12, 8);
            var lmp6 = new DateTime(2026, 5, 25);

            entity.HasData(
                new Pregnancy
                {
                    Id = 1,
                    ParentProfileId = 1,
                    LmpDate = lmp,
                    DueDate = lmp.AddDays(280),
                    CycleLengthDays = 28
                },
                new Pregnancy
                {
                    Id = 2,
                    ParentProfileId = 2,
                    LmpDate = lmp2,
                    DueDate = lmp2.AddDays(280),
                    CycleLengthDays = 28
                },
                new Pregnancy
                {
                    Id = 3,
                    ParentProfileId = 3,
                    LmpDate = lmp3,
                    DueDate = lmp3.AddDays(280),
                    CycleLengthDays = 28
                },
                new Pregnancy
                {
                    Id = 4,
                    ParentProfileId = 4,
                    LmpDate = lmp4,
                    DueDate = lmp4.AddDays(280),
                    CycleLengthDays = 28
                },
                new Pregnancy
                {
                    Id = 5,
                    ParentProfileId = 5,
                    LmpDate = lmp5,
                    DueDate = lmp5.AddDays(280),
                    CycleLengthDays = 28
                },
                new Pregnancy
                {
                    Id = 6,
                    ParentProfileId = 11,
                    LmpDate = lmp6,
                    DueDate = lmp6.AddDays(280),
                    CycleLengthDays = 28
                }
            );
        }
    }
}
