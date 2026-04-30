using System;
using System.ComponentModel.DataAnnotations;

public class CreateAppUserDto
{
    [Required, EmailAddress]
    public string Email { get; set; } = default!;

    [Required]
    public string FirstName { get; set; } = default!;

    [Required]
    public string LastName { get; set; } = default!;

    [Required]
    public string PhoneNumber { get; set; } = default!;

    [Required]
    public DateTime DateOfBirth { get; set; }

    [Required]
    public string Gender { get; set; } = default!;

    [Required]
    public string Username { get; set; } = default!;

    [Required]
    public string Password { get; set; } = default!;

    [Range(1, long.MaxValue)]
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
    [MaxLength(150)]
    public string? FirstName { get; set; }
    [MaxLength(150)]
    public string? LastName { get; set; }
    [MaxLength(50)]
    public string? PhoneNumber { get; set; }
    public DateTime? DateOfBirth { get; set; }
    [MaxLength(20)]
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
    [Required]
    public string NewPassword { get; set; } = default!;
    [Required]
    public string ConfirmPassword { get; set; } = default!;
}


