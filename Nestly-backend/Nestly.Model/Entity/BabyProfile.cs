using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        public ICollection<BabyGrowth> Growths { get; set; } = new List<BabyGrowth>();
        public ICollection<FeedingLog> FeedingLogs { get; set; } = new List<FeedingLog>();
        public ICollection<SleepLog> SleepLogs { get; set; } = new List<SleepLog>();
        public ICollection<MealPlan> MealPlans { get; set; } = new List<MealPlan>();
        public ICollection<DiaperLog> DiaperLogs { get; set; } = new List<DiaperLog>();
        public ICollection<Milestone> Milestones { get; set; } = new List<Milestone>();
        public ICollection<HealthEntry> HealthEntries { get; set; } = new List<HealthEntry>();
        public ICollection<CalendarEvent> CalendarEvents { get; set; } = new List<CalendarEvent>();
    }
}
