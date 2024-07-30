using Microsoft.EntityFrameworkCore;
using TodoApi.Application.Data;

namespace TodoApi.Infrastructure.Query;

public class GetTodoListQueryHandler(TodoApiDbContext context)
{
    private readonly TodoApiDbContext _context = context;

    public Task<TodoListDetailedData?> Get(int listId)
    {
        return _context.TodoLists
            .Where(list => list.Id == listId)
            .Select(
                list => new TodoListDetailedData(
                    list.Id,
                    list.Name,
                    list.Items.Select(
                        item => new TodoItemData(
                            item.Position,
                            item.Title,
                            item.IsComplete
                        )
                    ).ToArray()
                )
            )
            .AsNoTracking()
            .SingleOrDefaultAsync();
    }
}
