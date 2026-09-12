using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nestly.Model.Entity;

namespace Nestly.Services.Data.Seeders
{
    public static class BlogCategorySeeder
    {
        public static void SeedData(this EntityTypeBuilder<BlogCategory> entity)
        {
            entity.HasData(
                new BlogCategory
                {
                    Id = 1,
                    Name = "Trudnoća i zdravlje",
                    IsSystemCategory = true
                },
                new BlogCategory
                {
                    Id = 2,
                    Name = "Njega bebe",
                    IsSystemCategory = true
                },
                new BlogCategory
                {
                    Id = 3,
                    Name = "Ishrana i recepti",
                    IsSystemCategory = true
                },
                new BlogCategory
                {
                    Id = 4,
                    Name = "Savjeti roditelja",
                    IsSystemCategory = true
                },
                new BlogCategory
                {
                    Id = 5,
                    Name = "Psihološko zdravlje",
                    IsSystemCategory = true
                },
                new BlogCategory
                {
                    Id = 6,
                    Name = "Razvoj djeteta",
                    IsSystemCategory = true
                }
            );
        }
    }
}
