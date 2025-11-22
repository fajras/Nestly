using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nestly.Model.Entity
{
    public class SymptomDiary
    {
        [Key]
        public long Id { get; set; }

        [ForeignKey(nameof(ParentProfile))]
        public long ParentProfileId { get; set; }

        public DateTime Date { get; set; } = DateTime.UtcNow.Date;

        [Range(1, 5)]
        public int? Nausea { get; set; }

        [Range(1, 5)]
        public int? Fatigue { get; set; }

        [Range(1, 5)]
        public int? Headache { get; set; }

        [Range(1, 5)]
        public int? Heartburn { get; set; }

        [Range(1, 5)]
        public int? LegSwelling { get; set; }

        public ParentProfile ParentProfile { get; set; } = default!;
    }
}
