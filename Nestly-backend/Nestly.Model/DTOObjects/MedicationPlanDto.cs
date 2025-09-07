using System;

namespace Nestly.Model.DTOObjects
{
    public class MedicationPlanPatchDto
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? MedicineName { get; set; }
        public string? Dose { get; set; }
    }
    public class CreateMedicationPlanDto
    {
        public long UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string MedicineName { get; set; } = default!;
        public string Dose { get; set; } = default!;
    }
    public class MedicationPlanSearchObject
    {
        public long? UserId { get; set; }
        public string? MedicineName { get; set; }
        public DateTime? ActiveOn { get; set; }
    }
    public class CreateMedicationScheduleTimeDto
    {
        public long PlanId { get; set; }
        public TimeSpan IntakeTime { get; set; }
    }
    public class CreateMedicationIntakeLogDto
    {
        public long PlanId { get; set; }
        public DateTime ScheduledDate { get; set; }
        public TimeSpan IntakeTime { get; set; }
        public bool Taken { get; set; }
        public DateTime? TakenAt { get; set; }
    }
}
