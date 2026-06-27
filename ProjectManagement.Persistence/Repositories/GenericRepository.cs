namespace ProjectManagement.Persistence.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly ApplicationDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public async Task AddAsync(TEntity entity, CancellationToken ct = default)
            => await _dbSet.AddAsync(entity, ct);

        public void Delete(TEntity entity)
            => _dbSet.Remove(entity);

        public async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken ct = default)
            => await _dbSet.FindAsync(id, ct);

        public void Update(TEntity entity)
            => _dbSet.Update(entity);
    }
}
