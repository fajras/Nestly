using System.ComponentModel.DataAnnotations;

namespace Nestly.Model.DTOObjects
{
    public class LoginRequestDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class LoginResponseDto
    {
        public string Email { get; set; }
        public string token { get; set; }
        public string refreshToken { get; set; } = default!;
        public string Role { get; set; }
        public string? UserName { get; set; }
        public long? ParentProfileId { get; set; }
        public long? DoctorProfileId { get; set; }
    }

    public class RefreshTokenRequestDto
    {
        [Required]
        public string RefreshToken { get; set; } = default!;
    }

    public class RefreshTokenResponseDto
    {
        public string Token { get; set; } = default!;
        public string RefreshToken { get; set; } = default!;
    }

    public class LogoutRequestDto
    {
        [Required]
        public string RefreshToken { get; set; } = default!;
    }
}
