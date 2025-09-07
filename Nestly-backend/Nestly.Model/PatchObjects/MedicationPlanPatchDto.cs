using System;

namespace Nestly.Model.PatchObjects
{
    public class MedicationPlanPatchDto
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? MedicineName { get; set; }
        public string? Dose { get; set; }
    }
}
