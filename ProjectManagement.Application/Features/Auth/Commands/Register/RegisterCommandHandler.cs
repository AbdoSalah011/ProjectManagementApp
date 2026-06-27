namespace ProjectManagement.Application.Features.Auth.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public RegisterCommandHandler(UserManager<ApplicationUser> userManager)
            => _userManager = userManager;

        public async Task<RegisterResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var existing = await _userManager.FindByEmailAsync(request.Email);
            if (existing is not null)
                throw new ConflictException("A user with this email already exists.");

            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
                throw new ProjectManagement.Application.Common.Exceptions.ValidationException(
                    result.Errors.Select(e => new ValidationFailure(string.Empty, e.Description)));

            if (request.IsAdmin)
                await _userManager.AddToRoleAsync(user, Roles.Admin);
            else
                await _userManager.AddToRoleAsync(user, Roles.Member);

            return new RegisterResponse(user.Id.ToString(), user.Email!, $"{user.FirstName} {user.LastName}");
        }
    }
}
