using System;
using System.Collections.Generic;
using System.Text;

namespace Nestly.Model.SearchObjects
{
    public class MealPlanSearchObject
    {
        public long? BabyId { get; set; }
        public short? WeekNumber { get; set; }
        public string? FoodItem { get; set; }
    }
}
