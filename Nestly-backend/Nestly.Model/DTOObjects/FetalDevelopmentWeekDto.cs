namespace Nestly.Model.DTOObjects
{
    public class FetalDevelopmentWeekPatchDto
    {
        public short? WeekNumber { get; set; }
        public string? ImageUrl { get; set; }
        public string? BabyDevelopment { get; set; }
        public string? MotherChanges { get; set; }
    }
    public class CreateFetalDevelopmentWeekDto
    {
        public short WeekNumber { get; set; }
        public string? ImageUrl { get; set; }
        public string? BabyDevelopment { get; set; }
        public string? MotherChanges { get; set; }
    }
}
