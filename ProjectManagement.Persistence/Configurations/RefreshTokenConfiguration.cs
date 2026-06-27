namespace ProjectManagement.Persistence.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("RefreshTokens", "identity");

            builder.HasKey(rt => rt.Id);

            builder.Property(rt => rt.Token)
                   .IsRequired()
                   .HasMaxLength(512);

            builder.Property(rt => rt.ExpiresAt)
                   .IsRequired();

            builder.Property(rt => rt.CreatedAt)
                   .IsRequired()
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(rt => rt.RevokedAt)
                   .IsRequired(false);

            builder.Property(rt => rt.ReplacedByToken)
                   .IsRequired(false)
                   .HasMaxLength(512);

            // Indexes
            builder.HasIndex(rt => rt.Token)
                   .IsUnique();

            // Ignore Helper Properties
            builder.Ignore(rt => rt.IsExpired);
            builder.Ignore(rt => rt.IsRevoked);
            builder.Ignore(rt => rt.IsActive);


            // Relationships
            builder.HasOne(rt => rt.User)
                   .WithMany(u => u.RefreshTokens)
                   .HasForeignKey(rt => rt.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
