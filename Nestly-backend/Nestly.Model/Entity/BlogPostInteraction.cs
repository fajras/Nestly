using Nestly.Model.DTOObjects;
using System;
using System.ComponentModel.DataAnnotations;

namespace Nestly.Model.Entity
{
    public class BlogPostInteraction
    {
        [Key]
        public long Id { get; set; }
        public long UserId { get; set; }
        public long PostId { get; set; }
        public ArticleEventType EventType { get; set; }
        public int? SpentSeconds { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
