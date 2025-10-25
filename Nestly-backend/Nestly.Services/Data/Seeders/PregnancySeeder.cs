using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nestly.Model.Entity;

namespace Nestly.Services.Data.Seeders
{
    public static class PregnancySeeder
    {
        public static void SeedData(this EntityTypeBuilder<Pregnancy> entity)
        {
            var lmp = new DateTime(2025, 8, 1);
            entity.HasData(
                new Pregnancy
                {
                    Id = 1,
                    UserId = 1,
                    LmpDate = lmp,
                    DueDate = lmp.AddDays(280),
                    CycleLengthDays = 28
                }
            );
        }
    }
}
