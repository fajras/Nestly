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


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            //check password
            var identityUser = await userManager.FindByEmailAsync(request.Email);
            if (identityUser != null)
            {
                //check password
                var checkPasswordResult = await userManager.CheckPasswordAsync(identityUser, request.Password);

                if (checkPasswordResult)
                {
                    var roles = await userManager.GetRolesAsync(identityUser);
                    //create a token response
                    var jwtToken = tokenRepository.CreateJwtToken(identityUser, roles.ToList());

                    var profileId = _db.AppUsers
         .FirstOrDefault(p => p.IdentityUserId == identityUser.Id);

                    long ProfileId = profileId.Id;
                    var response = new LoginResponseDto()
                    {
                        Email = request.Email,
                        Role = string.Join(",", roles),
                        token = jwtToken,
                        parentProfileId = ProfileId,
                        UserName = identityUser?.UserName
                    };
                    return Ok(response);
                }
            }
            ModelState.AddModelError("", "Email or Password is incorrect");

            return ValidationProblem(ModelState);
        }
    }
}
