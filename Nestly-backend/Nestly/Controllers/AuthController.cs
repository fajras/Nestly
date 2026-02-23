using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nestly.Model.DTOObjects;
using Nestly.Services.Data;
using Nestly.Services.Interfaces;


namespace Nestly_WebAPI.Controllers
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
                .FirstOrDefault(u => u.IdentityUserId == identityUser.Id);

            if (appUser == null)
            {
                return Unauthorized("User profile not found.");
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

                parentProfileId = appUser.Id,

                UserName = identityUser.UserName
            });
        }

    }
}
