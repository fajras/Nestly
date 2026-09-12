using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nestly.Model.Entity;

namespace Nestly.Services.Data.Seeders
{
    public static class NotificationSeeder
    {
        public static void SeedData(this EntityTypeBuilder<Notification> entity)
        {
            entity.HasData(
                new Notification { Id = 9001, UserId = 1, Title = "Odgovoreno pitanje", Message = "Doktor je odgovorio na vaše pitanje.", IsRead = true, CreatedAt = new DateTime(2026, 9, 6, 9, 30, 0) },
                new Notification { Id = 9002, UserId = 1, Title = "Podsjetnik za termin", Message = "Vakcinacija je zakazana za 5 dana.", IsRead = false, CreatedAt = new DateTime(2026, 9, 8, 9, 30, 0) },
                new Notification { Id = 9004, UserId = 1, Title = "Dostignuće zabilježeno", Message = "Novo dostignuće je dodano za bebu Emma.", IsRead = true, CreatedAt = new DateTime(2026, 8, 27, 9, 30, 0) },
                new Notification { Id = 5001, UserId = 3, Title = "Odgovoreno pitanje", Message = "Doktor je odgovorio na vaše pitanje.", IsRead = false, CreatedAt = new DateTime(2026, 9, 1, 9, 30, 0) },
                new Notification { Id = 5002, UserId = 6, Title = "Podsjetnik za termin", Message = "Vaš termin pregleda je za 2 dana.", IsRead = false, CreatedAt = new DateTime(2026, 9, 7, 9, 30, 0) },
                new Notification { Id = 5004, UserId = 10, Title = "Odgovoreno pitanje", Message = "Doktor je odgovorio na vaše pitanje.", IsRead = false, CreatedAt = new DateTime(2026, 9, 5, 9, 30, 0) },
                new Notification { Id = 5005, UserId = 1, Title = "Novi savjet dostupan", Message = "Dostupan je novi savjet za trenutnu sedmicu trudnoće.", IsRead = true, CreatedAt = new DateTime(2026, 9, 4, 9, 30, 0) }
            );
        }
    }
}
