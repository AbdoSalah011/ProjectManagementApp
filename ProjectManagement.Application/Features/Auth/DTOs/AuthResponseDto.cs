namespace ProjectManagement.Application.Features.Auth.DTOs
{
    public record AuthResponseDto(
        string AccessToken,
        string RefreshToken,
        DateTime AccessTokenExpires);
}
