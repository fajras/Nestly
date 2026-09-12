using Microsoft.EntityFrameworkCore;
using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;
using Nestly.Services.Data;
using Nestly.Services.Exceptions;
using Nestly.Services.Extensions;
using Nestly.Services.Interfaces;
using Nestly.Services.Messaging;
namespace Nestly.Services.Repository
{
    public class ChatService : IChatService
    {
        private const int MaxMessageLength = 4000;

        private readonly IChatRepository _chatRepository;
        private readonly RabbitMqPublisher _publisher;
        private readonly IChatNotifier _chatNotifier;
        private readonly ICurrentUserService _currentUserService;
        private readonly NestlyDbContext _db;
        public ChatService(
            IChatRepository chatRepository,
            RabbitMqPublisher publisher,
            IChatNotifier chatNotifier,
            ICurrentUserService currentUserService, NestlyDbContext db)

        {
            _chatRepository = chatRepository;
            _publisher = publisher;
            _chatNotifier = chatNotifier;
            _currentUserService = currentUserService;
            _db = db;
        }


        public async Task<long> SendMessage(
            long senderId,
            SendMessageRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Content))
            {
                throw new BusinessException("Message cannot be empty.");
            }

            var content = request.Content.Trim();

            if (content.Length > MaxMessageLength)
            {
                throw new BusinessException($"Message cannot exceed {MaxMessageLength} characters.");
            }

            await _currentUserService
                .EnsureCanChatWithUserAsync(
                    request.ReceiverUserId);

            var conversation =
                await _chatRepository.GetConversation(
                    senderId,
                    request.ReceiverUserId);

            if (conversation == null)
            {
                conversation =
                    await _chatRepository.CreateConversation(
                        senderId,
                        request.ReceiverUserId);
            }

            if (conversation.User1Id != senderId &&
                conversation.User2Id != senderId)
            {
                throw new UnauthorizedAccessException(
                    "You do not have access to this conversation.");
            }

            var message = new ChatMessage
            {
                ConversationId = conversation.Id,
                SenderId = senderId,
                Content = content,
                CreatedAt = DateTime.UtcNow
            };

            _chatRepository.AddMessage(message);

            await _chatRepository.Save();

            var realtimeMessage =
                new ChatMessageRealtimeDto
                {
                    ConversationId = conversation.Id,
                    SenderId = senderId,
                    Content = message.Content,
                    CreatedAt = message.CreatedAt
                };

            await _chatNotifier.NotifyUser(
                senderId,
                realtimeMessage);

            await _chatNotifier.NotifyUser(
                request.ReceiverUserId,
                realtimeMessage);

            _publisher.Publish(new NotificationEvent
            {
                UserId = request.ReceiverUserId,
                Title = "Nova poruka",
                Message = "Imate novu poruku od korisnika."
            });

            return conversation.Id;
        }

        public async Task<List<ChatConversationResponse>> GetUserChats(long userId)
        {
            var chats = await _chatRepository.GetUserConversations(userId);

            var result = chats.Select(c =>
            {
                var otherUser = c.User1Id == userId ? c.User2 : c.User1;
                var parentProfile = otherUser.ParentProfile;

                var latestBabyBirthDate = parentProfile?.Babies?
                    .OrderByDescending(b => b.BirthDate)
                    .Select(b => (DateTime?)b.BirthDate)
                    .FirstOrDefault();

                var latestPregnancyDueDate = parentProfile?.Pregnancies?
                    .Where(p => p.DueDate != null && p.DueDate > DateTime.UtcNow)
                    .OrderByDescending(p => p.DueDate)
                    .Select(p => p.DueDate)
                    .FirstOrDefault();

                var (parentStatus, babyAgeMonths, pregnancyTrimester) =
                    ParentStatusCalculator.Resolve(latestBabyBirthDate, latestPregnancyDueDate);

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

            return result;
        }

        public async Task<ChatMessagePageResponse> GetMessages(
            long conversationId, long userId, int take = 50, long? beforeId = null)
        {
            var conversation = await _chatRepository.GetConversationById(conversationId);

            if (conversation == null ||
                (conversation.User1Id != userId && conversation.User2Id != userId))
            {
                throw new UnauthorizedAccessException();
            }

            var (messages, hasMore) = await _chatRepository.GetMessages(conversationId, take, beforeId);

            var items = messages.Select(m => new ChatMessageResponse
            {
                Id = m.Id,
                SenderId = m.SenderId,
                Content = m.Content,
                CreatedAt = m.CreatedAt
            }).ToList();

            return new ChatMessagePageResponse
            {
                Items = items,
                HasMore = hasMore
            };
        }

        public async Task<List<ChatUserDto>> GetAvailableUsers(
      long currentUserId)
        {
            return await _db.AppUsers
                .AsNoTracking()
                .Include(x => x.ParentProfile)
                    .ThenInclude(p => p.Babies)
                .Include(x => x.ParentProfile)
                    .ThenInclude(p => p.Pregnancies)
                .Where(x => x.Id != currentUserId)
                .Where(x => x.Role.Name == "Parent")
                .Select(x => new ChatUserDto
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,

                    ParentStatus =
                        x.ParentProfile!.Babies!.Any()
                            ? "PARENT"
                            : x.ParentProfile!.Pregnancies!.Any(p =>
                                p.DueDate != null &&
                                p.DueDate > DateTime.UtcNow)
                                ? "PREGNANT"
                                : "UNKNOWN",

                    BabyAgeMonths =
                        x.ParentProfile!.Babies!
                            .OrderByDescending(b => b.BirthDate)
                            .Select(b =>
                                ((DateTime.UtcNow.Year - b.BirthDate.Year) * 12)
                                + DateTime.UtcNow.Month
                                - b.BirthDate.Month
                                - (DateTime.UtcNow.Day < b.BirthDate.Day ? 1 : 0))
                            .FirstOrDefault(),

                    PregnancyTrimester =
                        x.ParentProfile!.Pregnancies!
                            .Where(p =>
                                p.DueDate != null &&
                                p.DueDate > DateTime.UtcNow)
                            .OrderByDescending(p => p.DueDate)
                            .Select(p =>
                                (
                                    ParentStatusCalculator.TotalGestationWeeks -
                                    ((p.DueDate!.Value - DateTime.UtcNow).Days / 7)
                                ) <= 13
                                    ? 1
                                    : (
                                        ParentStatusCalculator.TotalGestationWeeks -
                                        ((p.DueDate!.Value - DateTime.UtcNow).Days / 7)
                                      ) <= 27
                                        ? 2
                                        : 3)
                            .FirstOrDefault()
                })
                .ToListAsync();
        }
    }
}
