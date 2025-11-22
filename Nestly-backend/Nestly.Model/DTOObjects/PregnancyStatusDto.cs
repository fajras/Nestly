using System;

namespace Nestly.Model.DTOObjects
{
    public class PregnancyStatusDto
    {
        public long ParentProfileId { get; set; }

        public DateTime? LmpDate { get; set; }
        public DateTime? DueDate { get; set; }

        public int GestationalWeek { get; set; }
        public int DaysRemaining { get; set; }
    }
}
