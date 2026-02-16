using System;
using System.Collections.Generic;

namespace Nestly.Model.DTOObjects
{
    public class MedicationPlanSearchObject
    {
        public long? ParentProfileId { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }

    public class CreateMedicationPlanDto
    {
        public long ParentProfileId { get; set; }
        public string MedicineName { get; set; } = default!;
        public string Dose { get; set; } = default!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public List<TimeSpan> IntakeTimes { get; set; } = new();
    }

    public class MedicationPlanResponseDto
    {
        public long Id { get; set; }
        public long ParentProfileId { get; set; }
        public string MedicineName { get; set; } = default!;
        public string Dose { get; set; } = default!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public List<TimeSpan> IntakeTimes { get; set; } = new();
    }

    public class MedicationPlanPatchDto
    {
        public string? MedicineName { get; set; }
        public string? Dose { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class MarkMedicationTakenDto
    {
        public long IntakeLogId { get; set; }
    }
    public class MedicationIntakeLogDto
    {
        public long IntakeLogId { get; set; }
        public long PlanId { get; set; }
        public string MedicineName { get; set; } = default!;
        public string Dose { get; set; } = default!;
        public DateTime ScheduledDate { get; set; }
        public TimeSpan IntakeTime { get; set; }
        public bool Taken { get; set; }
    }

}
