using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nestly.Model.Entity;

namespace Nestly.Services.Data.Seeders
{
    public static class ChatMessageSeeder
    {
        public static void SeedData(this EntityTypeBuilder<ChatMessage> entity)
        {
            entity.HasData(
                // Conversation 5001: Amela (parent@nestly.com) <-> Amina Hodžić
                new ChatMessage { Id = 9001, ConversationId = 5001, SenderId = 1, Content = "Zdravo Amina! Vidjela sam da je i tvoja beba rođena otprilike u isto vrijeme kad i Emma.", CreatedAt = new DateTime(2026, 9, 2, 11, 15, 0) },
                new ChatMessage { Id = 9002, ConversationId = 5001, SenderId = 3, Content = "Zdravo! Da, razlika je par sedmica. Kako vama ide sa uvođenjem čvrste hrane?", CreatedAt = new DateTime(2026, 9, 2, 11, 20, 0) },
                new ChatMessage { Id = 9003, ConversationId = 5001, SenderId = 1, Content = "Polako, Emma najviše voli povrće, malo je izbirljivija sa voćem.", CreatedAt = new DateTime(2026, 9, 2, 11, 25, 0) },
                new ChatMessage { Id = 9004, ConversationId = 5001, SenderId = 3, Content = "Isto i moja! Probaj joj miksati voće sa jogurtom, meni je to upalilo.", CreatedAt = new DateTime(2026, 9, 2, 11, 30, 0) },
                new ChatMessage { Id = 9005, ConversationId = 5001, SenderId = 1, Content = "Hvala na ideji, probat ću sutra.", CreatedAt = new DateTime(2026, 9, 2, 11, 32, 0) },
                new ChatMessage { Id = 9006, ConversationId = 5001, SenderId = 3, Content = "Javi mi kako je prošlo. Kako spava noću kod vas?", CreatedAt = new DateTime(2026, 9, 4, 20, 10, 0) },
                new ChatMessage { Id = 9007, ConversationId = 5001, SenderId = 1, Content = "Prilično dobro, budi se jednom oko 3h ujutro pa opet zaspi.", CreatedAt = new DateTime(2026, 9, 4, 20, 15, 0) },
                new ChatMessage { Id = 9008, ConversationId = 5001, SenderId = 3, Content = "To je super u ovom uzrastu! Mi se još budimo dva-tri puta.", CreatedAt = new DateTime(2026, 9, 4, 20, 18, 0) },

                // Conversation 5002: Amela <-> Emina Softić
                new ChatMessage { Id = 9009, ConversationId = 5002, SenderId = 1, Content = "Emina, jesi li ti već bila na vakcinaciji ovaj mjesec?", CreatedAt = new DateTime(2026, 9, 4, 9, 0, 0) },
                new ChatMessage { Id = 9010, ConversationId = 5002, SenderId = 6, Content = "Jesam, prošle sedmice. Prošlo je bez problema, samo malo temperature uveče.", CreatedAt = new DateTime(2026, 9, 4, 9, 5, 0) },
                new ChatMessage { Id = 9011, ConversationId = 5002, SenderId = 1, Content = "Dobro je znati, mi imamo zakazano za par dana pa sam malo nervozna.", CreatedAt = new DateTime(2026, 9, 4, 9, 8, 0) },
                new ChatMessage { Id = 9012, ConversationId = 5002, SenderId = 6, Content = "Nema razloga za brigu, samo ponesi sirup za temperaturu za svaki slučaj.", CreatedAt = new DateTime(2026, 9, 4, 9, 10, 0) },
                new ChatMessage { Id = 9013, ConversationId = 5002, SenderId = 1, Content = "Hoću, hvala ti puno!", CreatedAt = new DateTime(2026, 9, 4, 9, 12, 0) },
                new ChatMessage { Id = 9014, ConversationId = 5002, SenderId = 6, Content = "Nema na čemu, javi kako je prošlo.", CreatedAt = new DateTime(2026, 9, 4, 9, 13, 0) },

                // Conversation 5003: Amela <-> Merisa Karić
                new ChatMessage { Id = 9015, ConversationId = 5003, SenderId = 9, Content = "Bok Amela, kako vam ide sa spavanjem po danu? Moja je totalno preokrenula raspored.", CreatedAt = new DateTime(2026, 9, 6, 15, 0, 0) },
                new ChatMessage { Id = 9016, ConversationId = 5003, SenderId = 1, Content = "Haha, i mi smo prošli kroz tu fazu prije mjesec dana. Pomoglo je da uvedemo isto vrijeme spavanja svaki dan.", CreatedAt = new DateTime(2026, 9, 6, 15, 10, 0) },
                new ChatMessage { Id = 9017, ConversationId = 5003, SenderId = 9, Content = "Probat ću to. A kako vam ide sa zubićima, je li već počela nicati?", CreatedAt = new DateTime(2026, 9, 6, 15, 15, 0) },
                new ChatMessage { Id = 9018, ConversationId = 5003, SenderId = 1, Content = "Da, prvi zubić se pojavio prije nedjelju dana. Malo je bila nemirna par dana.", CreatedAt = new DateTime(2026, 9, 6, 15, 18, 0) },
                new ChatMessage { Id = 9019, ConversationId = 5003, SenderId = 9, Content = "Hvala na savjetu, javit ću se ako bude trebalo još pitanja!", CreatedAt = new DateTime(2026, 9, 6, 15, 20, 0) }
            );
        }
    }
}
