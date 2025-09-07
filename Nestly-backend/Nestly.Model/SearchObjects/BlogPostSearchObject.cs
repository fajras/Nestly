using System;

namespace Nestly.Model.SearchObjects
{
    public class BlogPostSearchObject
    {
        public long? AuthorId { get; set; }
        public string? Title { get; set; }
        public DateTime? CreatedFrom { get; set; }
        public DateTime? CreatedTo { get; set; }
    }
}
