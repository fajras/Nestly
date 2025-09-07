using System;

namespace Nestly.Model.PatchObjects
{
    public class CalendarEventPatchDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime? StartAt { get; set; }
        public DateTime? EndAt { get; set; }
    }
}
