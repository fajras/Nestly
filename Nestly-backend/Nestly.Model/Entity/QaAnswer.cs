using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nestly.Model.Entity
{
    public class QaAnswer
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey(nameof(QaQuestion))]
        public long QuestionId { get; set; }
        public string AnswerText { get; set; }
        [ForeignKey(nameof(DoctorProfile))]
        public long? AnsweredById { get; set; }
        public DateTime CreatedAt { get; set; }

        public QaQuestion Question { get; set; }
        public DoctorProfile AnsweredBy { get; set; }
    }
}
