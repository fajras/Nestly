using System;
using System.ComponentModel.DataAnnotations;

namespace Nestly.Model.DTOObjects
{
    public class CalendarEventPatchDto
    {
        [MaxLength(200)]
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime? StartAt { get; set; }
    }
    public class CreateCalendarEventDto
    {
        [Range(1, long.MaxValue)]
        public long BabyId { get; set; }
        public long? UserId { get; set; }
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        [Required]
        public DateTime StartAt { get; set; }
    }
    public class CalendarEventSearchObject
    {
        public long? BabyId { get; set; }
        public long? UserId { get; set; }
        public DateTime? From { get; set; }
        public string? Title { get; set; }
    }
    public class CalendarEventResponseDto
    {
        public long Id { get; set; }
        public long BabyId { get; set; }
        public long? UserId { get; set; }
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        public DateTime StartAt { get; set; }
    }

}
