namespace ProjectManagement.Application.Common.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        // Reads
        Task<TEntity?> GetByIdAsync(Guid id, CancellationToken ct = default);


        // Writes
        Task AddAsync(TEntity entity, CancellationToken ct = default);
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}
