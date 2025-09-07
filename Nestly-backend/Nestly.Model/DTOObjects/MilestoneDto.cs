using System;

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
        public long BabyId { get; set; }
        public string Title { get; set; }
        public DateTime AchievedDate { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
