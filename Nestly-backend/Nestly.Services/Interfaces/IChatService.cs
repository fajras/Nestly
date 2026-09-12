using Nestly.Model.DTOObjects;

namespace Nestly.Services.Interfaces
{
    public interface IChatService
    {
        Task<long> SendMessage(long senderId, SendMessageRequest request);

        Task<List<ChatConversationResponse>> GetUserChats(long userId);

        Task<ChatMessagePageResponse> GetMessages(
            long conversationId, long userId, int take = 50, long? beforeId = null);
        Task<List<ChatUserDto>> GetAvailableUsers(long currentUserId);
    }

}
