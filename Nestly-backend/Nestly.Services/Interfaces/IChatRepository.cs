using Nestly.Model.Entity;

namespace Nestly.Services.Interfaces
{
    public interface IChatRepository
    {
        ChatConversation GetConversation(long user1Id, long user2Id);
        ChatConversation GetConversationById(long conversationId);

        ChatConversation CreateConversation(long user1Id, long user2Id);

        void AddMessage(ChatMessage message);

        List<ChatConversation> GetUserConversations(long userId);

        List<ChatMessage> GetMessages(long conversationId);

        void Save();
    }
}
