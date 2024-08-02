using MediatR;

namespace Todo.Application.Command;

public class EditTodoItemCommand(int listId, int position, string? title = null, bool? isCompleted = null, int? newPosition = null) : IRequest
{
    public int ListId { get; } = listId;
    public int Position { get; } = position;
    public string? Title { get; } = title;
    public bool? IsCompleted { get; } = isCompleted;
    public int? NewPosition { get; } = newPosition;
}
