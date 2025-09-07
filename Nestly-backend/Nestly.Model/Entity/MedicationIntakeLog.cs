using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nestly.Model.Entity
{
    public class MedicationIntakeLog
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey(nameof(MedicationPlan))]
        public long PlanId { get; set; }
        public DateTime ScheduledDate { get; set; }
        public TimeSpan IntakeTime { get; set; }
        public bool Taken { get; set; }
        public DateTime? TakenAt { get; set; }

        public MedicationPlan Plan { get; set; }
    }

}
