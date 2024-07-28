using Microsoft.EntityFrameworkCore;

namespace TodoApi.Models;

public class TodoApiDbContext(DbContextOptions<TodoApiDbContext> options) : DbContext(options)
{
    public DbSet<TodoItem> TodoItems { get; set; } = null!;
}
