using System;
using System.Collections.Generic;

namespace Nestly.Model.DTOObjects
{
    public class BlogPostPatchDto
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        public long? AuthorId { get; set; }
    }
    public class BlogPostResponseDto
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string ImageUrl { get; set; }
        public long? AuthorId { get; set; }
    }
    public class BlogPostSearchObject
    {
        public long? AuthorId { get; set; }
        public string? Title { get; set; }
        public DateTime? CreatedFrom { get; set; }
        public DateTime? CreatedTo { get; set; }
        public int? CategoryId { get; set; }
    }
    public class CreateBlogPostDto
    {
        public string Title { get; set; } = default!;
        public string Content { get; set; } = default!;
        public long? AuthorId { get; set; }
        public string ImageUrl { get; set; }
        public List<int> CategoryIds { get; set; }
    }
    public class BlogCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
    }
}
