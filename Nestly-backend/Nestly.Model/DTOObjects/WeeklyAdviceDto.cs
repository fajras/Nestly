using System.ComponentModel.DataAnnotations;

namespace Nestly.Model.DTOObjects
{
    public class WeeklyAdviceResponseDto
    {
        public int Id { get; set; }
        public short WeekNumber { get; set; }
        public string AdviceText { get; set; } = string.Empty;
    }

    public class WeeklyAdvicePatchDto
    {
        [Range(1, short.MaxValue)]
        public short? WeekNumber { get; set; }
        [MaxLength(4000)]
        public string? AdviceText { get; set; }
    }

    public class CreateWeeklyAdviceDto
    {
        [Range(1, short.MaxValue)]
        public short WeekNumber { get; set; }
        [Required]
        [MaxLength(4000)]
        public string AdviceText { get; set; } = string.Empty;
    }
    public class WeeklyAdviceSearchObject
    {
        public int Page { get; set; } = 1;

        private int _pageSize = 50;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > 100 ? 100 : value;
        }

        public short? WeekNumber { get; set; }
    }
}
