using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nestly.Model.Entity;

namespace Nestly.Services.Database.Seeders
{
    public static class RoleSeeder
    {
        public static void SeedData(this EntityTypeBuilder<Role> entity)
        {
            entity.HasData(
                new Role
                {
                    Id = 1,
                    Name = "Parent"
                },
                new Role
                {
                    Id = 2,
                    Name = "Doctor"
                }
            );
        }
    }
}
