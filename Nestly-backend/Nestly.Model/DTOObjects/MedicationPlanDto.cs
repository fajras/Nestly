using System;

namespace Nestly.Model.DTOObjects
{
    public class MedicationPlanSearchObject
    {
        public long? UserId { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }

    public class CreateMedicationPlanDto
    {
        public long UserId { get; set; }
        public string MedicineName { get; set; } = default!;
        public string Dose { get; set; } = default!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class MedicationPlanPatchDto
    {
        public string? MedicineName { get; set; }
        public string? Dose { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class MedicationPlanResponseDto
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string MedicineName { get; set; } = default!;
        public string Dose { get; set; } = default!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
