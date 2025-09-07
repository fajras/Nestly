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
        [ForeignKey(nameof(AppUser))]
        public long UserId { get; set; }

        public string BabyName { get; set; }

        public string Gender { get; set; }

        public DateTime BirthDate { get; set; }

        public DateTime CreatedAt { get; set; }

        public AppUser User { get; set; }
        public ICollection<BabyGrowth> Growths { get; set; }
        public ICollection<FeedingLog> FeedingLogs { get; set; }
        public ICollection<SleepLog> SleepLogs { get; set; }
        public ICollection<MealPlan> MealPlans { get; set; }
        public ICollection<DiaperLog> DiaperLogs { get; set; }
        public ICollection<Milestone> Milestones { get; set; }
        public ICollection<HealthEntry> HealthEntries { get; set; }
        public ICollection<CalendarEvent> CalendarEvents { get; set; }
    }
}
