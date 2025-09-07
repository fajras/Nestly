using System;

namespace Nestly.Model.DTOObjects
{
    public class SleepLogPatchDto
    {
        public DateTime? SleepDate { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? Notes { get; set; }
    }
    public class CreateSleepLogDto
    {
        public long BabyId { get; set; }
        public DateTime SleepDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string? Notes { get; set; }
    }
    public class SleepLogSearchObject
    {
        public long? BabyId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}
