namespace ProjectManagement.Application.Common.Interfaces
{
    public interface IProjectTaskRepository : IGenericRepository<ProjectTask>
    {
        Task<PaginatedList<ProjectTask>> GetPagedByProjectAsync(
            Guid projectId,
            int pageNumber,
            int pageSize,
            CancellationToken ct = default);
    }
}
