using MediatR;
using Todo.Application.Data;

namespace Todo.Application.Query;

public record GetTodoListQuery(int ListId): IRequest<TodoListDetailedData?>;
