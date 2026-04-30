using System.ComponentModel.DataAnnotations;

namespace Nestly.Model.Entity
{
    public class WeeklyAdvice
    {
        [Key]
        public int Id { get; set; }
        public short WeekNumber { get; set; }
        [Required, MaxLength(4000)]
        public string AdviceText { get; set; } = default!;
    }
}
