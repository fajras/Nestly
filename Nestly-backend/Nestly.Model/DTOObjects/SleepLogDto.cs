using System;

namespace Nestly.Model.DTOObjects
{
    public class CreateSleepLogDto
    {
        public long BabyId { get; set; }
        public DateTime SleepDate { get; set; }

        public string StartTime { get; set; } = default!;
        public string EndTime { get; set; } = default!;
    }

    public class SleepLogPatchDto
    {
        public DateTime? SleepDate { get; set; }
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }
    }

    public class SleepLogSearchObject
    {
        public long? BabyId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }

    public class SleepLogResponseDto
    {
        public long Id { get; set; }
        public long BabyId { get; set; }
        public DateTime SleepDate { get; set; }

        public string StartTime { get; set; } = default!;
        public string EndTime { get; set; } = default!;

        public int DurationMinutes { get; set; }
    }
}
