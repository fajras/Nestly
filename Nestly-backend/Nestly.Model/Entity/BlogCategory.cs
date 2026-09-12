using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nestly.Model.Entity
{
    public class BlogCategory
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        // Explicit flag for categories seeded by BlogCategorySeeder, replacing
        // the previous "id <= 6" magic-number check so the rule survives a
        // change of identifiers in the database.
        public bool IsSystemCategory { get; set; }
        public ICollection<BlogPostCategory> BlogPostCategories { get; set; } = new List<BlogPostCategory>();
    }
}
