using Nestly.Model.DTOObjects;

namespace Nestly.Services.Interfaces
{
    public interface IChatService
    {
        Task SendMessage(long senderId, SendMessageRequest request);

        Task<List<ChatConversationResponse>> GetUserChats(long userId);

        Task<List<ChatMessageResponse>> GetMessages(long conversationId, long userId);
    }

}
