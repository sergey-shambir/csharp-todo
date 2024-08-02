using MediatR;

namespace Todo.Application.Command;

public class CreateTodoListCommand(string Name) : IRequest<int>
{
    public string Name { get; } = Name;
}
