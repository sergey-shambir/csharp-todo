using Microsoft.EntityFrameworkCore;
using Todo.Application.Data;
using Todo.Infrastructure.Database;

namespace Todo.Infrastructure.Query;

public class GetTodoListQueryHandler(TodoApiDbContext context)
{
    public async Task<TodoListDetailedData?> Get(int listId)
    {
        var list = await context.TodoLists
            .Where(list => list.Id == listId)
            .Include(list => list.Items.OrderBy(item => item.Position))
            .AsNoTracking()
            .SingleOrDefaultAsync();
        if (list == null)
        {
            return null;
        }

        return new TodoListDetailedData(
            list.Id,
            list.Name,
            list.Items.Select(
                item => new TodoItemData(
                    item.Position,
                    item.Title,
                    item.IsComplete
                )
            ).ToArray()
        );
    }
}
