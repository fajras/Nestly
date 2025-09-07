using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nestly.Model.Entity
{
    public class Milestone
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey(nameof(BabyProfile))]
        public long BabyId { get; set; }
        public string Title { get; set; }
        public DateTime AchievedDate { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }

        public BabyProfile Baby { get; set; }
    }
}
