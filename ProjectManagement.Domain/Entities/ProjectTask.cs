namespace ProjectManagement.Domain.Entities
{
    public class ProjectTask : IBaseEntity, IAuditableEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public ProjectTaskStatus Status { get; set; }
        public DateTime DueDate { get; set; }
        public TaskPriority Priority { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // FKs
        public Guid ProjectId { get; set; }

        // Navigation Properties
        public virtual Project Project { get; set; } = null!;
    }
}
