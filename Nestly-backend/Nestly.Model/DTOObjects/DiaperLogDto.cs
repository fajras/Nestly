using System;
using System.ComponentModel.DataAnnotations;

namespace Nestly.Model.DTOObjects
{
    public class DiaperLogPatchDto
    {
        public DateTime? ChangeDate { get; set; }
        public TimeSpan? ChangeTime { get; set; }
        [MaxLength(30)]
        public string? DiaperState { get; set; }
        [MaxLength(1000)]
        public string? Notes { get; set; }
    }
    public class CreateDiaperLogDto
    {
        [Range(1, long.MaxValue)]
        public long BabyId { get; set; }
        [Required]
        public DateTime ChangeDate { get; set; }
        [Required]
        public TimeSpan ChangeTime { get; set; }
        [Required]
        [MaxLength(30)]
        public string DiaperState { get; set; } = default!;
        [MaxLength(1000)]
        public string? Notes { get; set; }
    }
    public class DiaperLogSearchObject
    {
        public long? BabyId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string? DiaperState { get; set; }
    }
    public class DiaperLogResponseDto
    {
        public long Id { get; set; }
        public long BabyId { get; set; }
        public DateTime ChangeDate { get; set; }
        public TimeSpan ChangeTime { get; set; }
        public string DiaperState { get; set; } = default!;
        public string? Notes { get; set; }
    }

}
