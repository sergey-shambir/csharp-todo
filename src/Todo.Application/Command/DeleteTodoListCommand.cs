using MediatR;

namespace Todo.Application.Command;

public class DeleteTodoListCommand(int listId) : IRequest
{
    public int ListId { get; } = listId;
}
