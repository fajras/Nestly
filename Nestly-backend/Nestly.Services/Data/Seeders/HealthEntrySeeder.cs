using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nestly.Model.Entity;

namespace Nestly.Services.Data.Seeders
{
    public static class HealthEntrySeeder
    {
        public static void SeedData(this EntityTypeBuilder<HealthEntry> entity)
        {
            entity.HasData(
                new HealthEntry { Id = 9001, BabyId = 1, EntryDate = new DateTime(2026, 9, 8), TemperatureC = 36.5m, Medicines = "Vitamin D kapi", DoctorVisit = null },
                new HealthEntry { Id = 9002, BabyId = 1, EntryDate = new DateTime(2026, 9, 3), TemperatureC = 36.65m, Medicines = null, DoctorVisit = null },
                new HealthEntry { Id = 9003, BabyId = 1, EntryDate = new DateTime(2026, 8, 29), TemperatureC = 36.8m, Medicines = "Vitamin D kapi", DoctorVisit = null },
                new HealthEntry { Id = 9004, BabyId = 1, EntryDate = new DateTime(2026, 8, 24), TemperatureC = 36.5m, Medicines = null, DoctorVisit = null },
                new HealthEntry { Id = 9005, BabyId = 1, EntryDate = new DateTime(2026, 8, 19), TemperatureC = 36.65m, Medicines = "Vitamin D kapi", DoctorVisit = null },
                new HealthEntry { Id = 9006, BabyId = 1, EntryDate = new DateTime(2026, 8, 14), TemperatureC = 36.8m, Medicines = null, DoctorVisit = "Redovna kontrola kod pedijatra" },
                new HealthEntry { Id = 9007, BabyId = 1, EntryDate = new DateTime(2026, 7, 12), TemperatureC = 36.7m, Medicines = null, DoctorVisit = "Mjesečna kontrola kod pedijatra" },
                new HealthEntry { Id = 9008, BabyId = 1, EntryDate = new DateTime(2026, 6, 14), TemperatureC = 36.7m, Medicines = null, DoctorVisit = "Mjesečna kontrola kod pedijatra" },
                new HealthEntry { Id = 9009, BabyId = 1, EntryDate = new DateTime(2026, 5, 17), TemperatureC = 36.7m, Medicines = null, DoctorVisit = "Mjesečna kontrola kod pedijatra" },
                new HealthEntry { Id = 9010, BabyId = 1, EntryDate = new DateTime(2026, 4, 19), TemperatureC = 36.7m, Medicines = null, DoctorVisit = "Mjesečna kontrola kod pedijatra" },
                new HealthEntry { Id = 9011, BabyId = 1, EntryDate = new DateTime(2026, 3, 22), TemperatureC = 36.7m, Medicines = null, DoctorVisit = "Mjesečna kontrola kod pedijatra" },
                new HealthEntry { Id = 5003, BabyId = 2, EntryDate = new DateTime(2026, 9, 6), TemperatureC = 36.6m, Medicines = null, DoctorVisit = null },
                new HealthEntry { Id = 5004, BabyId = 2, EntryDate = new DateTime(2026, 9, 1), TemperatureC = 36.8m, Medicines = "Vitamin D kapi", DoctorVisit = "Redovna kontrola kod pedijatra" },
                new HealthEntry { Id = 5005, BabyId = 3, EntryDate = new DateTime(2026, 9, 6), TemperatureC = 36.6m, Medicines = null, DoctorVisit = null },
                new HealthEntry { Id = 5006, BabyId = 3, EntryDate = new DateTime(2026, 9, 1), TemperatureC = 36.8m, Medicines = "Vitamin D kapi", DoctorVisit = "Redovna kontrola kod pedijatra" },
                new HealthEntry { Id = 5007, BabyId = 4, EntryDate = new DateTime(2026, 9, 6), TemperatureC = 36.6m, Medicines = null, DoctorVisit = null },
                new HealthEntry { Id = 5008, BabyId = 4, EntryDate = new DateTime(2026, 9, 1), TemperatureC = 36.8m, Medicines = "Vitamin D kapi", DoctorVisit = "Redovna kontrola kod pedijatra" },
                new HealthEntry { Id = 5009, BabyId = 5, EntryDate = new DateTime(2026, 9, 6), TemperatureC = 36.6m, Medicines = null, DoctorVisit = null },
                new HealthEntry { Id = 5010, BabyId = 5, EntryDate = new DateTime(2026, 9, 1), TemperatureC = 36.8m, Medicines = "Vitamin D kapi", DoctorVisit = "Redovna kontrola kod pedijatra" },
                new HealthEntry { Id = 5011, BabyId = 6, EntryDate = new DateTime(2026, 9, 6), TemperatureC = 36.6m, Medicines = null, DoctorVisit = null },
                new HealthEntry { Id = 5012, BabyId = 6, EntryDate = new DateTime(2026, 9, 1), TemperatureC = 36.8m, Medicines = "Vitamin D kapi", DoctorVisit = "Redovna kontrola kod pedijatra" },
                new HealthEntry { Id = 5013, BabyId = 7, EntryDate = new DateTime(2026, 9, 6), TemperatureC = 36.6m, Medicines = null, DoctorVisit = null },
                new HealthEntry { Id = 5014, BabyId = 7, EntryDate = new DateTime(2026, 9, 1), TemperatureC = 36.8m, Medicines = "Vitamin D kapi", DoctorVisit = "Redovna kontrola kod pedijatra" }
            );
        }
    }
}
