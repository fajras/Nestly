using Nestly.Model.Entity;

namespace Nestly.Services.Interfaces
{
    public interface IChatRepository
    {
        Task<ChatConversation?> GetConversation(long user1Id, long user2Id);
        Task<ChatConversation?> GetConversationById(long conversationId);

        Task<ChatConversation> CreateConversation(long user1Id, long user2Id);

        void AddMessage(ChatMessage message);

        Task<List<ChatConversation>> GetUserConversations(long userId);

        Task<(List<ChatMessage> Messages, bool HasMore)> GetMessages(
            long conversationId, int take = 50, long? beforeId = null);

        Task Save();
    }
}
