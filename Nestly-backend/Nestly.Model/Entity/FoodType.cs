using System.ComponentModel.DataAnnotations;

namespace Nestly.Model.Entity
{
    public class FoodType
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(120)]
        public string Name { get; set; } = default!;
    }
}
