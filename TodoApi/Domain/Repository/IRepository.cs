namespace TodoApi.Domain.Repository;

public interface IRepository<TEntity>
where TEntity : class
{
    Task<TEntity?> FindByIdAsync(int id);

    Task<List<TEntity>> ListAll();

    void Add(TEntity entity);

    void Update(TEntity entity);

    void Delete(TEntity entity);
}
