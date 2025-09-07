using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nestly.Model.Entity
{
    public class BlogPost
    {
        [Key]
        public long Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        [ForeignKey(nameof(DoctorProfile))]
        public long? AuthorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public DoctorProfile Author { get; set; }
        public ICollection<BlogPostCategory> BlogPostCategories { get; set; } = new List<BlogPostCategory>();
    }
}
