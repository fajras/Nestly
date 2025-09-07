namespace Nestly.Model.SearchObjects
{
    public class BabyGrowthSearchObject
    {
        public long? BabyId { get; set; }
        public short? WeekNumber { get; set; }

        public short? WeekFrom { get; set; }
        public short? WeekTo { get; set; }
    }
}
