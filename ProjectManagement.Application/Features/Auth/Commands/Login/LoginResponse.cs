namespace ProjectManagement.Application.Features.Auth.Commands.Login
{
    public record LoginResponse(
        string AccessToken,
        string RefreshToken,
        DateTime AccessTokenExpires);
}
