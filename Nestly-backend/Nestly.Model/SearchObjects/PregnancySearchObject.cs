using System;

namespace Nestly.Model.SearchObjects
{
    public class PregnancySearchObject
    {
        public long? UserId { get; set; }
        public DateTime? LmpFrom { get; set; }
        public DateTime? LmpTo { get; set; }
        public DateTime? DueFrom { get; set; }
        public DateTime? DueTo { get; set; }
    }
}
