using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nestly.Model.Entity
{
    public class BabyGrowth
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey(nameof(BabyProfile))]
        public long BabyId { get; set; }
        public short WeekNumber { get; set; }
        public decimal? WeightKg { get; set; }
        public decimal? HeightCm { get; set; }
        public decimal? HeadCircumferenceCm { get; set; }

        public BabyProfile Baby { get; set; }
    }

}
