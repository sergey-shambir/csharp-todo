using Todo.Application.Data;

namespace Todo.Application.Query;

public interface ITodoListQueryService
{
    public Task<TodoListDetailedData?> FindTodoList(int listId);

    public Task<IReadOnlyList<TodoListData>> SearchTodoLists(string? searchQuery);
}
