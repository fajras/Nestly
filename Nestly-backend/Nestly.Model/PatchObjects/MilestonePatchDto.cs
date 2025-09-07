using System;

namespace Nestly.Model.PatchObjects
{
    public class MilestonePatchDto
    {
        public string? Title { get; set; }
        public DateTime? AchievedDate { get; set; }
        public string? Notes { get; set; }
    }
}
