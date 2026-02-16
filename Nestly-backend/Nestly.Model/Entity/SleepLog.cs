// FILE: Nestly.Model/Entity/SleepLog.cs

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Nestly.Model.Entity
{
    public class SleepLog
    {
        [Key]
        public long Id { get; set; }

        [ForeignKey(nameof(BabyProfile))]
        public long BabyId { get; set; }

        public DateTime SleepDate { get; set; }

        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public int DurationMinutes
        {
            get
            {
                if (EndTime >= StartTime)
                {
                    return (int)(EndTime - StartTime).TotalMinutes;
                }

                return (int)((TimeSpan.FromHours(24) - StartTime + EndTime).TotalMinutes);
            }
        }

        [JsonIgnore]
        public BabyProfile Baby { get; set; }
    }
}
