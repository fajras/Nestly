using System;

namespace Nestly.Model.Entity
{
    public class ChatMessage
    {
        public long Id { get; set; }
        public long ConversationId { get; set; }
        public long SenderId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public ChatConversation Conversation { get; set; }
        public AppUser Sender { get; set; }
    }

}
