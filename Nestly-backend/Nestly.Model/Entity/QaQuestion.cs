using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nestly.Model.Entity
{
    public class QaQuestion
    {
        [Key]
        public long Id { get; set; }
        public string QuestionText { get; set; }
        [ForeignKey(nameof(AppUser))]
        public long? AskedByUserId { get; set; }
        public DateTime CreatedAt { get; set; }

        public AppUser AskedBy { get; set; }
        public ICollection<QaAnswer> Answers { get; set; }
    }
}
