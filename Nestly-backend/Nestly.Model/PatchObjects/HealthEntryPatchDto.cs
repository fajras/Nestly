using System;

namespace Nestly.Model.PatchObjects
{
    public class HealthEntryPatchDto
    {
        public DateTime? EntryDate { get; set; }
        public decimal? TemperatureC { get; set; }
        public string? Medicines { get; set; }
        public string? DoctorVisit { get; set; }
    }
}
