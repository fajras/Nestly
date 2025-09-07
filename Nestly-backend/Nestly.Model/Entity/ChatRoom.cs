using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nestly.Model.Entity
{
    public class ChatRoom
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsPrivate { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<ChatMember> Members { get; set; }
        public ICollection<ChatMessage> Messages { get; set; }
    }
}
