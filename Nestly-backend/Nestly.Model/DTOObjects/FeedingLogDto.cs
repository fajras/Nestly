using System;
using System.ComponentModel.DataAnnotations;

namespace Nestly.Model.DTOObjects
{
    public class FeedingLogPatchDto
    {
        public DateTime? FeedDate { get; set; }
        public TimeSpan? FeedTime { get; set; }
        public decimal? AmountMl { get; set; }
        public int? FoodTypeId { get; set; }
        [MaxLength(1000)]
        public string? Notes { get; set; }
    }
    public class CreateFeedingLogDto
    {
        [Range(1, long.MaxValue)]
        public long BabyId { get; set; }
        [Required]
        public DateTime FeedDate { get; set; }
        [Required]
        public TimeSpan FeedTime { get; set; }
        public decimal? AmountMl { get; set; }
        public int? FoodTypeId { get; set; }
        [MaxLength(1000)]
        public string? Notes { get; set; }
    }
    public class FeedingLogSearchObject
    {
        public long? BabyId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
    public class FeedingLogResponseDto
    {
        public long Id { get; set; }
        public long BabyId { get; set; }
        public DateTime FeedDate { get; set; }
        public TimeSpan FeedTime { get; set; }
        public decimal? AmountMl { get; set; }
        public int? FoodTypeId { get; set; }
        public string? FoodTypeName { get; set; }
        public string? Notes { get; set; }
    }

}
