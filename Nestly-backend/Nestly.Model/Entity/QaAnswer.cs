using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Nestly.Model.Entity
{
    public class QaAnswer
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey(nameof(QaQuestion))]
        public long QuestionId { get; set; }
        [Required]
        public string AnswerText { get; set; } = default!;
        [ForeignKey(nameof(DoctorProfile))]
        public long? AnsweredById { get; set; }
        public DateTime CreatedAt { get; set; }

        [JsonIgnore]
        public QaQuestion Question { get; set; }
        [JsonIgnore]
        public DoctorProfile AnsweredBy { get; set; }
    }
}
