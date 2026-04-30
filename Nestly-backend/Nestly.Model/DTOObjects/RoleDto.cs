using System.ComponentModel.DataAnnotations;

namespace Nestly.Model.DTOObjects
{
    public class RoleDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = default!;
    }

    public class RoleInsertDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = default!;
    }

    public class RoleUpdateDto
    {
        public string Name { get; set; } = default!;
    }

    public class RoleSearchObject
    {
        public string? Name { get; set; }
        public int Page { get; set; } = 1;

        private int _pageSize = 20;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > 100 ? 100 : value;
        }
    }
}
