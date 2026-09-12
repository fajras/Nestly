using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Nestly.Model.DTOObjects;

namespace Nestly.Model.Entity
{
    public class HealthDeviationAlert
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey(nameof(Baby))]
        public long BabyId { get; set; }
        public PediatricParameterType ParameterType { get; set; }
        public HealthAlertSeverity Severity { get; set; }
        [Required, MaxLength(200)]
        public string Title { get; set; } = default!;
        [Required, MaxLength(1000)]
        public string Message { get; set; } = default!;
        [Required, MaxLength(1000)]
        public string Recommendation { get; set; } = default!;
        public DateTime DetectedAt { get; set; }
        public DateTime PeriodFrom { get; set; }
        public DateTime PeriodTo { get; set; }
        public bool IsResolved { get; set; }
        public DateTime? ResolvedAt { get; set; }

        // Human-in-the-loop feedback: lets a doctor confirm or dispute the
        // accuracy of a ML-generated alert. Collected explicitly rather than
        // inferred, so it can later be used to evaluate/retrain the
        // deviation-detection model against real clinical judgement.
        public bool? DoctorFeedbackIsAccurate { get; set; }
        [MaxLength(1000)]
        public string? DoctorFeedbackComment { get; set; }
        [ForeignKey(nameof(FeedbackByDoctor))]
        public long? DoctorFeedbackByDoctorId { get; set; }
        public DateTime? DoctorFeedbackAt { get; set; }

        [JsonIgnore]
        public DoctorProfile? FeedbackByDoctor { get; set; }

        [JsonIgnore]
        public BabyProfile Baby { get; set; }
    }
}
