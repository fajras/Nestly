using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nestly.Model.DTOObjects
{
    public class MedicationPlanSearchObject
    {
        public long? ParentProfileId { get; set; }
        public string? MedicineNameContains { get; set; }
        public DateTime? StartDateFrom { get; set; }
        public DateTime? StartDateTo { get; set; }
        public DateTime? EndDateFrom { get; set; }
        public DateTime? EndDateTo { get; set; }
    }

    public class CreateMedicationPlanDto
    {
        [Range(1, long.MaxValue)]
        public long ParentProfileId { get; set; }

        [Required]
        public string MedicineName { get; set; } = default!;

        [Required]
        public string Dose { get; set; } = default!;

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [MinLength(1)]
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
