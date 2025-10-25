using System;

namespace Nestly.Model.DTOObjects
{
    public class MealPlanSearchObject
    {
        public long? BabyId { get; set; }
        public int? FoodTypeId { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }

    public class CreateMealPlanDto
    {
        public long BabyId { get; set; }
        public int FoodTypeId { get; set; }
        public short? Rating { get; set; }
        public DateTime? TriedAt { get; set; }
    }


    public class MealPlanPatchDto
    {
        public short? Rating { get; set; }
        public DateTime? TriedAt { get; set; }
    }
}
