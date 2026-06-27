namespace ProjectManagement.Application.Features.Auth.Commands.RefreshToken
{
    public record RefreshTokenCommand(string Token) : IRequest<RefreshTokenResponse>;
}
