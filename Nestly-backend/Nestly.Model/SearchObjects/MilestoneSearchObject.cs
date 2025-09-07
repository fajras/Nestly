using System;

namespace Nestly.Model.SearchObjects
{
    public class MilestoneSearchObject
    {
        public long? BabyId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string? Title { get; set; }
    }
}
