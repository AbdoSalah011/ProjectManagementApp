namespace ProjectManagement.Persistence.Context
{
    public class ApplicationDbContext :
        IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>,
        IApplicationDbContext
    {
        #region Constructor
        public ApplicationDbContext(DbContextOptions options) : base(options) { }
        #endregion

        #region DbSets
        public override DbSet<ApplicationUser> Users => Set<ApplicationUser>();
        public DbSet<Project> Projects => Set<Project>();
        public DbSet<ProjectTask> Tasks => Set<ProjectTask>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
        #endregion

        #region Actions
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Identity Tables Configuratons
            builder.Entity<IdentityRole<Guid>>().ToTable("Roles", "identity");
            builder.Entity<IdentityUserRole<Guid>>().ToTable("UserRoles", "identity");
            builder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims", "identity");
            builder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogins", "identity");
            builder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaims", "identity");
            builder.Entity<IdentityUserToken<Guid>>().ToTable("UserTokens", "identity");

            // Apply full Configuration of all Entities
            builder.ApplyConfigurationsFromAssembly(
                typeof(ProjectConfiguration).Assembly);
        }
        #endregion
    }
}
