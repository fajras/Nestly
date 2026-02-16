namespace Nestly.Model.DTOObjects
{
    public class CreateFetalDevelopmentWeekDto
    {
        public short WeekNumber { get; set; }
        public string? ImageUrl { get; set; }
        public string? BabyDevelopment { get; set; }
        public string? MotherChanges { get; set; }
    }

    public class FetalDevelopmentWeekPatchDto
    {
        public short? WeekNumber { get; set; }
        public string? ImageUrl { get; set; }
        public string? BabyDevelopment { get; set; }
        public string? MotherChanges { get; set; }
    }

    public class FetalDevelopmentWeekResponseDto
    {
        public int Id { get; set; }
        public short WeekNumber { get; set; }
        public string? ImageUrl { get; set; }
        public string? BabyDevelopment { get; set; }
        public string? MotherChanges { get; set; }
    }
}
