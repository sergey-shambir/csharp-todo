using Microsoft.EntityFrameworkCore;
using TodoApi.Application.Persistence;
using TodoApi.Domain;

namespace TodoApi.Infrastructure;

public class TodoApiDbContext(DbContextOptions<TodoApiDbContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<TodoList> TodoLists { get; set; } = null!;

    async Task IUnitOfWork.SaveChangesAsync(CancellationToken cancellationToken)
    {
        await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSnakeCaseNamingConvention();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}
