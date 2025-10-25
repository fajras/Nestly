using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nestly.Model.Entity
{
    public class Pregnancy
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey(nameof(ParentProfile))]
        public long UserId { get; set; }
        public DateTime? LmpDate { get; set; }
        public DateTime? DueDate { get; set; }
        public int? CycleLengthDays { get; set; }
        public ParentProfile User { get; set; }
    }
}
