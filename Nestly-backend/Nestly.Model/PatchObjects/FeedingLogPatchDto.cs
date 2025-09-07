using System;

namespace Nestly.Model.PatchObjects
{
    public class FeedingLogPatchDto
    {
        public DateTime? FeedDate { get; set; }
        public TimeSpan? FeedTime { get; set; }
        public decimal? AmountMl { get; set; }
        public long? FoodTypeId { get; set; }
        public string? Notes { get; set; }
    }
}
