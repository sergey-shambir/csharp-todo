using Microsoft.EntityFrameworkCore;
using TodoApi.Domain;

namespace TodoApi.Infrastructure.Persistence;

public class TodoListRepository(TodoApiDbContext context) : AbstractEfRepository<TodoList>(context)
{
    public override Task<TodoList?> FindByIdAsync(int id)
    {
        return _context.Set<TodoList>()
            .Include(list => list.Items.OrderBy(item => item.Position))
            .Where(item => item.Id == id)
            .FirstOrDefaultAsync();
    }

    public override Task<List<TodoList>> ListAll()
    {
        return _context.Set<TodoList>()
            .OrderBy(list => list.Id)
            .Include(list => list.Items.OrderBy(item => item.Position))
            .ToListAsync();
    }
}
