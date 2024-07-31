using Humanizer;
using Microsoft.CodeAnalysis.Elfie.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using TodoApi.Application.Data;

namespace TodoApi.Infrastructure.Query;

public class SearchTodoListsQueryHandler(TodoApiDbContext context)
{
    private readonly TodoApiDbContext _context = context;

    public Task<TodoListData[]> Search(string? searchQuery)
    {
        return BuildSearchQuery(searchQuery).ToArrayAsync();
    }

    private IQueryable<TodoListData> BuildSearchQuery(string? searchQuery)
    {
        if (searchQuery != null)
        {
            string searchPattern = EscapeLikePattern(searchQuery);
            return _context.Database.SqlQueryRaw<TodoListData>(
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

        return _context.Database.SqlQueryRaw<TodoListData>(
            """
            SELECT
                tl.id,
                tl.name
            FROM todo_list tl
            ORDER BY tl.id
            """
        );
    }

    private static string EscapeLikePattern(string pattern)
    {
        return "%" + pattern.Replace("%", "\\%").Replace("_", "\\_") + "%";
    }
}
