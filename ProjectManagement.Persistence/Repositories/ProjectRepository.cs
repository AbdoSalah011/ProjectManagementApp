namespace ProjectManagement.Persistence.Repositories
{
    public class ProjectRepository : GenericRepository<Project>, IProjectRepository
    {
        public ProjectRepository(ApplicationDbContext context)
            : base(context) { }

        public async Task<PaginatedList<Project>> GetPagedByUserAsync(
            Guid userId,
            bool isAdmin,
            int pageNumber,
            int pageSize,
            CancellationToken ct = default)
        {
            var query = _dbSet.AsNoTracking().AsQueryable();

            if (!isAdmin)
            {
                query = query.Where(p => p.UserId == userId);
            }

            return await PaginatedList<Project>.CreatAsync(
                query,
                pageNumber,
                pageSize,
                ct);
        }
    }
}
