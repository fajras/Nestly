namespace Nestly.Model.DTOObjects
{
    public class MealPlanPatchDto
    {
        public short? WeekNumber { get; set; }
        public string? FoodItem { get; set; }
        public short? FoodRating { get; set; }
    }
    public class CreateMealPlanDto
    {
        public long BabyId { get; set; }
        public short WeekNumber { get; set; }
        public string FoodItem { get; set; } = default!;
        public short? FoodRating { get; set; }
    }
    public class MealPlanSearchObject
    {
        public long? BabyId { get; set; }
        public short? WeekNumber { get; set; }
        public string? FoodItem { get; set; }
    }
}
