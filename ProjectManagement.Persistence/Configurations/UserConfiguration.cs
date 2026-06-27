namespace ProjectManagement.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable("Users", "identity");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.FirstName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(u => u.LastName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(u => u.CreatedAt)
                   .IsRequired();

            builder.Property(u => u.UpdatedAt)
                   .IsRequired(false);

            builder.HasIndex(u => new { u.FirstName, u.LastName });
        }
    }
}
