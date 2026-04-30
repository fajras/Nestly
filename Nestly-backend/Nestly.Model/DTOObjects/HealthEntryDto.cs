using System;
using System.ComponentModel.DataAnnotations;

namespace Nestly.Model.DTOObjects
{
    public class HealthEntryPatchDto
    {
        public DateTime? EntryDate { get; set; }
        public decimal? TemperatureC { get; set; }
        [MaxLength(1000)]
        public string? Medicines { get; set; }
        [MaxLength(500)]
        public string? DoctorVisit { get; set; }
    }
    public class CreateHealthEntryDto
    {
        [Range(1, long.MaxValue)]
        public long BabyId { get; set; }
        [Required]
        public DateTime EntryDate { get; set; }
        [Range(30, 45)]
        public decimal? TemperatureC { get; set; }
        [MaxLength(1000)]
        public string? Medicines { get; set; }
        [MaxLength(500)]
        public string? DoctorVisit { get; set; }
    }
    public class HealthEntrySearchObject
    {
        public long? BabyId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        public int Page { get; set; } = 1;

        private int _pageSize = 20;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > 100 ? 100 : value;
        }
    }
    public class HealthEntryResponseDto
    {
        public long Id { get; set; }
        public long BabyId { get; set; }
        public DateTime EntryDate { get; set; }
        public decimal? TemperatureC { get; set; }
        public string? Medicines { get; set; }
        public string? DoctorVisit { get; set; }
    }

}
