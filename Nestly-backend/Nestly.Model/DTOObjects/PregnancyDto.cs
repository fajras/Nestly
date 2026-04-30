using System;

namespace Nestly.Model.DTOObjects
{
    public class PregnancyPatchDto
    {
        public long? UserId { get; set; }
        public DateTime? LmpDate { get; set; }
        public DateTime? DueDate { get; set; }
        public int? CycleLengthDays { get; set; }
    }
    public class CreatePregnancyDto
    {
        public long UserId { get; set; }
        public DateTime? LmpDate { get; set; }
        public DateTime? DueDate { get; set; }
        public int? CycleLengthDays { get; set; }
    }
    public class PregnancySearchObject
    {
        public long? UserId { get; set; }
        public DateTime? LmpFrom { get; set; }
        public DateTime? LmpTo { get; set; }
        public DateTime? DueFrom { get; set; }
        public DateTime? DueTo { get; set; }
        public int Page { get; set; } = 1;

        private int _pageSize = 20;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > 100 ? 100 : value;
        }
    }
    public class PregnancyStatusDto
    {
        public long ParentProfileId { get; set; }
        public DateTime? LmpDate { get; set; }
        public DateTime? DueDate { get; set; }
        public int GestationalWeek { get; set; }
        public int DaysRemaining { get; set; }
    }
    public class PregnancyResponseDto
    {
        public long Id { get; set; }
        public long ParentProfileId { get; set; }
        public DateTime? LmpDate { get; set; }
        public DateTime? DueDate { get; set; }
        public int? CycleLengthDays { get; set; }
    }

}
