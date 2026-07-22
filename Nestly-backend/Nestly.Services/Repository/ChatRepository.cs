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

        public async Task<ChatConversation?> GetConversation(long user1Id, long user2Id)
        {
            return await _context.ChatConversations
                .Include(c => c.Messages)
                .Include(c => c.User1)
                .Include(c => c.User2)
                .FirstOrDefaultAsync(c =>
                    (c.User1Id == user1Id && c.User2Id == user2Id) ||
                    (c.User1Id == user2Id && c.User2Id == user1Id));
        }

        public async Task<ChatConversation?> GetConversationById(long conversationId)
        {
            return await _context.ChatConversations
                .Include(c => c.User1)
                .Include(c => c.User2)
                .FirstOrDefaultAsync(c => c.Id == conversationId);
        }

        public async Task<ChatConversation> CreateConversation(long user1Id, long user2Id)
        {
            var conversation = new ChatConversation
            {
                User1Id = user1Id,
                User2Id = user2Id,
                CreatedAt = DateTime.UtcNow
            };

            _context.ChatConversations.Add(conversation);
            await _context.SaveChangesAsync();

            return conversation;
        }

        public void AddMessage(ChatMessage message)
        {
            _context.ChatMessages.Add(message);
        }

        public async Task<List<ChatConversation>> GetUserConversations(long userId)
        {
            return await _context.ChatConversations
                .Include(c => c.Messages)

                .Include(c => c.User1)
                    .ThenInclude(u => u.ParentProfile)
                        .ThenInclude(p => p.Babies)

                .Include(c => c.User1)
                    .ThenInclude(u => u.ParentProfile)
                        .ThenInclude(p => p.Pregnancies)

                .Include(c => c.User2)
                    .ThenInclude(u => u.ParentProfile)
                        .ThenInclude(p => p.Babies)

                .Include(c => c.User2)
                    .ThenInclude(u => u.ParentProfile)
                        .ThenInclude(p => p.Pregnancies)

                .Where(c =>
                    c.User1Id == userId ||
                    c.User2Id == userId)
                .ToListAsync();
        }

        public async Task<List<ChatMessage>> GetMessages(long conversationId, int maxCount = 200)
        {
            // Keep only the most recent `maxCount` messages so a very long
            // conversation doesn't load its entire history in one request.
            var recentDescending = await _context.ChatMessages
                .Where(m => m.ConversationId == conversationId)
                .OrderByDescending(m => m.CreatedAt)
                .Take(maxCount)
                .ToListAsync();

            recentDescending.Reverse();

            return recentDescending;
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
