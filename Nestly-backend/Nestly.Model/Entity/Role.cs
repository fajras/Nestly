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
        [JsonIgnore]
        public ICollection<AppUser> Users { get; set; } = new List<AppUser>();
    }

}
