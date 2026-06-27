namespace ProjectManagement.Application.Common.Interfaces
{
    public interface IProjectRepository : IGenericRepository<Project>
    {
        Task<PaginatedList<Project>> GetPagedByUserAsync(
            Guid userId,
            bool isAdmin,
            int pageNumber,
            int pageSize,
            CancellationToken ct = default);
    }
}
