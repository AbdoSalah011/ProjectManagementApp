namespace ProjectManagement.Domain.Entities
{
    public class RefreshToken : IBaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Token { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? RevokedAt { get; set; }
        public string? ReplacedByToken { get; set; }

        // Helper Properties
        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
        public bool IsRevoked => RevokedAt.HasValue;
        public bool IsActive => !IsExpired && !IsRevoked;

        // FKs 
        public Guid UserId { get; set; }

        // Navigation Properties
        public virtual ApplicationUser User { get; set; } = null!;
    }
}
