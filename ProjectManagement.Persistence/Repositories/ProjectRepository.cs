namespace ProjectManagement.Persistence.Repositories
{
    public class ProjectRepository : GenericRepository<Project>, IProjectRepository
    {
        public ProjectRepository(ApplicationDbContext context)
            : base(context) { }

        public IQueryable<Project> GetPagedByUser(Guid userId, bool isAdmin)
        {
            var query = _dbSet.AsNoTracking().AsQueryable();

            if (!isAdmin)
            {
                query = query.Where(p => p.UserId == userId);
            }

            return query;
        }
    }
}
