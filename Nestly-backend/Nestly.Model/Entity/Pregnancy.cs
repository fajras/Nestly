using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nestly.Model.Entity
{
    public class Pregnancy : ISoftDeletable
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey(nameof(ParentProfile))]
        public long ParentProfileId { get; set; }
        public DateTime? LmpDate { get; set; }
        public DateTime? DueDate { get; set; }
        public int? CycleLengthDays { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public ParentProfile ParentProfile { get; set; }
    }
}
