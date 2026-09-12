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

            var parentRoleId = "2b3b5f31-c1d6-4a82-8e1e-2c318c5bc98e";
            var doctorRoleId = "3d4e2bfa-3a12-4df8-9fc5-bd45e2102a3c";

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

            var demoUser3 = new IdentityUser
            {
                Id = "8339ef91-9659-4986-b918-af66726adf19",
                UserName = "amina.hodzic@nestly.com",
                NormalizedUserName = "AMINA.HODZIC@NESTLY.COM",
                Email = "amina.hodzic@nestly.com",
                NormalizedEmail = "AMINA.HODZIC@NESTLY.COM",
                EmailConfirmed = true,
                SecurityStamp = System.Guid.NewGuid().ToString("D"),
                ConcurrencyStamp = System.Guid.NewGuid().ToString("D")
            };
            demoUser3.PasswordHash = hasher.HashPassword(demoUser3, "test");

            var demoUser4 = new IdentityUser
            {
                Id = "aa140327-579a-4a94-b635-b4d9a915d279",
                UserName = "lejla.kovacevic@nestly.com",
                NormalizedUserName = "LEJLA.KOVACEVIC@NESTLY.COM",
                Email = "lejla.kovacevic@nestly.com",
                NormalizedEmail = "LEJLA.KOVACEVIC@NESTLY.COM",
                EmailConfirmed = true,
                SecurityStamp = System.Guid.NewGuid().ToString("D"),
                ConcurrencyStamp = System.Guid.NewGuid().ToString("D")
            };
            demoUser4.PasswordHash = hasher.HashPassword(demoUser4, "test");

            var demoUser5 = new IdentityUser
            {
                Id = "d771c068-ae58-4a8c-abc1-88ea3418bcc3",
                UserName = "ajla.delic@nestly.com",
                NormalizedUserName = "AJLA.DELIC@NESTLY.COM",
                Email = "ajla.delic@nestly.com",
                NormalizedEmail = "AJLA.DELIC@NESTLY.COM",
                EmailConfirmed = true,
                SecurityStamp = System.Guid.NewGuid().ToString("D"),
                ConcurrencyStamp = System.Guid.NewGuid().ToString("D")
            };
            demoUser5.PasswordHash = hasher.HashPassword(demoUser5, "test");

            var demoUser6 = new IdentityUser
            {
                Id = "27cc0316-257a-442d-bbe5-46656ec343e0",
                UserName = "emina.softic@nestly.com",
                NormalizedUserName = "EMINA.SOFTIC@NESTLY.COM",
                Email = "emina.softic@nestly.com",
                NormalizedEmail = "EMINA.SOFTIC@NESTLY.COM",
                EmailConfirmed = true,
                SecurityStamp = System.Guid.NewGuid().ToString("D"),
                ConcurrencyStamp = System.Guid.NewGuid().ToString("D")
            };
            demoUser6.PasswordHash = hasher.HashPassword(demoUser6, "test");

            var demoUser7 = new IdentityUser
            {
                Id = "2d30b0f8-e94e-4a85-be12-bb2bb528b70b",
                UserName = "selma.begic@nestly.com",
                NormalizedUserName = "SELMA.BEGIC@NESTLY.COM",
                Email = "selma.begic@nestly.com",
                NormalizedEmail = "SELMA.BEGIC@NESTLY.COM",
                EmailConfirmed = true,
                SecurityStamp = System.Guid.NewGuid().ToString("D"),
                ConcurrencyStamp = System.Guid.NewGuid().ToString("D")
            };
            demoUser7.PasswordHash = hasher.HashPassword(demoUser7, "test");

            var demoUser8 = new IdentityUser
            {
                Id = "d66502ad-89df-4de1-90ab-61c153a26d12",
                UserName = "amra.halilovic@nestly.com",
                NormalizedUserName = "AMRA.HALILOVIC@NESTLY.COM",
                Email = "amra.halilovic@nestly.com",
                NormalizedEmail = "AMRA.HALILOVIC@NESTLY.COM",
                EmailConfirmed = true,
                SecurityStamp = System.Guid.NewGuid().ToString("D"),
                ConcurrencyStamp = System.Guid.NewGuid().ToString("D")
            };
            demoUser8.PasswordHash = hasher.HashPassword(demoUser8, "test");

            var demoUser9 = new IdentityUser
            {
                Id = "e02fd7d5-9498-4439-877b-1170b9ee6f4e",
                UserName = "merisa.karic@nestly.com",
                NormalizedUserName = "MERISA.KARIC@NESTLY.COM",
                Email = "merisa.karic@nestly.com",
                NormalizedEmail = "MERISA.KARIC@NESTLY.COM",
                EmailConfirmed = true,
                SecurityStamp = System.Guid.NewGuid().ToString("D"),
                ConcurrencyStamp = System.Guid.NewGuid().ToString("D")
            };
            demoUser9.PasswordHash = hasher.HashPassword(demoUser9, "test");

            var demoUser10 = new IdentityUser
            {
                Id = "13edc40b-a767-49d0-a194-610e85ae84d0",
                UserName = "dzenita.mujic@nestly.com",
                NormalizedUserName = "DZENITA.MUJIC@NESTLY.COM",
                Email = "dzenita.mujic@nestly.com",
                NormalizedEmail = "DZENITA.MUJIC@NESTLY.COM",
                EmailConfirmed = true,
                SecurityStamp = System.Guid.NewGuid().ToString("D"),
                ConcurrencyStamp = System.Guid.NewGuid().ToString("D")
            };
            demoUser10.PasswordHash = hasher.HashPassword(demoUser10, "test");

            var demoUser11 = new IdentityUser
            {
                Id = "323c2855-426d-4726-9520-bdd2da76a5d5",
                UserName = "amila.zukic@nestly.com",
                NormalizedUserName = "AMILA.ZUKIC@NESTLY.COM",
                Email = "amila.zukic@nestly.com",
                NormalizedEmail = "AMILA.ZUKIC@NESTLY.COM",
                EmailConfirmed = true,
                SecurityStamp = System.Guid.NewGuid().ToString("D"),
                ConcurrencyStamp = System.Guid.NewGuid().ToString("D")
            };
            demoUser11.PasswordHash = hasher.HashPassword(demoUser11, "test");

            var demoUser12 = new IdentityUser
            {
                Id = "465ced2a-f9e9-4f23-acbb-5a11658737b2",
                UserName = "nadira.sehic@nestly.com",
                NormalizedUserName = "NADIRA.SEHIC@NESTLY.COM",
                Email = "nadira.sehic@nestly.com",
                NormalizedEmail = "NADIRA.SEHIC@NESTLY.COM",
                EmailConfirmed = true,
                SecurityStamp = System.Guid.NewGuid().ToString("D"),
                ConcurrencyStamp = System.Guid.NewGuid().ToString("D")
            };
            demoUser12.PasswordHash = hasher.HashPassword(demoUser12, "test");

            builder.Entity<IdentityUser>().HasData(parentUser, doctorUser, demoUser3, demoUser4, demoUser5, demoUser6, demoUser7, demoUser8, demoUser9, demoUser10, demoUser11, demoUser12);

            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string> { UserId = parentUserId, RoleId = parentRoleId },
                new IdentityUserRole<string> { UserId = doctorUserId, RoleId = doctorRoleId },
                new IdentityUserRole<string> { UserId = demoUser3.Id, RoleId = parentRoleId }, new IdentityUserRole<string> { UserId = demoUser4.Id, RoleId = parentRoleId }, new IdentityUserRole<string> { UserId = demoUser5.Id, RoleId = parentRoleId }, new IdentityUserRole<string> { UserId = demoUser6.Id, RoleId = parentRoleId }, new IdentityUserRole<string> { UserId = demoUser7.Id, RoleId = parentRoleId }, new IdentityUserRole<string> { UserId = demoUser8.Id, RoleId = parentRoleId }, new IdentityUserRole<string> { UserId = demoUser9.Id, RoleId = parentRoleId }, new IdentityUserRole<string> { UserId = demoUser10.Id, RoleId = parentRoleId }, new IdentityUserRole<string> { UserId = demoUser11.Id, RoleId = parentRoleId }, new IdentityUserRole<string> { UserId = demoUser12.Id, RoleId = parentRoleId }
            );
        }
    }
}
