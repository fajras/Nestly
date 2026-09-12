using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nestly.Model.Entity;

namespace Nestly.Services.Data.Seeders
{
    public static class MedicationPlanSeeder
    {
        public static void SeedData(this EntityTypeBuilder<MedicationPlan> entity)
        {
            entity.HasData(
                new MedicationPlan { Id = 9001, ParentProfileId = 1, MedicineName = "Prenatalni vitamini (Vitamin D + Folna kiselina)", Dose = "1 tableta", StartDate = new DateTime(2025, 6, 3), EndDate = new DateTime(2026, 3, 10) },
                new MedicationPlan { Id = 9002, ParentProfileId = 1, MedicineName = "Željezo (Fe) dodatak", Dose = "1 tableta", StartDate = new DateTime(2025, 12, 16), EndDate = new DateTime(2026, 3, 10) },
                new MedicationPlan { Id = 5001, ParentProfileId = 2, MedicineName = "Prenatalni vitamini (Vitamin D + Folna kiselina)", Dose = "1 tableta", StartDate = new DateTime(2026, 8, 18), EndDate = new DateTime(2026, 12, 16) },
                new MedicationPlan { Id = 5002, ParentProfileId = 3, MedicineName = "Prenatalni vitamini (Vitamin D + Folna kiselina)", Dose = "1 tableta", StartDate = new DateTime(2026, 8, 18), EndDate = new DateTime(2026, 12, 16) },
                new MedicationPlan { Id = 5003, ParentProfileId = 4, MedicineName = "Prenatalni vitamini (Vitamin D + Folna kiselina)", Dose = "1 tableta", StartDate = new DateTime(2026, 8, 18), EndDate = new DateTime(2026, 12, 16) },
                new MedicationPlan { Id = 5004, ParentProfileId = 5, MedicineName = "Prenatalni vitamini (Vitamin D + Folna kiselina)", Dose = "1 tableta", StartDate = new DateTime(2026, 8, 18), EndDate = new DateTime(2026, 12, 16) },
                new MedicationPlan { Id = 5005, ParentProfileId = 11, MedicineName = "Prenatalni vitamini (Vitamin D + Folna kiselina)", Dose = "1 tableta", StartDate = new DateTime(2026, 8, 18), EndDate = new DateTime(2026, 12, 16) }
            );
        }
    }
}
