using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nestly.Model.Entity
{
    public class BlogCategory
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<BlogPostCategory> BlogPostCategories { get; set; } = new List<BlogPostCategory>();
    }
}
