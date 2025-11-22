using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nestly.Model.Entity
{
    public class SleepLog
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey(nameof(BabyProfile))]
        public long BabyId { get; set; }

        public DateTime SleepDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public int DurationMinutes => (int)(EndTime - StartTime).TotalMinutes;

        public string Notes { get; set; }

        public BabyProfile Baby { get; set; }
    }
}
