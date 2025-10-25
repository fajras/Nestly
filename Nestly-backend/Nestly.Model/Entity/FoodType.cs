using System.ComponentModel.DataAnnotations;

namespace Nestly.Model.Entity
{
    public class FoodType
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
