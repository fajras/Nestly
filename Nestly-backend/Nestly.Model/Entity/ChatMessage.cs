using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nestly.Model.Entity
{
    public class ChatMessage
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey(nameof(ChatRoom))]
        public long RoomId { get; set; }
        [ForeignKey(nameof(AppUser))]
        public long UserId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }

        public ChatRoom Room { get; set; }
        public AppUser User { get; set; }
    }
}
