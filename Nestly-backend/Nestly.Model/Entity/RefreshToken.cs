using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Nestly.Model.Entity
{
    // Opaque, long-lived credential used only to mint new short-lived
    // access tokens via POST /api/auth/refresh. The raw value is only ever
    // returned to the client once, at issuance; only its SHA-256 hash is
    // stored, so a leaked database backup does not hand out usable tokens.
    public class RefreshToken
    {
        [Key]
        public long Id { get; set; }

        [ForeignKey(nameof(AppUser))]
        public long AppUserId { get; set; }

        [JsonIgnore]
        public AppUser AppUser { get; set; } = default!;

        [Required, MaxLength(128)]
        public string TokenHash { get; set; } = default!;

        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? RevokedAt { get; set; }

        // Set when this token was rotated out in favor of a new one, so a
        // reuse of an already-rotated token can be detected/traced.
        public string? ReplacedByTokenHash { get; set; }

        [NotMapped]
        public bool IsActive => RevokedAt == null && DateTime.UtcNow < ExpiresAt;
    }
}
