using System;

namespace Nestly.Model.DTOObjects
{
    public class HealthEntryPatchDto
    {
        public DateTime? EntryDate { get; set; }
        public decimal? TemperatureC { get; set; }
        public string? Medicines { get; set; }
        public string? DoctorVisit { get; set; }
    }
    public class CreateHealthEntryDto
    {
        public long BabyId { get; set; }
        public DateTime EntryDate { get; set; }
        public decimal? TemperatureC { get; set; }
        public string? Medicines { get; set; }
        public string? DoctorVisit { get; set; }
    }
    public class HealthEntrySearchObject
    {
        public long? BabyId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}
