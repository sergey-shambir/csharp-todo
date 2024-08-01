using Microsoft.EntityFrameworkCore;
using Todo.Application.Data;
using Todo.Infrastructure.Database;

namespace Todo.Infrastructure.Query;

public class SearchTodoListsQueryHandler(TodoApiDbContext context)
{
    public async Task<IReadOnlyList<TodoListData>> Search(string? searchQuery)
    {
        return (await BuildSearchQuery(searchQuery).ToListAsync()).AsReadOnly();
    }

    private IQueryable<TodoListData> BuildSearchQuery(string? searchQuery)
    {
        if (searchQuery == null)
            return context.Database.SqlQueryRaw<TodoListData>(
                """
                SELECT
                    tl.id,
                    tl.name
                FROM todo_list tl
                ORDER BY tl.id
                """
            );

        string searchPattern = EscapeLikePattern(searchQuery);
        return context.Database.SqlQueryRaw<TodoListData>(
            """
            SELECT
              tl.id,
              ANY_VALUE(tl.name) AS name
            FROM todo_list tl
              LEFT JOIN todo_item ti ON tl.id = ti.list_id AND ti.title LIKE {0}
            WHERE (tl.name LIKE {0} OR ti.id IS NOT NULL)
            GROUP BY tl.id
            ORDER BY tl.id
            """,
            searchPattern
        );

    }

    private static string EscapeLikePattern(string pattern)
    {
        return "%" + pattern.Replace("%", "\\%").Replace("_", "\\_") + "%";
    }
}