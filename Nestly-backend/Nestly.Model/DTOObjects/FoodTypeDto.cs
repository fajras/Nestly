using System.ComponentModel.DataAnnotations;

namespace Nestly.Model.DTOObjects
{
    public class FoodTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
    }

    public class FoodTypeInsertDto
    {
        [Required]
        [MaxLength(120)]
        public string Name { get; set; } = default!;
    }

    public class FoodTypeUpdateDto
    {
        [Required]
        [MaxLength(120)]
        public string Name { get; set; } = default!;
    }

    public class FoodTypeSearchObject
    {
        public string? Name { get; set; }

        public int Page { get; set; } = 1;

        private int _pageSize = 20;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > 100 ? 100 : value;
        }
    }
}
