namespace ProjectManagement.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction? _currentTransaction;
        private bool _disposed;

        public IProjectRepository Projects { get; }
        public IProjectTaskRepository ProjectTasks { get; }
        public IRefreshTokenRepository RefreshTokens { get; }

        public UnitOfWork(
            ApplicationDbContext context,
            IProjectRepository projectRepo,
            IProjectTaskRepository projectTaskRepo,
            IRefreshTokenRepository refreshTokens)
        {
            _context = context;
            Projects = projectRepo;
            ProjectTasks = projectTaskRepo;
            RefreshTokens = refreshTokens;
        }


        public async Task BeginTransactionAsync(CancellationToken ct = default)
        {
            if (_currentTransaction != null)
                return;

            _currentTransaction = await _context.Database.BeginTransactionAsync(ct);
        }

        public async Task CommitAsync(CancellationToken ct = default)
        {
            try
            {
                await _context.SaveChangesAsync(ct);
                if (_currentTransaction != null)
                    await _currentTransaction.CommitAsync(ct);
            }
            catch
            {
                if (_currentTransaction != null)
                {
                    await _currentTransaction.RollbackAsync(ct);
                    throw;
                }
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    await _currentTransaction.DisposeAsync();
                    _currentTransaction = null;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task RollbackAsync(CancellationToken ct = default)
        {
            try
            {
                if (_currentTransaction != null)
                {
                    await _currentTransaction.RollbackAsync(ct);
                }
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    await _currentTransaction.DisposeAsync();
                    _currentTransaction = null;
                }
            }
        }

        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
            => await _context.SaveChangesAsync(ct);

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _currentTransaction?.Dispose();
                    _context.Dispose();
                }
                _disposed = true;
            }
        }
    }
}
