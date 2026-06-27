namespace ProjectManagement.Application.Features.Auth.Commands.Register
{
    public record RegisterResponse(
        string UserId,
        string Email,
        string FullName);
}
