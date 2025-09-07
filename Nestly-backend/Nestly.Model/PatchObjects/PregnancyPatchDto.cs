using System;

namespace Nestly.Model.PatchObjects
{
    public class PregnancyPatchDto
    {
        public long? UserId { get; set; }
        public DateTime? LmpDate { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
