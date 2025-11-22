using Microsoft.AspNetCore.Identity;

namespace Nestly.Services.Interfaces
{
    public interface ITokenRepository
    {
        string CreateJwtToken(IdentityUser user, List<string> roles);
    }
}
