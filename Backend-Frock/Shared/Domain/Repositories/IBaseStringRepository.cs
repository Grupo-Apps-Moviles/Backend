namespace Backend_Frock.Shared.Domain.Repositories;

public interface IBaseStringRepository<TEntity>
{
    Task AddAsync(TEntity entity);
    Task<TEntity?> FindByIdAsync(string id);
    Task<TEntity?> FindByIdIntAsync(int id);
    void Update(TEntity entity);
    void Remove(TEntity entity);
    Task<IEnumerable<TEntity>> ListAsync();
}