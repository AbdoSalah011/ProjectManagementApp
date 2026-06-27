namespace ProjectManagement.Application.Common.Interfaces
{
    public interface ICurrentUserService
    {
        public Guid UserId { get; }

        public string Email { get; }

        public IEnumerable<string> Roles { get; }

        public bool IsAuthenticated { get; }
        public bool IsAdmin { get; }
    }
}
