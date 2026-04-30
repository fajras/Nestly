using System;
using System.ComponentModel.DataAnnotations;

namespace Nestly.Model.DTOObjects
{
    public class MilestonePatchDto
    {
        public string? Title { get; set; }
        public DateTime? AchievedDate { get; set; }
        public string? Notes { get; set; }
    }
    public class MilestoneSearchObject
    {
        public long? BabyId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string? Title { get; set; }
    }

    public class CreateMilestoneDto
    {
        [Range(1, long.MaxValue)]
        public long BabyId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public DateTime AchievedDate { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }

    }
    public class MilestoneResponseDto
    {
        public long Id { get; set; }
        public long BabyId { get; set; }
        public string Title { get; set; } = default!;
        public DateTime AchievedDate { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
