namespace ProjectManagement.Application.Features.Auth.DTOs
{
    public sealed class UserDto
    {
        public Guid Id { get; init; }

        public string FirstName { get; init; } = default!;

        public string LastName { get; init; } = default!;

        public string Email { get; init; } = default!;

        public IList<string> Roles { get; init; }
            = new List<string>();
    }
}
