using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Nestly.Services.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Fixed role IDs
            var parentRoleId = "2b3b5f31-c1d6-4a82-8e1e-2c318c5bc98e";
            var doctorRoleId = "3d4e2bfa-3a12-4df8-9fc5-bd45e2102a3c";

            // Roles
            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = parentRoleId,
                    Name = "Parent",
                    NormalizedName = "PARENT",
                    ConcurrencyStamp = parentRoleId
                },
                new IdentityRole
                {
                    Id = doctorRoleId,
                    Name = "Doctor",
                    NormalizedName = "DOCTOR",
                    ConcurrencyStamp = doctorRoleId
                }
            };
            builder.Entity<IdentityRole>().HasData(roles);

            // Fixed user IDs
            var parentUserId = "b5b77b5d-65b6-4f32-93f4-3f76b14e6f3c";
            var doctorUserId = "work7b5d-65b6-4f32-93f4-126sko5e6f3c";

            var parentUser = new IdentityUser
            {
                Id = parentUserId,
                UserName = "parent@nestly.com",
                NormalizedUserName = "PARENT@NESTLY.COM",
                Email = "parent@nestly.com",
                NormalizedEmail = "PARENT@NESTLY.COM",
                EmailConfirmed = true,
                SecurityStamp = System.Guid.NewGuid().ToString("D"),
                ConcurrencyStamp = System.Guid.NewGuid().ToString("D")
            };

            var doctorUser = new IdentityUser
            {
                Id = doctorUserId,
                UserName = "doctor@nestly.com",
                NormalizedUserName = "DOCTOR@NESTLY.COM",
                Email = "doctor@nestly.com",
                NormalizedEmail = "DOCTOR@NESTLY.COM",
                EmailConfirmed = true,
                SecurityStamp = System.Guid.NewGuid().ToString("D"),
                ConcurrencyStamp = System.Guid.NewGuid().ToString("D")
            };

            var hasher = new PasswordHasher<IdentityUser>();
            parentUser.PasswordHash = hasher.HashPassword(parentUser, "test");
            doctorUser.PasswordHash = hasher.HashPassword(doctorUser, "test");

            builder.Entity<IdentityUser>().HasData(parentUser, doctorUser);

            // User-roles
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string> { UserId = parentUserId, RoleId = parentRoleId },
                new IdentityUserRole<string> { UserId = doctorUserId, RoleId = doctorRoleId }
            );
        }
    }
}
