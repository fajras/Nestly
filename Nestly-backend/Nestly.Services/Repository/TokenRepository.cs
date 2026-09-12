using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Nestly.Model.Entity;
using Nestly.Services.Data;
using Nestly.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Nestly.Services.Repository
{
    public class TokenRepository : ITokenRepository
    {
        private readonly IConfiguration configuration;
        private readonly NestlyDbContext _db;

        public TokenRepository(IConfiguration configuration, NestlyDbContext db)
        {
            this.configuration = configuration;
            _db = db;
        }

        // Access tokens used to live for 30 days with no way to invalidate
        // one before it expired. They are now short-lived; a client stays
        // signed in by exchanging its refresh token for a new one via
        // POST /api/auth/refresh instead.
        private int AccessTokenMinutes =>
            int.TryParse(configuration["Jwt:AccessTokenMinutes"], out var m) && m > 0 ? m : 30;

        private int RefreshTokenDays =>
            int.TryParse(configuration["Jwt:RefreshTokenDays"], out var d) && d > 0 ? d : 30;

        public string CreateJwtToken(
            IdentityUser user,
            List<string> roles,
            long appUserId
        )
        {
            var claims = new List<Claim>
            {
                new Claim("userId", appUserId.ToString()),
                new Claim("identityUserId", user.Id),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
                // Unique id per access token - lets a specific token be
                // revoked (logout) without waiting for its natural expiry.
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            claims.AddRange(
                roles.Select(r => new Claim(ClaimTypes.Role, r))
            );

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration["Jwt:Key"])
            );

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(AccessTokenMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<(string RawToken, RefreshToken Entity)> CreateRefreshTokenAsync(long appUserId)
        {
            var rawToken = GenerateRawToken();

            var entity = new RefreshToken
            {
                AppUserId = appUserId,
                TokenHash = Hash(rawToken),
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(RefreshTokenDays)
            };

            _db.RefreshTokens.Add(entity);
            await _db.SaveChangesAsync();

            return (rawToken, entity);
        }

        public async Task<RefreshToken?> GetActiveRefreshTokenAsync(string rawToken)
        {
            var hash = Hash(rawToken);

            var entity = await _db.RefreshTokens
                .FirstOrDefaultAsync(t => t.TokenHash == hash);

            return entity is { IsActive: true } ? entity : null;
        }

        public async Task RevokeRefreshTokenAsync(RefreshToken token, string? replacedByRawToken = null)
        {
            token.RevokedAt = DateTime.UtcNow;

            if (replacedByRawToken is not null)
            {
                token.ReplacedByTokenHash = Hash(replacedByRawToken);
            }

            await _db.SaveChangesAsync();
        }

        public async Task RevokeAllRefreshTokensForUserAsync(long appUserId)
        {
            // Bulk update: no need to round-trip every row through change
            // tracking just to flip one column on all of a user's tokens
            // (e.g. on password change or "sign out everywhere").
            await _db.RefreshTokens
                .Where(t => t.AppUserId == appUserId && t.RevokedAt == null)
                .ExecuteUpdateAsync(s => s.SetProperty(t => t.RevokedAt, DateTime.UtcNow));
        }

        public async Task RevokeAccessTokenAsync(string jti, DateTime accessTokenExpiresAt)
        {
            if (string.IsNullOrWhiteSpace(jti))
            {
                return;
            }

            bool alreadyRevoked = await _db.RevokedAccessTokens.AnyAsync(t => t.Jti == jti);

            if (alreadyRevoked)
            {
                return;
            }

            _db.RevokedAccessTokens.Add(new RevokedAccessToken
            {
                Jti = jti,
                ExpiresAt = accessTokenExpiresAt,
                RevokedAt = DateTime.UtcNow
            });

            await _db.SaveChangesAsync();
        }

        public async Task<bool> IsAccessTokenRevokedAsync(string jti)
        {
            if (string.IsNullOrWhiteSpace(jti))
            {
                return false;
            }

            return await _db.RevokedAccessTokens.AnyAsync(t => t.Jti == jti);
        }

        private static string GenerateRawToken()
        {
            var bytes = RandomNumberGenerator.GetBytes(64);
            return Convert.ToBase64String(bytes)
                .Replace("+", "-")
                .Replace("/", "_")
                .Replace("=", "");
        }

        private static string Hash(string rawToken)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(rawToken));
            return Convert.ToHexString(bytes);
        }
    }
}
