using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Nestly.Model.Entity
{
    public class CalendarEvent
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey(nameof(BabyProfile))]
        public long BabyId { get; set; }
        [ForeignKey(nameof(ParentProfile))]
        public long? UserId { get; set; }
        [Required, MaxLength(200)]
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        public DateTime StartAt { get; set; }
        public bool Reminder24hSent { get; set; } = false;
        [JsonIgnore]
        public BabyProfile BabyProfile { get; set; }
        [JsonIgnore]
        public ParentProfile ParentProfile { get; set; }
    }
}
