using System;

namespace Nestly.Model.DTOObjects
{
    public class CalendarEventPatchDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime? StartAt { get; set; }
        public DateTime? EndAt { get; set; }
    }
    public class CreateCalendarEventDto
    {
        public long BabyId { get; set; }
        public long? UserId { get; set; }
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime? EndAt { get; set; }
    }
    public class CalendarEventSearchObject
    {
        public long? BabyId { get; set; }
        public long? UserId { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public string? Title { get; set; }
    }
}
