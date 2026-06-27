namespace ProjectManagement.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _context;

        public CurrentUserService(IHttpContextAccessor context)
        {
            _context = context;
        }

        // Helper 
        private ClaimsPrincipal? User => _context.HttpContext?.User;

        public Guid UserId =>
            Guid.Parse(
                User?
                .FindFirstValue(ClaimTypes.NameIdentifier)!);

        public string Email =>
            User?
            .FindFirstValue(ClaimTypes.Email)!;

        public IEnumerable<string> Roles
        {
            get
            {
                if (User is null)
                    return Enumerable.Empty<string>();

                return User.FindAll(ClaimTypes.Role).Select(c => c.Value);
            }
        }

        public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;

        public bool IsAdmin =>
            User?.IsInRole("Admin") ?? false;
    }
}
