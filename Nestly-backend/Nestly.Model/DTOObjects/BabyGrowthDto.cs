namespace Nestly.Model.DTOObjects
{
    public class BabyGrowthPatchDto
    {
        public short? WeekNumber { get; set; }
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
    }
    public class CreateBabyGrowthDto
    {
        public long BabyId { get; set; }
        public short WeekNumber { get; set; }
        public decimal? WeightKg { get; set; }
        public decimal? HeightCm { get; set; }
        public decimal? HeadCircumferenceCm { get; set; }
    }
}
