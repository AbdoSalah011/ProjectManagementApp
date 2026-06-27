namespace ProjectManagement.Application.Common.Interfaces
{
    public interface IJwtService
    {
        Task<string> GenerateAccessTokenAsync(ApplicationUser user, CancellationToken ct = default);

        Task<RefreshToken> GenerateRefreshTokenAsync(Guid userId, CancellationToken ct = default);

    }
}
