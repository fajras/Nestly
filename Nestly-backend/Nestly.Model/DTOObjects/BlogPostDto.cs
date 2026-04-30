using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        public long Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? ImageUrl { get; set; }
        public long? AuthorId { get; set; }
        public UserPhase Phase { get; set; }
        public int? WeekFrom { get; set; }
        public int? WeekTo { get; set; }
        public List<int> CategoryIds { get; set; } = new();
    }

    public class BlogPostSearchObject
    {
        public long? AuthorId { get; set; }
        public string? Title { get; set; }
        public DateTime? CreatedFrom { get; set; }
        public DateTime? CreatedTo { get; set; }
        public int? CategoryId { get; set; }

        public int Page { get; set; } = 1;

        private int _pageSize = 20;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > 100 ? 100 : value;
        }
    }
    public class CreateBlogPostDto
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = default!;
        [Required]
        public string Content { get; set; } = default!;
        public long? AuthorId { get; set; }
        public int Phase { get; set; }
        public int? WeekFrom { get; set; }
        public int? WeekTo { get; set; }
        [Required]
        public List<int> CategoryIds { get; set; }
    }

}
