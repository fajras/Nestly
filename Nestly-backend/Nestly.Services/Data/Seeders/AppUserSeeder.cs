using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nestly.Model.Entity;

namespace Nestly.Services.Data.Seeders
{
    public static class AppUserSeeder
    {
        public static void SeedData(this EntityTypeBuilder<AppUser> entity)
        {
            const string parentUserId = "b5b77b5d-65b6-4f32-93f4-3f76b14e6f3c";
            const string doctorUserId = "work7b5d-65b6-4f32-93f4-126sko5e6f3c";

            entity.HasData(
                new AppUser
                {
                    Id = 1,
                    IdentityUserId = parentUserId,
                    Email = "parent@nestly.com",
                    FirstName = "TestParent",
                    LastName = "User",
                    PhoneNumber = "+38761000000",
                    DateOfBirth = new DateTime(1998, 5, 10),
                    Gender = "Female",
                    RoleId = 1 // Parent
                },
                new AppUser
                {
                    Id = 2,
                    IdentityUserId = doctorUserId,
                    Email = "doctor@nestly.com",
                    FirstName = "TestDoctor",
                    LastName = "User",
                    PhoneNumber = "+38762000000",
                    DateOfBirth = new DateTime(1990, 3, 25),
                    Gender = "Female",
                    RoleId = 2 // Doctor
                }
            );
        }
    }
}
