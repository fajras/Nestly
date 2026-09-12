using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Nestly.Model.Entity
{
    public class Role
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; } = default!;
        // Explicit flag for roles seeded by RoleSeeder (Parent/Doctor),
        // replacing the previous "id == 1 || id == 2" magic-number check.
        public bool IsSystemRole { get; set; }
        [JsonIgnore]
        public ICollection<AppUser> Users { get; set; } = new List<AppUser>();
    }

}
