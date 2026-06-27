namespace ProjectManagement.Persistence.Configurations
{
    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.ToTable("Projects", "app");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(p => p.Description)
                   .IsRequired(false)
                   .HasMaxLength(500);

            builder.Property(p => p.CreatedAt)
                   .IsRequired();

            builder.Property(p => p.UpdatedAt)
                   .IsRequired(false);

            // Relationships
            builder.HasOne(p => p.User)
                   .WithMany(u => u.Projects)
                   .HasForeignKey(p => p.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
