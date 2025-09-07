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
        [ForeignKey(nameof(ParentProfile))]
        public long? AskedById { get; set; }
        public DateTime CreatedAt { get; set; }

        public ParentProfile AskedBy { get; set; }
        public ICollection<QaAnswer> Answers { get; set; } = new List<QaAnswer>();
    }
}
