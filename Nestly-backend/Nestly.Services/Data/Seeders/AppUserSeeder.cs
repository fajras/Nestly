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
                    FirstName = "Amela",
                    LastName = "Hasić",
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
                    FirstName = "Naida",
                    LastName = "Bećirović",
                    PhoneNumber = "+38762000000",
                    DateOfBirth = new DateTime(1990, 3, 25),
                    Gender = "Female",
                    RoleId = 2 // Doctor
                },
                new AppUser
                {
                    Id = 3,
                    IdentityUserId = "8339ef91-9659-4986-b918-af66726adf19",
                    Email = "amina.hodzic@nestly.com",
                    FirstName = "Amina",
                    LastName = "Hodžić",
                    PhoneNumber = "+3876100001",
                    DateOfBirth = new DateTime(1996, 4, 12),
                    Gender = "Female",
                    RoleId = 1 // Parent
                },
                new AppUser
                {
                    Id = 4,
                    IdentityUserId = "aa140327-579a-4a94-b635-b4d9a915d279",
                    Email = "lejla.kovacevic@nestly.com",
                    FirstName = "Lejla",
                    LastName = "Kovačević",
                    PhoneNumber = "+3876100002",
                    DateOfBirth = new DateTime(1993, 11, 2),
                    Gender = "Female",
                    RoleId = 1 // Parent
                },
                new AppUser
                {
                    Id = 5,
                    IdentityUserId = "d771c068-ae58-4a8c-abc1-88ea3418bcc3",
                    Email = "ajla.delic@nestly.com",
                    FirstName = "Ajla",
                    LastName = "Delić",
                    PhoneNumber = "+3876100003",
                    DateOfBirth = new DateTime(1991, 7, 19),
                    Gender = "Female",
                    RoleId = 1 // Parent
                },
                new AppUser
                {
                    Id = 6,
                    IdentityUserId = "27cc0316-257a-442d-bbe5-46656ec343e0",
                    Email = "emina.softic@nestly.com",
                    FirstName = "Emina",
                    LastName = "Softić",
                    PhoneNumber = "+3876100004",
                    DateOfBirth = new DateTime(1994, 2, 28),
                    Gender = "Female",
                    RoleId = 1 // Parent
                },
                new AppUser
                {
                    Id = 7,
                    IdentityUserId = "2d30b0f8-e94e-4a85-be12-bb2bb528b70b",
                    Email = "selma.begic@nestly.com",
                    FirstName = "Selma",
                    LastName = "Begić",
                    PhoneNumber = "+3876100005",
                    DateOfBirth = new DateTime(1995, 9, 14),
                    Gender = "Female",
                    RoleId = 1 // Parent
                },
                new AppUser
                {
                    Id = 8,
                    IdentityUserId = "d66502ad-89df-4de1-90ab-61c153a26d12",
                    Email = "amra.halilovic@nestly.com",
                    FirstName = "Amra",
                    LastName = "Halilović",
                    PhoneNumber = "+3876100006",
                    DateOfBirth = new DateTime(1992, 1, 8),
                    Gender = "Female",
                    RoleId = 1 // Parent
                },
                new AppUser
                {
                    Id = 9,
                    IdentityUserId = "e02fd7d5-9498-4439-877b-1170b9ee6f4e",
                    Email = "merisa.karic@nestly.com",
                    FirstName = "Merisa",
                    LastName = "Karić",
                    PhoneNumber = "+3876100007",
                    DateOfBirth = new DateTime(1990, 6, 23),
                    Gender = "Female",
                    RoleId = 1 // Parent
                },
                new AppUser
                {
                    Id = 10,
                    IdentityUserId = "13edc40b-a767-49d0-a194-610e85ae84d0",
                    Email = "dzenita.mujic@nestly.com",
                    FirstName = "Dženita",
                    LastName = "Mujić",
                    PhoneNumber = "+3876100008",
                    DateOfBirth = new DateTime(1989, 12, 30),
                    Gender = "Female",
                    RoleId = 1 // Parent
                },
                new AppUser
                {
                    Id = 11,
                    IdentityUserId = "323c2855-426d-4726-9520-bdd2da76a5d5",
                    Email = "amila.zukic@nestly.com",
                    FirstName = "Amila",
                    LastName = "Zukić",
                    PhoneNumber = "+3876100009",
                    DateOfBirth = new DateTime(1988, 3, 17),
                    Gender = "Female",
                    RoleId = 1 // Parent
                },
                new AppUser
                {
                    Id = 12,
                    IdentityUserId = "465ced2a-f9e9-4f23-acbb-5a11658737b2",
                    Email = "nadira.sehic@nestly.com",
                    FirstName = "Nadira",
                    LastName = "Šehić",
                    PhoneNumber = "+3876100010",
                    DateOfBirth = new DateTime(1987, 10, 5),
                    Gender = "Female",
                    RoleId = 1 // Parent
                }
            );
        }
    }
}
