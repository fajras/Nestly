using System;

namespace Nestly.Model.DTOObjects
{
    public class CreateSymptomDiaryDto
    {
        public long ParentProfileId { get; set; }
        public DateTime? Date { get; set; }
        public int? Nausea { get; set; }
        public int? Fatigue { get; set; }
        public int? Headache { get; set; }
        public int? Heartburn { get; set; }
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
}
