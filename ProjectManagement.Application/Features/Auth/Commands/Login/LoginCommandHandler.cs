namespace ProjectManagement.Application.Features.Auth.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtService _jwtService;
        private readonly IUnitOfWork _unitOfWork;

        public LoginCommandHandler(
            UserManager<ApplicationUser> userManager,
            IJwtService jwtService,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _unitOfWork = unitOfWork;
        }

        public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken ct)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            // Deliberately identical error for "user not found" and "wrong password" —
            // distinguishing them lets an attacker enumerate which emails are registered.
            if (user is null || !await _userManager.CheckPasswordAsync(user, request.Password))
                throw new Application.Common.Exceptions.ValidationException(
                    new[] { new ValidationFailure(string.Empty, "Invalid email or password.") });

            var roles = await _userManager.GetRolesAsync(user);

            var accessToken = await _jwtService.GenerateAccessTokenAsync(user, ct);
            var refreshToken = await _jwtService.GenerateRefreshTokenAsync(user.Id);

            await _unitOfWork.RefreshTokens.AddAsync(refreshToken, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return new LoginResponse(accessToken, refreshToken.Token, DateTime.UtcNow.AddMinutes(15));
        }
    }
}
