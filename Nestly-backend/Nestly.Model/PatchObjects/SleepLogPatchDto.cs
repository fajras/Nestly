using System;

namespace Nestly.Model.PatchObjects
{
    public class SleepLogPatchDto
    {
        public DateTime? SleepDate { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? Notes { get; set; }
    }
}
