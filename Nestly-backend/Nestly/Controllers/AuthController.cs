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
        private readonly ICurrentUserService _currentUserService;

        public AuthController(
            UserManager<IdentityUser> userManager,
            ITokenRepository tokenRepository,
            NestlyDbContext db,
            ICurrentUserService currentUserService)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
            _db = db;
            _currentUserService = currentUserService;
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

            var appUser = await _db.AppUsers
                .Include(u => u.ParentProfile)
                .Include(u => u.DoctorProfile)
                .FirstOrDefaultAsync(
                    u => u.IdentityUserId == identityUser.Id);

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

            var (refreshToken, _) = await tokenRepository.CreateRefreshTokenAsync(appUser.Id);

            return Ok(new LoginResponseDto
            {
                Email = request.Email,
                Role = string.Join(",", roles),
                token = jwtToken,
                refreshToken = refreshToken,
                ParentProfileId = parentProfileId,
                DoctorProfileId = doctorProfileId,
                UserName = identityUser.UserName
            });
        }

        // Access tokens are short-lived; the client calls this with its
        // refresh token to get a new access token without asking the user
        // to log in again. The refresh token itself is rotated on every
        // use (old one revoked, new one issued) so a stolen-but-unused
        // refresh token becomes worthless the next time the legitimate
        // client refreshes.
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequestDto request)
        {
            var existing = await tokenRepository.GetActiveRefreshTokenAsync(request.RefreshToken);

            if (existing == null)
            {
                return Unauthorized("Refresh token is invalid or has expired.");
            }

            var appUser = await _db.AppUsers
                .FirstOrDefaultAsync(u => u.Id == existing.AppUserId);

            if (appUser == null)
            {
                return Unauthorized("User not found.");
            }

            var identityUser = await userManager.FindByIdAsync(appUser.IdentityUserId);

            if (identityUser == null)
            {
                return Unauthorized("User not found.");
            }

            var roles = await userManager.GetRolesAsync(identityUser);

            var newAccessToken = tokenRepository.CreateJwtToken(
                identityUser,
                roles.ToList(),
                appUser.Id
            );

            var (newRefreshToken, _) = await tokenRepository.CreateRefreshTokenAsync(appUser.Id);

            await tokenRepository.RevokeRefreshTokenAsync(existing, newRefreshToken);

            return Ok(new RefreshTokenResponseDto
            {
                Token = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }

        // Manual token revocation: signs the current device out immediately
        // by revoking its refresh token (so it can no longer mint new
        // access tokens) and blacklisting the still-valid access token used
        // to call this endpoint (so it stops working right away instead of
        // staying valid until it naturally expires).
        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutRequestDto request)
        {
            var existing = await tokenRepository.GetActiveRefreshTokenAsync(request.RefreshToken);

            if (existing != null)
            {
                await tokenRepository.RevokeRefreshTokenAsync(existing);
            }

            var jti = User.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti)?.Value;
            var expClaim = User.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Exp)?.Value;

            if (!string.IsNullOrEmpty(jti) &&
                long.TryParse(expClaim, out var expUnix))
            {
                var expiresAt = DateTimeOffset.FromUnixTimeSeconds(expUnix).UtcDateTime;
                await tokenRepository.RevokeAccessTokenAsync(jti, expiresAt);
            }

            return Ok();
        }
        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(
    [FromBody] ChangePasswordDto dto)
        {
            if (dto.NewPassword != dto.ConfirmPassword)
            {
                return BadRequest("Passwords do not match.");
            }

            if (string.IsNullOrWhiteSpace(dto.OldPassword))
            {
                return BadRequest("Old password is required.");
            }

            var currentUserId =
                _currentUserService
                    .GetCurrentAppUserId();

            var appUser = await _db.AppUsers
                .FirstOrDefaultAsync(
                    x => x.Id == currentUserId);

            if (appUser == null)
            {
                return NotFound("User not found.");
            }

            var identityUser =
                await userManager.FindByIdAsync(
                    appUser.IdentityUserId);

            if (identityUser == null)
            {
                return NotFound("Identity user not found.");
            }

            var result =
                await userManager.ChangePasswordAsync(
                    identityUser,
                    dto.OldPassword,
                    dto.NewPassword
                );

            if (!result.Succeeded)
            {
                return BadRequest(
                    result.Errors.Select(
                        e => e.Description));
            }

            // A password change should invalidate every other signed-in
            // session/device for this account, not just the current one.
            await tokenRepository.RevokeAllRefreshTokensForUserAsync(appUser.Id);

            return Ok();
        }

    }
}
