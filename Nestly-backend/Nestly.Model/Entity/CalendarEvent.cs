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
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartAt { get; set; }
        [JsonIgnore]
        public BabyProfile BabyProfile { get; set; }
        [JsonIgnore]
        public ParentProfile ParentProfile { get; set; }
    }
}
