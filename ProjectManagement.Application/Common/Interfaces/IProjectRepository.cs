namespace ProjectManagement.Application.Common.Interfaces
{
    public interface IProjectRepository : IGenericRepository<Project>
    {
        IQueryable<Project> GetPagedByUser(Guid userId, bool isAdmin);
    }
}
