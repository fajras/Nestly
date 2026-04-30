using System;
using System.ComponentModel.DataAnnotations;

namespace Nestly.Model.DTOObjects
{
    public class BabyProfilePatchDto
    {
        [MaxLength(150)]
        public string? BabyName { get; set; }
        [MaxLength(20)]
        public string? Gender { get; set; }
        public DateTime? BirthDate { get; set; }
    }
    public class BabyProfileSummaryDto
    {
        public long Id { get; set; }
        public string BabyName { get; set; } = default!;
        public string Gender { get; set; } = default!;
        public DateTime BirthDate { get; set; }
    }
    public class BabyProfileSearchObject
    {
        public long? UserId { get; set; }
        public string? BabyName { get; set; }
        public string? Gender { get; set; }
        public DateTime? BirthDateFrom { get; set; }
        public DateTime? BirthDateTo { get; set; }

        public int Page { get; set; } = 1;

        private int _pageSize = 20;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > 100 ? 100 : value;
        }
    }
    public class CreateBabyProfileDto
    {
        [Range(1, long.MaxValue)]
        public long ParentProfileId { get; set; }

        [Required]
        public string BabyName { get; set; } = default!;

        [Required]
        public string Gender { get; set; } = default!;

        [Required]
        public DateTime BirthDate { get; set; }

        public long? PregnancyId { get; set; }
    }


}
