using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Nestly.Model.Entity
{
    public class BlogPostCategory
    {
        [ForeignKey(nameof(BlogPost))]
        public long PostId { get; set; }
        [ForeignKey(nameof(BlogCategory))]
        public int CategoryId { get; set; }
        [JsonIgnore]
        public BlogPost Post { get; set; }
        public BlogCategory Category { get; set; }
    }
}
