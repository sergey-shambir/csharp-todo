using Microsoft.EntityFrameworkCore;
using Todo.Application.Persistence;
using Todo.Domain.Model;

namespace Todo.Infrastructure.Database;

public class TodoApiDbContext(DbContextOptions<TodoApiDbContext> options) : DbContext(options)
{
    public DbSet<TodoList> TodoLists { get; init; } = null!;

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
