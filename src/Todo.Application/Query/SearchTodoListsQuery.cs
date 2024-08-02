using MediatR;
using Todo.Application.Data;

namespace Todo.Application.Query;

public record SearchTodoListsQuery(string? SearchQuery): IRequest<IReadOnlyList<TodoListData>>;
