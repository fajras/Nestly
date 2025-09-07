using System;

namespace Nestly.Model.DTOObjects
{
    public class BabyProfilePatchDto
    {
        public string? BabyName { get; set; }
        public string? Gender { get; set; }
        public DateTime? BirthDate { get; set; }
    }
    public class BabyProfileSearchObject
    {
        public long? UserId { get; set; }
        public string? BabyName { get; set; }
        public string? Gender { get; set; }
        public DateTime? BirthDateFrom { get; set; }
        public DateTime? BirthDateTo { get; set; }
    }
    public class CreateBabyProfileDto
    {
        public long ParentProfileId { get; set; }
        public string BabyName { get; set; } = default!;
        public string Gender { get; set; } = default!;
        public DateTime BirthDate { get; set; }
        public long? PregnancyId { get; set; }
    }


}
