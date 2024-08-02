using MediatR;

namespace Todo.Application.Command;

public class DeleteTodoItemCommand(int listId, int position): IRequest
{
    public int ListId { get; } = listId;
    public int Position { get; } = position;
}
