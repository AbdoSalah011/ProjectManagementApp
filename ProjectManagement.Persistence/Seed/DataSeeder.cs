namespace ProjectManagement.Persistence.Seed
{
    public class DataSeeder : IDataSeeder
    {
        private readonly ApplicationDbContext _context;
        public DataSeeder(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            if (!_context.Roles.Any())
            {
                var roles = new List<IdentityRole<Guid>>
                {
                    new IdentityRole<Guid> { Name = Roles.Admin.ToLower(), NormalizedName = Roles.Admin },
                    new IdentityRole<Guid> { Name = Roles.Member.ToLower(), NormalizedName = Roles.Admin }
                };
                _context.Roles.AddRange(roles);
                _context.SaveChanges();
            }
        }
    }
}
