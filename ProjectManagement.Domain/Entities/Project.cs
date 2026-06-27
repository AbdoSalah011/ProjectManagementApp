namespace ProjectManagement.Domain.Entities
{
    public class Project : IBaseEntity, IAuditableEntity
    {
        // Properties
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // FKs
        public Guid UserId { get; set; }

        // Navigation Properties
        public virtual ApplicationUser User { get; set; } = null!;
        public virtual ICollection<ProjectTask> Tasks { get; set; } = new List<ProjectTask>();
    }
}
