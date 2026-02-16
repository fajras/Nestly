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
        public short? WeekNumber { get; set; }
        public string? AdviceText { get; set; }
    }

    public class CreateWeeklyAdviceDto
    {
        public short WeekNumber { get; set; }
        public string AdviceText { get; set; } = string.Empty;
    }
}
