using System;

namespace Nestly.Model.DTOObjects
{
    public class PregnancyPatchDto
    {
        public long? UserId { get; set; }
        public DateTime? LmpDate { get; set; }
        public DateTime? DueDate { get; set; }
    }
    public class CreatePregnancyDto
    {
        public long UserId { get; set; }
        public DateTime? LmpDate { get; set; }
        public DateTime? DueDate { get; set; }
    }
    public class PregnancySearchObject
    {
        public long? UserId { get; set; }
        public DateTime? LmpFrom { get; set; }
        public DateTime? LmpTo { get; set; }
        public DateTime? DueFrom { get; set; }
        public DateTime? DueTo { get; set; }
    }
}
