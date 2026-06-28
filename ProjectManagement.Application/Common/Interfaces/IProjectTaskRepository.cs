namespace ProjectManagement.Application.Common.Interfaces
{
    public interface IProjectTaskRepository : IGenericRepository<ProjectTask>
    {
        IQueryable<ProjectTask> GetPagedByProject(Guid projectId);
    }
}
