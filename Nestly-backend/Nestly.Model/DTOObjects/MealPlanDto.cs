using System;
using System.ComponentModel.DataAnnotations;

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
        [Range(1, long.MaxValue)]
        public long BabyId { get; set; }
        [Range(1, int.MaxValue)]
        public int FoodTypeId { get; set; }
        [Range(1, 5)]
        public short? Rating { get; set; }
        public DateTime? TriedAt { get; set; }
    }


    public class MealPlanPatchDto
    {
        public short? Rating { get; set; }
        public DateTime? TriedAt { get; set; }
    }

    public class MealRecommendationDto
    {
        public long Id { get; set; }
        [Range(1, short.MaxValue)]
        public short WeekNumber { get; set; }
        [Range(1, int.MaxValue)]
        public int FoodTypeId { get; set; }
        public string FoodName { get; set; } = string.Empty;
    }

    public class MealRecommendationSearchObject
    {
        public short? WeekNumber { get; set; }
    }
    public class MealPlanResponseDto
    {
        public long Id { get; set; }
        public long BabyId { get; set; }
        public int FoodTypeId { get; set; }
        public string FoodName { get; set; } = string.Empty;
        public short? Rating { get; set; }
        public DateTime TriedAt { get; set; }
    }
    public class CreateMealRecommendationDto
    {
        public short WeekNumber { get; set; }
        public int FoodTypeId { get; set; }
    }

}
