using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nestly.Model.Entity;

namespace Nestly.Services.Data.Seeders
{
    public static class CalendarEventSeeder
    {
        public static void SeedData(this EntityTypeBuilder<CalendarEvent> entity)
        {
            entity.HasData(
                new CalendarEvent { Id = 9001, BabyId = 1, UserId = 1, Title = "Prvi sistematski pregled", Description = "Sistematski pregled novorođenčeta.", StartAt = new DateTime(2026, 3, 17, 10, 0, 0) },
                new CalendarEvent { Id = 9002, BabyId = 1, UserId = 1, Title = "Kontrola u 1. mjesecu", Description = "Redovna kontrola rasta i razvoja.", StartAt = new DateTime(2026, 4, 9, 10, 0, 0) },
                new CalendarEvent { Id = 9003, BabyId = 1, UserId = 1, Title = "Vakcinacija - 2. mjesec", Description = "Redovna vakcinacija prema kalendaru.", StartAt = new DateTime(2026, 5, 9, 10, 0, 0) },
                new CalendarEvent { Id = 9004, BabyId = 1, UserId = 1, Title = "Kontrola u 4. mjesecu", Description = "Redovna kontrola kod pedijatra.", StartAt = new DateTime(2026, 7, 8, 10, 0, 0) },
                new CalendarEvent { Id = 9005, BabyId = 1, UserId = 1, Title = "Vakcinacija - 6. mjesec", Description = "Redovna vakcinacija prema kalendaru.", StartAt = new DateTime(2026, 9, 13, 10, 0, 0) },
                new CalendarEvent { Id = 9006, BabyId = 1, UserId = 1, Title = "Kontrola rasta i razvoja", Description = "Sistematski pregled za 6 mjeseci.", StartAt = new DateTime(2026, 9, 22, 10, 0, 0) },
                new CalendarEvent { Id = 5002, BabyId = 2, UserId = 6, Title = "Kontrola kod pedijatra", Description = "Redovni sistematski pregled za bebu Faris.", StartAt = new DateTime(2026, 9, 12, 10, 0, 0) },
                new CalendarEvent { Id = 5003, BabyId = 3, UserId = 7, Title = "Kontrola kod pedijatra", Description = "Redovni sistematski pregled za bebu Amar.", StartAt = new DateTime(2026, 9, 13, 10, 0, 0) },
                new CalendarEvent { Id = 5004, BabyId = 4, UserId = 8, Title = "Kontrola kod pedijatra", Description = "Redovni sistematski pregled za bebu Lamija.", StartAt = new DateTime(2026, 9, 14, 10, 0, 0) },
                new CalendarEvent { Id = 5005, BabyId = 5, UserId = 9, Title = "Kontrola kod pedijatra", Description = "Redovni sistematski pregled za bebu Hana.", StartAt = new DateTime(2026, 9, 10, 10, 0, 0) },
                new CalendarEvent { Id = 5006, BabyId = 6, UserId = 10, Title = "Kontrola kod pedijatra", Description = "Redovni sistematski pregled za bebu Adnan.", StartAt = new DateTime(2026, 9, 11, 10, 0, 0) },
                new CalendarEvent { Id = 5007, BabyId = 7, UserId = 11, Title = "Kontrola kod pedijatra", Description = "Redovni sistematski pregled za bebu Tarik.", StartAt = new DateTime(2026, 9, 12, 10, 0, 0) }
            );
        }
    }
}
