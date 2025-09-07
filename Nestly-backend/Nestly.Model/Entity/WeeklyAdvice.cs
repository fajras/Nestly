using System.ComponentModel.DataAnnotations;

namespace Nestly.Model.Entity
{
    public class WeeklyAdvice
    {
        [Key]
        public int Id { get; set; }
        public short WeekNumber { get; set; }
        public string AdviceText { get; set; }
    }
}
