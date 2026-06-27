namespace ProjectManagement.Application.Common.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        #region Repositories
        public IProjectRepository Projects { get; }
        public IProjectTaskRepository ProjectTasks { get; }
        public IRefreshTokenRepository RefreshTokens { get; }
        #endregion

        #region Actions
        Task<int> SaveChangesAsync(CancellationToken ct = default);
        Task BeginTransactionAsync(CancellationToken ct = default);
        Task CommitAsync(CancellationToken ct = default);
        Task RollbackAsync(CancellationToken ct = default);
        #endregion
    }
}
