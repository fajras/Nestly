using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nestly.Model.Entity
{
    public class MealPlan
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey(nameof(BabyProfile))]
        public long BabyId { get; set; }
        public short WeekNumber { get; set; }
        public string FoodItem { get; set; }
        public short? FoodRating { get; set; }
        public DateTime CreatedAt { get; set; }

        public BabyProfile Baby { get; set; }
    }
}
