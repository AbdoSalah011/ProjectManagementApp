namespace ProjectManagement.Persistence.Configurations
{
    public class ProjectTaskConfiguration : IEntityTypeConfiguration<ProjectTask>
    {
        public void Configure(EntityTypeBuilder<ProjectTask> builder)
        {
            builder.ToTable("Tasks", "app");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Title)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(t => t.Description)
                   .IsRequired(false)
                   .HasMaxLength(1000);

            builder.Property(t => t.Status)
                   .IsRequired()
                   .HasConversion<string>()
                   .HasDefaultValue(ProjectTaskStatus.ToDo);

            builder.Property(t => t.DueDate)
                   .IsRequired();

            builder.Property(t => t.Priority)
                   .IsRequired()
                   .HasConversion<string>();

            builder.Property(t => t.CreatedAt)
                   .IsRequired();

            builder.Property(t => t.UpdatedAt)
                   .IsRequired(false);

            // Indexes
            builder.HasIndex(t => t.Title);


            // Relationships
            builder.HasOne(t => t.Project)
                   .WithMany(p => p.Tasks)
                   .HasForeignKey(t => t.ProjectId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
