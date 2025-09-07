using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nestly.Model.Entity
{
    public class ChatMember
    {
        [ForeignKey(nameof(ChatRoom))]
        public long RoomId { get; set; }
        [ForeignKey(nameof(AppUser))]
        public long UserId { get; set; }
        public DateTime JoinedAt { get; set; }

        public ChatRoom Room { get; set; }
        public AppUser User { get; set; }
    }
}
