using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nestly.Model.DTOObjects;
using Nestly.Services.Data;
using Nestly.Services.Interfaces;


namespace Nestly.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;
        private readonly NestlyDbContext _db;

        public AuthController(
            UserManager<IdentityUser> userManager,
            ITokenRepository tokenRepository,
            NestlyDbContext db)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
            _db = db;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var identityUser = await userManager.FindByEmailAsync(request.Email);
            if (identityUser == null)
            {
                return Unauthorized("Email or password is incorrect.");
            }

            var passwordOk = await userManager.CheckPasswordAsync(identityUser, request.Password);
            if (!passwordOk)
            {
                return Unauthorized("Email or password is incorrect.");
            }

            var appUser = _db.AppUsers
                .Include(u => u.ParentProfile)
                .Include(u => u.DoctorProfile)
                .FirstOrDefault(u => u.IdentityUserId == identityUser.Id);

            if (appUser == null)
            {
                return Unauthorized("User profile not found.");
            }

            long? parentProfileId = null;
            long? doctorProfileId = null;

            if (appUser.ParentProfile != null)
            {
                parentProfileId = appUser.ParentProfile.Id;
            }

            if (appUser.DoctorProfile != null)
            {
                doctorProfileId = appUser.DoctorProfile.Id;
            }

            var roles = await userManager.GetRolesAsync(identityUser);

            var jwtToken = tokenRepository.CreateJwtToken(
                identityUser,
                roles.ToList(),
                appUser.Id
            );

            return Ok(new LoginResponseDto
            {
                Email = request.Email,
                Role = string.Join(",", roles),
                token = jwtToken,
                ParentProfileId = parentProfileId,
                DoctorProfileId = doctorProfileId,
                UserName = identityUser.UserName
            });
        }
        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            var userIdClaim = User.FindFirst("userId")?.Value;

            if (string.IsNullOrWhiteSpace(userIdClaim))
            {
                return Unauthorized();
            }

            if (dto.NewPassword != dto.ConfirmPassword)
            {
                return BadRequest("Passwords do not match.");
            }

            if (string.IsNullOrWhiteSpace(dto.OldPassword))
            {
                return BadRequest("Old password is required.");
            }

            var currentUserId = long.Parse(userIdClaim);

            var appUser = await _db.AppUsers
                .FirstOrDefaultAsync(x => x.Id == currentUserId);

            if (appUser == null)
            {
                return NotFound("User not found.");
            }

            var identityUser = await userManager.FindByIdAsync(appUser.IdentityUserId);

            if (identityUser == null)
            {
                return NotFound("Identity user not found.");
            }

            var result = await userManager.ChangePasswordAsync(
                identityUser,
                dto.OldPassword,
                dto.NewPassword
            );

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.Select(e => e.Description));
            }

            return Ok();
        }

    }
}
