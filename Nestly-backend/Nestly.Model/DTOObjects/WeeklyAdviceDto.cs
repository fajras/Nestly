﻿namespace Nestly.Model.DTOObjects
{
    public class WeeklyAdvicePatchDto
    {
        public short? WeekNumber { get; set; }
        public string? AdviceText { get; set; }
    }
    public class CreateWeeklyAdviceDto
    {
        public short WeekNumber { get; set; }
        public string AdviceText { get; set; } = default!;
    }
}
