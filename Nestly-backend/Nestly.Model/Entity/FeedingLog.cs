using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nestly.Model.Entity
{
    public class FeedingLog
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey(nameof(BabyProfile))]
        public long BabyId { get; set; }
        public DateTime FeedDate { get; set; }
        public TimeSpan FeedTime { get; set; }
        public decimal? AmountMl { get; set; }
        [ForeignKey(nameof(FoodType))]
        public int? FoodTypeId { get; set; }
        public string Notes { get; set; }

        public BabyProfile Baby { get; set; }
        public FoodType FoodType { get; set; }
    }
}
