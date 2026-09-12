using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nestly.Model.Entity;

namespace Nestly.Services.Data.Seeders
{
    public static class ParentProfileSeeder
    {
        public static void SeedData(this EntityTypeBuilder<ParentProfile> entity)
        {
            entity.HasData(
                new ParentProfile
                {
                    Id = 1,
                    UserId = 1,
                },
                new ParentProfile
                {
                    Id = 2,
                    UserId = 3,
                },
                new ParentProfile
                {
                    Id = 3,
                    UserId = 4,
                },
                new ParentProfile
                {
                    Id = 4,
                    UserId = 5,
                },
                new ParentProfile
                {
                    Id = 5,
                    UserId = 6,
                },
                new ParentProfile
                {
                    Id = 6,
                    UserId = 7,
                },
                new ParentProfile
                {
                    Id = 7,
                    UserId = 8,
                },
                new ParentProfile
                {
                    Id = 8,
                    UserId = 9,
                },
                new ParentProfile
                {
                    Id = 9,
                    UserId = 10,
                },
                new ParentProfile
                {
                    Id = 10,
                    UserId = 11,
                },
                new ParentProfile
                {
                    Id = 11,
                    UserId = 12,
                }
            );
        }
    }
}
