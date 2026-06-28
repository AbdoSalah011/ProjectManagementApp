namespace ProjectManagement.Application.Features.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthResponseDto>
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

        public async Task<AuthResponseDto> Handle(RefreshTokenCommand request, CancellationToken ct)
        {
            var existingToken = await _unitOfWork.RefreshTokens.GetByTokenAsync(request.Token);

            if (existingToken is null || !existingToken.IsActive)
                throw new ForbiddenException("Invalid or expired refresh token.");

            var user = await _userManager.FindByIdAsync(existingToken.UserId.ToString());
            if (user is null)
                throw new NotFoundException(nameof(ApplicationUser), existingToken.UserId);

            var newRefreshToken = await _jwtService.GenerateRefreshTokenAsync(user.Id, ct);

            existingToken.RevokedAt = DateTime.UtcNow;
            existingToken.ReplacedByToken = newRefreshToken.Token;
            _unitOfWork.RefreshTokens.Update(existingToken);

            await _unitOfWork.RefreshTokens.AddAsync(newRefreshToken);

            var accessToken = await _jwtService.GenerateAccessTokenAsync(user, ct);

            await _unitOfWork.SaveChangesAsync(ct);

            return new AuthResponseDto(accessToken, newRefreshToken.Token, DateTime.UtcNow.AddMinutes(15));
        }
    }
}
