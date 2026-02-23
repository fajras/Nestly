using System;
using System.ComponentModel.DataAnnotations;

namespace Nestly.Model.Entity
{
    public class RecommendationModelState
    {
        [Key]
        public int Id { get; set; }
        public string WeightsJson { get; set; } = default!;
        public DateTime UpdatedAt { get; set; }
    }

}
