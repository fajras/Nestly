using System;

namespace Nestly.Model.PatchObjects
{
    public class DiaperLogPatchDto
    {
        public DateTime? ChangeDate { get; set; }
        public TimeSpan? ChangeTime { get; set; }
        public string? DiaperState { get; set; }
        public string? Notes { get; set; }
    }
}
