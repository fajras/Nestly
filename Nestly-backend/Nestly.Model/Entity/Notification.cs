using System;
using System.ComponentModel.DataAnnotations;

namespace Nestly.Model.Entity
{
    public class Notification
    {
        public int Id { get; set; }
        public long UserId { get; set; }

        [Required]
        public string Title { get; set; } = default!;

        [Required]
        public string Message { get; set; } = default!;

        public bool IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
