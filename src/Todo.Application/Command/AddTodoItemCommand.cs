using MediatR;

namespace Todo.Application.Command;

public class AddTodoItemCommand(int listId, string title) : IRequest<int>
{
    public int ListId { get; } = listId;
    public string Title { get; } = title;
}
