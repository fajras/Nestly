using System;

namespace Nestly.Model.DTOObjects
{
    public enum PediatricParameterType
    {
        Growth = 1,
        Feeding = 2,
        Sleep = 3,
        Diaper = 4,
        Fever = 5
    }

    public enum HealthAlertSeverity
    {
        Info = 1,
        Warning = 2,
        Critical = 3
    }

    public class HealthDeviationAlertResponseDto
    {
        public long Id { get; set; }
        public long BabyId { get; set; }
        public string? BabyName { get; set; }
        public PediatricParameterType ParameterType { get; set; }
        public HealthAlertSeverity Severity { get; set; }
        public string Title { get; set; } = default!;
        public string Message { get; set; } = default!;
        public string Recommendation { get; set; } = default!;
        public DateTime DetectedAt { get; set; }
        public DateTime PeriodFrom { get; set; }
        public DateTime PeriodTo { get; set; }
        public bool IsResolved { get; set; }
        public DateTime? ResolvedAt { get; set; }
    }

    public class HealthDeviationAlertSearchObject
    {
        public long? BabyId { get; set; }
        public bool? IsResolved { get; set; }

        public int Page { get; set; } = 1;

        private int _pageSize = 20;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > 100 ? 100 : value;
        }
    }
}
