namespace ProjectManagement.Application.Common.Interfaces
{
    public interface IRefreshTokenRepository : IGenericRepository<RefreshToken>
    {
        Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken ct = default);
    }
}
