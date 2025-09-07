using System;

namespace Nestly.Model.DTOObjects
{
    public class CreateAppUserDto
    {
        public string Email { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; } = default!;
        public string Username { get; set; } = default!;
        public string Password { get; set; } = default!;
    }

    public class AppUserSearchObject
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }

    public class CreateParentProfileDto
    {
        public long UserId { get; set; }
    }
    public class CreateDoctorProfileDto
    {
        public long UserId { get; set; }
    }
}
