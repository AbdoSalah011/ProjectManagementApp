namespace ProjectManagement.Domain.Entities
{
    public class ApplicationUser : IdentityUser<Guid>, IBaseEntity, IAuditableEntity
    {
        public override Guid Id { get; set; } = Guid.NewGuid();
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation Properties
        public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
