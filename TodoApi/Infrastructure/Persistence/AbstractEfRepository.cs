using Microsoft.EntityFrameworkCore;
using TodoApi.Domain.Repository;

namespace TodoApi.Infrastructure.Persistence;

public abstract class AbstractEfRepository<TEntity>(TodoApiDbContext context) : IRepository<TEntity>
where TEntity : class
{
    protected readonly TodoApiDbContext _context = context;

    public virtual Task<TEntity?> FindByIdAsync(int id)
    {
        return _context.Set<TEntity>().FindAsync(id).AsTask();
    }

    public virtual Task<List<TEntity>> ListAll()
    {
        return _context.Set<TEntity>().ToListAsync();
    }

    public void Add(TEntity entity)
    {
        _context.Set<TEntity>().Add(entity);
    }

    public void Update(TEntity entity)
    {
        _context.Set<TEntity>().Update(entity);
    }

    public void Delete(TEntity entity)
    {
        _context.Set<TEntity>().Remove(entity);
    }
}
