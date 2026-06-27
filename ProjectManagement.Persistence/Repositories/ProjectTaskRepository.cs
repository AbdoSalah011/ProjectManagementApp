namespace ProjectManagement.Persistence.Repositories
{
    public class ProjectTaskRepository : GenericRepository<ProjectTask>, IProjectTaskRepository
    {
        public ProjectTaskRepository(ApplicationDbContext context)
            : base(context) { }


        public async Task<PaginatedList<ProjectTask>> GetPagedByProjectAsync(
            Guid projectId,
            int pageNumber,
            int pageSize,
            CancellationToken ct = default)
        {
            var query = _dbSet
                .AsNoTracking()
                .Where(t => t.ProjectId == projectId);

            return await PaginatedList<ProjectTask>.CreatAsync(
                query,
                pageNumber,
                pageSize,
                ct);
        }

    }
}
