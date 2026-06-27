namespace ProjectManagement.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        #region DbSets
        public DbSet<ApplicationUser> Users { get; }
        public DbSet<Project> Projects { get; }
        public DbSet<ProjectTask> Tasks { get; }
        public DbSet<RefreshToken> RefreshTokens { get; }
        #endregion


        #region EF Core Methods
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        #endregion
    }
}
