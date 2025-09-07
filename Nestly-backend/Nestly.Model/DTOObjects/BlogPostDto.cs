using System;
using System.Collections.Generic;

namespace Nestly.Model.PatchObjects
{
    public class BlogPostPatchDto
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        public long? AuthorId { get; set; }
    }
    public class BlogPostSearchObject
    {
        public long? AuthorId { get; set; }
        public string? Title { get; set; }
        public DateTime? CreatedFrom { get; set; }
        public DateTime? CreatedTo { get; set; }
    }
    public class CreateBlogPostDto
    {
        public string Title { get; set; } = default!;
        public string Content { get; set; } = default!;
        public long? AuthorId { get; set; } 
        public List<int> CategoryIds { get; set; } 
    }
}
