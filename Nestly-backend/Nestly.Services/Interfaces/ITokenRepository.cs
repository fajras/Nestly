using Microsoft.AspNetCore.Identity;
using Nestly.Model.Entity;

namespace Nestly.Services.Interfaces
{
    public interface ITokenRepository
    {
        string CreateJwtToken(
            IdentityUser user,
            List<string> roles,
            long appUserId
        );

        Task<(string RawToken, RefreshToken Entity)> CreateRefreshTokenAsync(long appUserId);

        Task<RefreshToken?> GetActiveRefreshTokenAsync(string rawToken);

        Task RevokeRefreshTokenAsync(RefreshToken token, string? replacedByRawToken = null);

        Task RevokeAllRefreshTokensForUserAsync(long appUserId);

        Task RevokeAccessTokenAsync(string jti, DateTime accessTokenExpiresAt);

        Task<bool> IsAccessTokenRevokedAsync(string jti);
    }
}
