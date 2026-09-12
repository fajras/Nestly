using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nestly.Model.Entity;

namespace Nestly.Services.Data.Seeders
{
    public static class MedicationScheduleTimeSeeder
    {
        public static void SeedData(this EntityTypeBuilder<MedicationScheduleTime> entity)
        {
            entity.HasData(
                new MedicationScheduleTime { Id = 9001, PlanId = 9001, IntakeTime = new TimeSpan(8, 0, 0) },
                new MedicationScheduleTime { Id = 9002, PlanId = 9002, IntakeTime = new TimeSpan(20, 0, 0) },
                new MedicationScheduleTime { Id = 5001, PlanId = 5001, IntakeTime = new TimeSpan(8, 0, 0) },
                new MedicationScheduleTime { Id = 5002, PlanId = 5002, IntakeTime = new TimeSpan(8, 0, 0) },
                new MedicationScheduleTime { Id = 5003, PlanId = 5003, IntakeTime = new TimeSpan(8, 0, 0) },
                new MedicationScheduleTime { Id = 5004, PlanId = 5004, IntakeTime = new TimeSpan(8, 0, 0) },
                new MedicationScheduleTime { Id = 5005, PlanId = 5005, IntakeTime = new TimeSpan(8, 0, 0) }
            );
        }
    }
}
