namespace Nestly.Model.PatchObjects
{
    public class BabyGrowthPatchDto
    {
        public short? WeekNumber { get; set; }
        public decimal? WeightKg { get; set; }
        public decimal? HeightCm { get; set; }
        public decimal? HeadCircumferenceCm { get; set; }
    }
}
