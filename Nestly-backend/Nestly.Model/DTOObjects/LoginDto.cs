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
        public long parentProfileId { get; set; }
        public string Role { get; set; }
        public string? UserName { get; set; }
    }
}
