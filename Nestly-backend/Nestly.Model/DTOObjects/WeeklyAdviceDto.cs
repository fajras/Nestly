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
}
