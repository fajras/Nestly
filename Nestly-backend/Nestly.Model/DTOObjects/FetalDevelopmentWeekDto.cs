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
    public class FetalDevelopmentWeekSearchObject
    {
        public short? WeekNumber { get; set; }

        public int Page { get; set; } = 1;

        private int _pageSize = 20;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > 100 ? 100 : value;
        }
    }
}
