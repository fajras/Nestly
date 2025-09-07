using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nestly.Model.Entity
{
    public class Role
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; } = default!;
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }

    public class UserRole
    {
        [ForeignKey(nameof(AppUser))]
        public long UserId { get; set; }
        public AppUser User { get; set; } = default!;
        [ForeignKey(nameof(Role))]
        public long RoleId { get; set; }
        public Role Role { get; set; } = default!;
    }
}
