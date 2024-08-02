using Todo.Application.Persistence;

namespace Todo.Infrastructure.Database;

public class UnitOfWork(TodoApiDbContext context) : IUnitOfWork
{
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}
