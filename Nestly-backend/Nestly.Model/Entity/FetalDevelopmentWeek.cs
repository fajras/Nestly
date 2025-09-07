using System.ComponentModel.DataAnnotations;

namespace Nestly.Model.Entity
{
    public class FetalDevelopmentWeek
    {
        [Key]
        public int Id { get; set; }
        public short WeekNumber { get; set; }
        public string ImageUrl { get; set; }
        public string BabyDevelopment { get; set; }
        public string MotherChanges { get; set; }
    }
}
