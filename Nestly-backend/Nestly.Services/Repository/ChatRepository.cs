using Microsoft.EntityFrameworkCore;
using Nestly.Model.Entity;
using Nestly.Services.Data;
using Nestly.Services.Interfaces;

namespace Nestly.Services.Repository
{
    public class ChatRepository : IChatRepository
    {
        private readonly NestlyDbContext _context;

        public ChatRepository(NestlyDbContext context)
        {
            _context = context;
        }

        public ChatConversation GetConversation(long user1Id, long user2Id)
        {
            return _context.ChatConversations
                .Include(c => c.Messages)
                .Include(c => c.User1)
                .Include(c => c.User2)
                .FirstOrDefault(c =>
                    (c.User1Id == user1Id && c.User2Id == user2Id) ||
                    (c.User1Id == user2Id && c.User2Id == user1Id));
        }

        public ChatConversation GetConversationById(long conversationId)
        {
            return _context.ChatConversations
                .Include(c => c.User1)
                .Include(c => c.User2)
                .FirstOrDefault(c => c.Id == conversationId);
        }

        public ChatConversation CreateConversation(long user1Id, long user2Id)
        {
            var conversation = new ChatConversation
            {
                User1Id = user1Id,
                User2Id = user2Id,
                CreatedAt = DateTime.UtcNow
            };

            _context.ChatConversations.Add(conversation);
            _context.SaveChanges();

            return conversation;
        }

        public void AddMessage(ChatMessage message)
        {
            _context.ChatMessages.Add(message);
        }

        public List<ChatConversation> GetUserConversations(long userId)
        {
            return _context.ChatConversations
                .Include(c => c.Messages)
                .Include(c => c.User1)
                .Include(c => c.User2)
                .Where(c => c.User1Id == userId || c.User2Id == userId)
                .ToList();
        }

        public List<ChatMessage> GetMessages(long conversationId)
        {
            return _context.ChatMessages
                .Where(m => m.ConversationId == conversationId)
                .OrderBy(m => m.CreatedAt)
                .ToList();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
