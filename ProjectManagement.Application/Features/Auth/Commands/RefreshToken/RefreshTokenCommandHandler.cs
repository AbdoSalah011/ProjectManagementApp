namespace ProjectManagement.Application.Features.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, RefreshTokenResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtService _jwtService;

        public RefreshTokenCommandHandler(
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager,
            IJwtService jwtService)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _jwtService = jwtService;
        }

        public async Task<RefreshTokenResponse> Handle(RefreshTokenCommand request, CancellationToken ct)
        {
            var existingToken = await _unitOfWork.RefreshTokens.GetByTokenAsync(request.Token);

            if (existingToken is null || !existingToken.IsActive)
                throw new ForbiddenException("Invalid or expired refresh token.");

            var user = await _userManager.FindByIdAsync(existingToken.UserId.ToString());
            if (user is null)
                throw new NotFoundException(nameof(ApplicationUser), existingToken.UserId);

            // Rotation: revoke the used token, issue a brand new one, chain them via ReplacedByToken.
            // This limits the blast radius if a refresh token is ever stolen — a stolen-and-reused
            // token is immediately detectable because it'll already be revoked on the legitimate next use.
            var newRefreshToken = await _jwtService.GenerateRefreshTokenAsync(user.Id, ct);

            existingToken.RevokedAt = DateTime.UtcNow;
            existingToken.ReplacedByToken = newRefreshToken.Token;
            _unitOfWork.RefreshTokens.Update(existingToken);

            await _unitOfWork.RefreshTokens.AddAsync(newRefreshToken);

            var accessToken = await _jwtService.GenerateAccessTokenAsync(user, ct);

            await _unitOfWork.SaveChangesAsync(ct);

            return new RefreshTokenResponse(accessToken, newRefreshToken.Token, DateTime.UtcNow.AddMinutes(15));
        }
    }
}
