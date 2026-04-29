using System;

public class CreateAppUserDto
{
    public string Email { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; } = default!;
    public string Username { get; set; } = default!;
    public string Password { get; set; } = default!;
    public long RoleId { get; set; }

    public DateTime? LmpDate { get; set; }
    public DateTime? DueDate { get; set; }
    public int? CycleLengthDays { get; set; }
}


public class AppUserSearchObject
{
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public long? RoleId { get; set; }

}

public class AppUserPatchDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Gender { get; set; }
}

public class AppUserResultDto
{
    public long Id { get; set; }
    public string Email { get; set; } = default!;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public long? RoleId { get; set; }
    public string IdentityUserId { get; set; } = default!;
    public string ParentStatus { get; set; }
    public int? BabyAgeMonths { get; set; }
    public int? PregnancyTrimester { get; set; }
    public long? ParentProfileId { get; set; }
}

public class ChangePasswordDto
{
    public string? OldPassword { get; set; }
    public string NewPassword { get; set; } = default!;
    public string ConfirmPassword { get; set; } = default!;
}


