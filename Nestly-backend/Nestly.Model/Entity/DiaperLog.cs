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
        [Required, MaxLength(30)]
        public string DiaperState { get; set; } = default!;
        public string? Notes { get; set; }
        public BabyProfile Baby { get; set; }
    }
}
