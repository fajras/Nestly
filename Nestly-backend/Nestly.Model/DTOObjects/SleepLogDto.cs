using System;
using System.ComponentModel.DataAnnotations;

namespace Nestly.Model.DTOObjects
{
    public class CreateSleepLogDto
    {
        [Range(1, long.MaxValue)]
        public long BabyId { get; set; }
        [Required]
        public DateTime SleepDate { get; set; }
        [Required]
        public string StartTime { get; set; } = default!;
        [Required]
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
        public int Page { get; set; } = 1;

        private int _pageSize = 20;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > 100 ? 100 : value;
        }
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
