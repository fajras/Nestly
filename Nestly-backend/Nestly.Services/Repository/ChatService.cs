using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;
using Nestly.Services.Interfaces;
using Nestly.Services.Messaging;

namespace Nestly.Services.Repository
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepository;
        private readonly RabbitMqPublisher _publisher;
        public ChatService(
      IChatRepository chatRepository,
      RabbitMqPublisher publisher)
        {
            _chatRepository = chatRepository;
            _publisher = publisher;
        }

        public async Task SendMessage(long senderId, SendMessageRequest request)
        {
            var conversation =
                _chatRepository.GetConversation(senderId, request.ReceiverUserId)
                ?? _chatRepository.CreateConversation(senderId, request.ReceiverUserId);

            var message = new ChatMessage
            {
                ConversationId = conversation.Id,
                SenderId = senderId,
                Content = request.Content,
                CreatedAt = DateTime.UtcNow
            };

            _chatRepository.AddMessage(message);
            _chatRepository.Save();

            var realtimeMessage = new ChatMessageRealtimeDto
            {
                ConversationId = conversation.Id,
                SenderId = senderId,
                Content = message.Content,
                CreatedAt = message.CreatedAt
            };


            _publisher.Publish(new NotificationEvent
            {
                UserId = request.ReceiverUserId,
                Title = "Nova poruka",
                Message = $"Imate novu poruku od korisnika."
            });
        }

        public Task<List<ChatConversationResponse>> GetUserChats(long userId)
        {
            var chats = _chatRepository.GetUserConversations(userId);

            var result = chats.Select(c =>
            {
                var otherUser = c.User1Id == userId ? c.User2 : c.User1;
                var parentProfile = otherUser.ParentProfile;

                string parentStatus;
                int? babyAgeMonths = null;
                int? pregnancyTrimester = null;

                // 1️⃣ PRVO PROVJERA DIJETE
                var latestBaby = parentProfile?.Babies?
                    .OrderByDescending(b => b.BirthDate)
                    .FirstOrDefault();

                if (latestBaby != null)
                {
                    parentStatus = "PARENT";
                    babyAgeMonths = CalculateBabyAgeInMonths(latestBaby.BirthDate);
                }
                else
                {
                    var pregnancy = parentProfile?.Pregnancies?
                        .OrderByDescending(p => p.DueDate)
                        .FirstOrDefault(p => p.DueDate != null && p.DueDate > DateTime.UtcNow);

                    if (pregnancy != null && pregnancy.DueDate.HasValue)
                    {
                        parentStatus = "PREGNANT";
                        pregnancyTrimester = CalculatePregnancyTrimester(pregnancy.DueDate.Value);
                    }
                    else
                    {
                        parentStatus = "UNKNOWN";
                    }
                }

                var lastMessage = c.Messages
                    .OrderByDescending(m => m.CreatedAt)
                    .FirstOrDefault();

                return new ChatConversationResponse
                {
                    ConversationId = c.Id,

                    OtherUserId = otherUser.Id,
                    FirstName = otherUser.FirstName,
                    LastName = otherUser.LastName,

                    ParentStatus = parentStatus,
                    BabyAgeMonths = babyAgeMonths,
                    PregnancyTrimester = pregnancyTrimester,

                    LastMessage = lastMessage?.Content,
                    LastMessageTime = lastMessage?.CreatedAt
                };

            }).ToList();

            return Task.FromResult(result);
        }

        public Task<List<ChatMessageResponse>> GetMessages(long conversationId, long userId)
        {
            var conversation = _chatRepository.GetConversationById(conversationId);

            if (conversation.User1Id != userId && conversation.User2Id != userId)
            {
                throw new UnauthorizedAccessException();
            }

            var messages = _chatRepository.GetMessages(conversationId);

            var result = messages.Select(m => new ChatMessageResponse
            {
                Id = m.Id,
                SenderId = m.SenderId,
                Content = m.Content,
                CreatedAt = m.CreatedAt
            }).ToList();

            return Task.FromResult(result);
        }


        private static int CalculateBabyAgeInMonths(DateTime birthDate)
        {
            var now = DateTime.UtcNow;

            return (now.Year - birthDate.Year) * 12
                   + now.Month - birthDate.Month;
        }

        private static int CalculatePregnancyTrimester(DateTime dueDate)
        {
            var totalWeeks = 40;
            var weeksLeft = (dueDate - DateTime.UtcNow).Days / 7;
            var currentWeek = totalWeeks - weeksLeft;

            if (currentWeek <= 13)
            {
                return 1;
            }

            if (currentWeek <= 27)
            {
                return 2;
            }

            return 3;
        }
    }
}
