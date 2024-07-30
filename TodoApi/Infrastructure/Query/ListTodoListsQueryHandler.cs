using Microsoft.EntityFrameworkCore;
using TodoApi.Application.Data;

namespace TodoApi.Infrastructure.Query;

public class ListTodoListsQueryHandler(TodoApiDbContext context)
{
    private readonly TodoApiDbContext _context = context;

    public Task<TodoListData[]> List()
    {
        return _context.TodoLists
            .Select(list => new TodoListData(list.Id, list.Name))
            .AsNoTracking()
            .ToArrayAsync();
    }
}
