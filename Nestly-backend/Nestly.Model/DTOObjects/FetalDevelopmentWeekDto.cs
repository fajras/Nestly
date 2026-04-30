using System.ComponentModel.DataAnnotations;

namespace Nestly.Model.DTOObjects
{
    public class CreateFetalDevelopmentWeekDto
    {
        [Range(1, short.MaxValue)]
        public short WeekNumber { get; set; }
        public string? ImageUrl { get; set; }
        [Required]
        [MaxLength(5000)]
        public string BabyDevelopment { get; set; }
        [Required]
        [MaxLength(5000)]
        public string MotherChanges { get; set; }
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
