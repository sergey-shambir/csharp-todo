using Microsoft.EntityFrameworkCore;
using TodoApi.Domain;
using TodoApi.Domain.Repository;

namespace TodoApi.Infrastructure.Persistence;

public class TodoListRepository(TodoApiDbContext context) : ITodoListRepository
{
    public Task<TodoList?> FindByIdAsync(int id)
    {
        return context.Set<TodoList>()
            .Include(list => list.Items.OrderBy(item => item.Position))
            .Where(item => item.Id == id)
            .FirstOrDefaultAsync();
    }

    public void Add(TodoList list)
    {
        context.TodoLists.Add(list);
    }

    public void Update(TodoList list)
    {
        context.TodoLists.Update(list);
    }

    public void Delete(TodoList list)
    {
        context.TodoLists.Remove(list);
    }
}