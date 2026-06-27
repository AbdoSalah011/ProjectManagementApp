namespace ProjectManagement.Application.Features.Auth.DTOs
{
    public sealed class AuthResponseDto
    {
        public string AccessToken { get; init; } = default!;

        public string RefreshToken { get; init; } = default!;

        public DateTime ExpiresAt { get; init; }

        public UserDto User { get; init; } = default!;
    }
}
