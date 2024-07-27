using Microsoft.EntityFrameworkCore;

namespace TodoApi.Models;

public class TodoApiContext(DbContextOptions<TodoApiContext> options) : DbContext(options)
{
    public DbSet<TodoItem> TodoItems { get; set; } = null!;
}