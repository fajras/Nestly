using System;
using System.ComponentModel.DataAnnotations;

namespace Nestly.Model.DTOObjects
{
    public class CreateSymptomDiaryDto
    {
        [Range(1, long.MaxValue)]
        public long ParentProfileId { get; set; }
        public DateTime? Date { get; set; }
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
    }

    public class SymptomDiaryPatchDto
    {
        public int? Nausea { get; set; }
        public int? Fatigue { get; set; }
        public int? Headache { get; set; }
        public int? Heartburn { get; set; }
        public int? LegSwelling { get; set; }
    }

    public class SymptomDiaryResponseDto
    {
        public long Id { get; set; }
        public long ParentProfileId { get; set; }
        public DateTime Date { get; set; }
        public int? Nausea { get; set; }
        public int? Fatigue { get; set; }
        public int? Headache { get; set; }
        public int? Heartburn { get; set; }
        public int? LegSwelling { get; set; }
    }

    public class SymptomDiarySearchObject
    {
        public long? ParentProfileId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}
