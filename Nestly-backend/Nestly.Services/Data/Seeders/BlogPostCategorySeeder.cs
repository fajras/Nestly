using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nestly.Model.Entity;

namespace Nestly.Services.Data.Seeders
{
    public static class BlogPostCategorySeeder
    {
        public static void SeedData(this EntityTypeBuilder<BlogPostCategory> entity)
        {
            entity.HasData(
                new BlogPostCategory { PostId = 1, CategoryId = 1 },
                new BlogPostCategory { PostId = 2, CategoryId = 1 },

                new BlogPostCategory { PostId = 3, CategoryId = 2 },
                new BlogPostCategory { PostId = 4, CategoryId = 2 },

                new BlogPostCategory { PostId = 5, CategoryId = 3 },
                new BlogPostCategory { PostId = 6, CategoryId = 3 },

                new BlogPostCategory { PostId = 7, CategoryId = 4 },
                new BlogPostCategory { PostId = 8, CategoryId = 4 },

                new BlogPostCategory { PostId = 9, CategoryId = 5 },
                new BlogPostCategory { PostId = 10, CategoryId = 5 },

                new BlogPostCategory { PostId = 11, CategoryId = 6 },
                new BlogPostCategory { PostId = 12, CategoryId = 6 }
            );
        }
    }
}
