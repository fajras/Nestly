using System;

namespace Nestly.Model.SearchObjects
{
    public class DiaperLogSearchObject
    {
        public long? BabyId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string? DiaperState { get; set; }
    }
}
