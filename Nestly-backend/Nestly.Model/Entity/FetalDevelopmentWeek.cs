using System.ComponentModel.DataAnnotations;

namespace Nestly.Model.Entity
{
    public class FetalDevelopmentWeek
    {
        [Key]
        public int Id { get; set; }
        public short WeekNumber { get; set; }
        [Required, MaxLength(500)]
        public string ImageUrl { get; set; }
        [Required, MaxLength(5000)]
        public string BabyDevelopment { get; set; }
        [Required, MaxLength(5000)]
        public string MotherChanges { get; set; }
    }
}
