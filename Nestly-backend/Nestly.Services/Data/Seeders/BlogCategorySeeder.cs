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
                    Name = "Trudnoća i zdravlje"
                },
                new BlogCategory
                {
                    Id = 2,
                    Name = "Njega bebe"
                },
                new BlogCategory
                {
                    Id = 3,
                    Name = "Ishrana i recepti"
                },
                new BlogCategory
                {
                    Id = 4,
                    Name = "Savjeti roditelja"
                },
                new BlogCategory
                {
                    Id = 5,
                    Name = "Psihološko zdravlje"
                },
                new BlogCategory
                {
                    Id = 6,
                    Name = "Razvoj djeteta"
                }
            );
        }
    }
}
