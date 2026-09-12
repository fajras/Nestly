using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nestly.Model.Entity;

namespace Nestly.Services.Data.Seeders
{
    public static class ChatConversationSeeder
    {
        public static void SeedData(this EntityTypeBuilder<ChatConversation> entity)
        {
            entity.HasData(
                new ChatConversation { Id = 5001, User1Id = 1, User2Id = 3, CreatedAt = new DateTime(2026, 9, 2) },
                new ChatConversation { Id = 5002, User1Id = 1, User2Id = 6, CreatedAt = new DateTime(2026, 9, 4) },
                new ChatConversation { Id = 5003, User1Id = 1, User2Id = 9, CreatedAt = new DateTime(2026, 9, 6) }
            );
        }
    }
}
