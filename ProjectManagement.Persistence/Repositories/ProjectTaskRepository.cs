namespace ProjectManagement.Persistence.Repositories
{
    public class ProjectTaskRepository : GenericRepository<ProjectTask>, IProjectTaskRepository
    {
        public ProjectTaskRepository(ApplicationDbContext context)
            : base(context) { }


        public IQueryable<ProjectTask> GetPagedByProject(Guid projectId)
        {
            var query = _dbSet
                .AsNoTracking()
                .Where(t => t.ProjectId == projectId);

            return query;
        }

    }
}
