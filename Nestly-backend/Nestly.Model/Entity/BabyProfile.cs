using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Nestly.Model.Entity
{
    public class BabyProfile
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey(nameof(ParentProfile))]
        public long ParentProfileId { get; set; }
        public string BabyName { get; set; }
        public string Gender { get; set; }
        public DateTime BirthDate { get; set; }
        [ForeignKey(nameof(Pregnancy))]
        public long? PregnancyId { get; set; }
        public ParentProfile ParentProfile { get; set; }
        public Pregnancy? Pregnancy { get; set; }

        [JsonIgnore]
        public ICollection<BabyGrowth> Growths { get; set; } = new List<BabyGrowth>();
        [JsonIgnore]
        public ICollection<FeedingLog> FeedingLogs { get; set; } = new List<FeedingLog>();
        [JsonIgnore]
        public ICollection<SleepLog> SleepLogs { get; set; } = new List<SleepLog>();
        [JsonIgnore]
        public ICollection<MealPlan> MealPlans { get; set; } = new List<MealPlan>();
        [JsonIgnore]
        public ICollection<DiaperLog> DiaperLogs { get; set; } = new List<DiaperLog>();
        [JsonIgnore]
        public ICollection<Milestone> Milestones { get; set; } = new List<Milestone>();
        [JsonIgnore]
        public ICollection<HealthEntry> HealthEntries { get; set; } = new List<HealthEntry>();
        [JsonIgnore]
        public ICollection<CalendarEvent> CalendarEvents { get; set; } = new List<CalendarEvent>();
    }
}
