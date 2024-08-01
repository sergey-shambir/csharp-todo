namespace Todo.Application.Persistence;

public interface IUnitOfWork
{
    public Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
