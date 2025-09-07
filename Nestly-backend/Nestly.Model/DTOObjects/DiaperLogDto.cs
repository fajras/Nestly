using System;

namespace Nestly.Model.DTOObjects
{
    public class DiaperLogPatchDto
    {
        public DateTime? ChangeDate { get; set; }
        public TimeSpan? ChangeTime { get; set; }
        public string? DiaperState { get; set; }
        public string? Notes { get; set; }
    }
    public class CreateDiaperLogDto
    {
        public long BabyId { get; set; }
        public DateTime ChangeDate { get; set; }
        public TimeSpan ChangeTime { get; set; }
        public string DiaperState { get; set; } = default!;
        public string? Notes { get; set; }
    }
    public class DiaperLogSearchObject
    {
        public long? BabyId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string? DiaperState { get; set; }
    }
}
