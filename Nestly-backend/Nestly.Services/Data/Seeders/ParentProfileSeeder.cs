using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nestly.Model.Entity;

namespace Nestly.Services.Data.Seeders
{
    public static class ParentProfileSeeder
    {
        public static void SeedData(this EntityTypeBuilder<ParentProfile> entity)
        {
            entity.HasData(new ParentProfile
            {
                Id = 1,
                UserId = 1,
                ConceptionDate = new DateTime(2025, 1, 1)
            });
        }
    }
}