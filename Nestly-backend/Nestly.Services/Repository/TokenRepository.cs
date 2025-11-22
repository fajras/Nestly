using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Nestly.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Nestly.Services.Repository
{
    public class TokenRepository : ITokenRepository
    {
        private readonly IConfiguration configuration;

        public TokenRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public string CreateJwtToken(IdentityUser user, List<string> roles)
        {
            //create claims from roles that we have
            var claims = new List<Claim>
           {
               new Claim(ClaimTypes.Email, user.Email),
               new Claim(ClaimTypes.NameIdentifier, user.Id),


           };
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            //JWT Security Token Parameters

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials
                );

            //return token
            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
