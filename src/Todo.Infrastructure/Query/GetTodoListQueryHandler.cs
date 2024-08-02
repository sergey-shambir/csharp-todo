using MediatR;
using Microsoft.EntityFrameworkCore;
using Todo.Application.Data;
using Todo.Application.Query;
using Todo.Infrastructure.Database;

namespace Todo.Infrastructure.Query;

public class GetTodoListQueryHandler(TodoApiDbContext context): IRequestHandler<GetTodoListQuery,TodoListDetailedData?>
{
    public async Task<TodoListDetailedData?> Handle(GetTodoListQuery request, CancellationToken cancellationToken)
    {
        var list = await context.TodoLists
            .Where(list => list.Id == request.ListId)
            .Include(list => list.Items.OrderBy(item => item.Position))
            .AsNoTracking()
            .SingleOrDefaultAsync(cancellationToken: cancellationToken);
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
