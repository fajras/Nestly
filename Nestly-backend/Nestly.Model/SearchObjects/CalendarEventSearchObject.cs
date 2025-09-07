using System;

namespace Nestly.Model.SearchObjects
{
    public class CalendarEventSearchObject
    {
        public long? BabyId { get; set; }
        public long? UserId { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public string? Title { get; set; }
    }
}
