using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nestly.Model.Entity
{
    public class DiaperLog
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey(nameof(BabyProfile))]
        public long BabyId { get; set; }
        public DateTime ChangeDate { get; set; }
        public TimeSpan ChangeTime { get; set; }
        public string DiaperState { get; set; }
        public string Notes { get; set; }

        public BabyProfile Baby { get; set; }
    }
}
