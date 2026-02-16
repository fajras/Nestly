using System;

namespace Nestly.Model.DTOObjects
{

    public class ChatMessageRealtimeDto
    {
        public long ConversationId { get; set; }
        public long SenderId { get; set; }
        public string Content { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
    }

    public class SendMessageRequest
    {
        public long ReceiverUserId { get; set; }
        public string Content { get; set; } = default!;
    }

    public class ChatConversationResponse
    {
        public long ConversationId { get; set; }

        public long OtherUserId { get; set; }

        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;

        public string ParentStatus { get; set; } = default!;


        public int? BabyAgeMonths { get; set; }

        public int? PregnancyTrimester { get; set; }

        public string? LastMessage { get; set; }
        public DateTime? LastMessageTime { get; set; }
    }

    public class ChatMessageResponse
    {
        public long Id { get; set; }
        public long SenderId { get; set; }
        public string Content { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
    }
}
