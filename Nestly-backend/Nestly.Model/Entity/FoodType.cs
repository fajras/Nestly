using System.ComponentModel.DataAnnotations;

namespace Nestly.Model.Entity
{
    public class FoodType
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
