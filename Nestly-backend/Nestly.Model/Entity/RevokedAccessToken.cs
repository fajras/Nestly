using System;
using System.ComponentModel.DataAnnotations;

namespace Nestly.Model.Entity
{
    // Explicit blacklist of access-token ids (JWT "jti" claim) revoked
    // before their natural expiry - e.g. on logout. Access tokens are now
    // short-lived, so this list stays small; ExpiresAt mirrors the token's
    // own expiry purely so old entries can eventually be purged.
    public class RevokedAccessToken
    {
        [Key]
        public long Id { get; set; }

        [Required, MaxLength(64)]
        public string Jti { get; set; } = default!;

        public DateTime ExpiresAt { get; set; }
        public DateTime RevokedAt { get; set; }
    }
}
