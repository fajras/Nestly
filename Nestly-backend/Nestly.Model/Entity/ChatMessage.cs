using System;
using System.ComponentModel.DataAnnotations;

namespace Nestly.Model.Entity
{
    public class ChatMessage
    {
        public long Id { get; set; }
        public long ConversationId { get; set; }
        public long SenderId { get; set; }
        [Required, MaxLength(4000)]
        public string Content { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
        public ChatConversation Conversation { get; set; }
        public AppUser Sender { get; set; }
    }

}
