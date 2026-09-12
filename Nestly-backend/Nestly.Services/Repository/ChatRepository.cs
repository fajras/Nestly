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

        public async Task<(List<ChatMessage> Messages, bool HasMore)> GetMessages(
            long conversationId, int take = 50, long? beforeId = null)
        {
            take = take < 1 ? 50 : take > 200 ? 200 : take;

            // Cursor-based pagination: the client fetches the newest page
            // first, then walks further back in history by passing the id
            // of the oldest message it already has as `beforeId`. Fetching
            // take+1 lets us tell whether there is another older page
            // without a separate COUNT query.
            var query = _context.ChatMessages
                .Where(m => m.ConversationId == conversationId);

            if (beforeId.HasValue)
            {
                query = query.Where(m => m.Id < beforeId.Value);
            }

            var descending = await query
                .OrderByDescending(m => m.Id)
                .Take(take + 1)
                .ToListAsync();

            var hasMore = descending.Count > take;

            if (hasMore)
            {
                descending.RemoveAt(descending.Count - 1);
            }

            descending.Reverse();

            return (descending, hasMore);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
