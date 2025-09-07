using System;

namespace Nestly.Model.DTOObjects
{
    public class FeedingLogPatchDto
    {
        public DateTime? FeedDate { get; set; }
        public TimeSpan? FeedTime { get; set; }
        public decimal? AmountMl { get; set; }
        public long? FoodTypeId { get; set; }
        public string? Notes { get; set; }
    }
    public class CreateFeedingLogDto
    {
        public long BabyId { get; set; }
        public DateTime FeedDate { get; set; }
        public TimeSpan FeedTime { get; set; }
        public decimal? AmountMl { get; set; }
        public long? FoodTypeId { get; set; }
        public string? Notes { get; set; }
    }
    public class FeedingLogSearchObject
    {
        public long? BabyId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}
