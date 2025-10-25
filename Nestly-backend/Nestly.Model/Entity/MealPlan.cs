using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nestly.Model.Entity
{
    public class MealPlan
    {
        [Key] public long Id { get; set; }

        [ForeignKey(nameof(BabyProfile))]
        public long BabyId { get; set; }

        [ForeignKey(nameof(FoodType))]
        public int FoodTypeId { get; set; }

        public short? Rating { get; set; }

        public DateTime TriedAt { get; set; }

        public BabyProfile Baby { get; set; } = default!;
        public FoodType FoodType { get; set; } = default!;
    }
}
