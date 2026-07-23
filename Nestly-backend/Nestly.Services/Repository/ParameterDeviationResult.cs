using System;
using Nestly.Model.DTOObjects;

namespace Nestly.Services.Repository
{
    /// <summary>
    /// Output of a single WHO-standards evaluator (growth/feeding/sleep/
    /// diaper/fever) before it is persisted as a <see cref="Nestly.Model.Entity.HealthDeviationAlert"/>.
    /// </summary>
    public class ParameterDeviationResult
    {
        public PediatricParameterType ParameterType { get; set; }
        public HealthAlertSeverity Severity { get; set; }
        public string Title { get; set; } = default!;
        public string Message { get; set; } = default!;
        public string Recommendation { get; set; } = default!;
        public DateTime PeriodFrom { get; set; }
        public DateTime PeriodTo { get; set; }
    }
}
