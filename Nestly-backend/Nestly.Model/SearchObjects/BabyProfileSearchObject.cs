using System;

namespace Nestly.Model.SearchObjects
{
    public class BabyProfileSearchObject
    {
        public long? UserId { get; set; }
        public string? BabyName { get; set; }
        public string? Gender { get; set; }

        public DateTime? BirthDateFrom { get; set; }
        public DateTime? BirthDateTo { get; set; }
    }
}
