namespace Nestly.Model.DTOObjects
{
    public class FoodTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
    }

    public class FoodTypeInsertDto
    {
        public string Name { get; set; } = default!;
    }

    public class FoodTypeUpdateDto
    {
        public string Name { get; set; } = default!;
    }

    public class FoodTypeSearchObject
    {
        public string? Name { get; set; }
    }
}
