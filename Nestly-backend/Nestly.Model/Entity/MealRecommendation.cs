using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nestly.Model.Entity
{
    public class MealRecommendation
    {
        [Key] public long Id { get; set; }
        public short WeekNumber { get; set; }

        [ForeignKey(nameof(FoodType))]
        public int FoodTypeId { get; set; }

        public FoodType FoodType { get; set; } = default!;
    }
}
