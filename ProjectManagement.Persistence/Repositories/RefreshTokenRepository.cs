namespace ProjectManagement.Persistence.Repositories
{
    public class RefreshTokenRepository : GenericRepository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(ApplicationDbContext context)
            : base(context) { }


        public async Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken ct = default)
            => await _dbSet.AsNoTracking().SingleOrDefaultAsync(rt => rt.Token == token, ct);
    }
}
