namespace ProjectManagement.Application.Features.Auth.Commands.Register
{
    public sealed record RegisterCommand(
        string FirstName,
        string LastName,
        string Email,
        string Password,
        bool IsAdmin)
        : IRequest<RegisterResponse>;
}
