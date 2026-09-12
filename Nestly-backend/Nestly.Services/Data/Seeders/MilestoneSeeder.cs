using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nestly.Model.Entity;

namespace Nestly.Services.Data.Seeders
{
    public static class MilestoneSeeder
    {
        public static void SeedData(this EntityTypeBuilder<Milestone> entity)
        {
            entity.HasData(
                new Milestone { Id = 9001, BabyId = 1, Title = "Prvi osmijeh", AchievedDate = new DateTime(2026, 4, 9), Notes = null, CreatedAt = new DateTime(2026, 4, 9) },
                new Milestone { Id = 9002, BabyId = 1, Title = "Prati predmete pogledom", AchievedDate = new DateTime(2026, 5, 9), Notes = null, CreatedAt = new DateTime(2026, 5, 9) },
                new Milestone { Id = 9003, BabyId = 1, Title = "Podiže glavu dok leži na stomaku", AchievedDate = new DateTime(2026, 6, 8), Notes = null, CreatedAt = new DateTime(2026, 6, 8) },
                new Milestone { Id = 9004, BabyId = 1, Title = "Guguče i smije se naglas", AchievedDate = new DateTime(2026, 6, 23), Notes = null, CreatedAt = new DateTime(2026, 6, 23) },
                new Milestone { Id = 9005, BabyId = 1, Title = "Prevrće se sa stomaka na leđa", AchievedDate = new DateTime(2026, 7, 23), Notes = null, CreatedAt = new DateTime(2026, 7, 23) },
                new Milestone { Id = 9006, BabyId = 1, Title = "Počinje sjedati uz pridržavanje", AchievedDate = new DateTime(2026, 8, 22), Notes = null, CreatedAt = new DateTime(2026, 8, 22) },
                new Milestone { Id = 9007, BabyId = 1, Title = "Pokazuje interes za hranu", AchievedDate = new DateTime(2026, 9, 4), Notes = null, CreatedAt = new DateTime(2026, 9, 4) },
                new Milestone { Id = 5003, BabyId = 2, Title = "Prvi osmijeh", AchievedDate = new DateTime(2026, 8, 18), Notes = null, CreatedAt = new DateTime(2026, 8, 18) },
                new Milestone { Id = 5004, BabyId = 3, Title = "Podiže glavu dok leži na stomaku", AchievedDate = new DateTime(2026, 6, 9), Notes = null, CreatedAt = new DateTime(2026, 6, 9) },
                new Milestone { Id = 5005, BabyId = 3, Title = "Prati predmete pogledom", AchievedDate = new DateTime(2026, 7, 9), Notes = null, CreatedAt = new DateTime(2026, 7, 9) },
                new Milestone { Id = 5006, BabyId = 4, Title = "Samostalno sjedi", AchievedDate = new DateTime(2026, 3, 11), Notes = null, CreatedAt = new DateTime(2026, 3, 11) },
                new Milestone { Id = 5007, BabyId = 4, Title = "Počinje puzati", AchievedDate = new DateTime(2026, 7, 9), Notes = null, CreatedAt = new DateTime(2026, 7, 9) },
                new Milestone { Id = 5008, BabyId = 5, Title = "Prve samostalne riječi (mama, tata)", AchievedDate = new DateTime(2025, 11, 11), Notes = null, CreatedAt = new DateTime(2025, 11, 11) },
                new Milestone { Id = 5009, BabyId = 5, Title = "Stoji uz pridržavanje", AchievedDate = new DateTime(2026, 2, 19), Notes = null, CreatedAt = new DateTime(2026, 2, 19) },
                new Milestone { Id = 5010, BabyId = 6, Title = "Prvi samostalni koraci", AchievedDate = new DateTime(2025, 6, 14), Notes = null, CreatedAt = new DateTime(2025, 6, 14) },
                new Milestone { Id = 5011, BabyId = 6, Title = "Jede kašikom uz pomoć", AchievedDate = new DateTime(2025, 9, 22), Notes = null, CreatedAt = new DateTime(2025, 9, 22) },
                new Milestone { Id = 5012, BabyId = 7, Title = "Trči i penje se po namještaju", AchievedDate = new DateTime(2024, 10, 7), Notes = null, CreatedAt = new DateTime(2024, 10, 7) },
                new Milestone { Id = 5013, BabyId = 7, Title = "Govori kratke rečenice", AchievedDate = new DateTime(2025, 4, 25), Notes = null, CreatedAt = new DateTime(2025, 4, 25) }
            );
        }
    }
}
