using System.ComponentModel.DataAnnotations;

namespace Nestly.Model.DTOObjects
{
    public class BabyGrowthResponseDto
    {
        public long Id { get; set; }
        public long BabyId { get; set; }
        public short WeekNumber { get; set; }
        public decimal? WeightKg { get; set; }
        public decimal? HeightCm { get; set; }
        public decimal? HeadCircumferenceCm { get; set; }
    }
    public class BabyGrowthPatchDto
    {
        public decimal? WeightKg { get; set; }
        public decimal? HeightCm { get; set; }
        public decimal? HeadCircumferenceCm { get; set; }
    }
    public class BabyGrowthSearchObject
    {
        public long? BabyId { get; set; }
        public short? WeekNumber { get; set; }
        public short? WeekFrom { get; set; }
        public short? WeekTo { get; set; }

        public int Page { get; set; } = 1;

        private int _pageSize = 20;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > 100 ? 100 : value;
        }
    }
    public class CreateBabyGrowthDto
    {
        [Range(1, long.MaxValue)]
        public long BabyId { get; set; }

        [Range(1, short.MaxValue)]
        public short WeekNumber { get; set; }
        public decimal? WeightKg { get; set; }
        public decimal? HeightCm { get; set; }
        public decimal? HeadCircumferenceCm { get; set; }
    }
}
