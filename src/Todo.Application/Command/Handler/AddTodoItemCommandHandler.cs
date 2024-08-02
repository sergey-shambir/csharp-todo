using MediatR;
using Todo.Application.Exceptions;
using Todo.Domain.Model;
using Todo.Domain.Repository;

namespace Todo.Application.Command.Handler;

public class AddTodoItemCommandHandler(ITodoListRepository repository) : IRequestHandler<AddTodoItemCommand, int>
{
    public async Task<int> Handle(AddTodoItemCommand request, CancellationToken cancellationToken)
    {
        TodoList? list = await repository.FindByIdAsync(request.ListId);
        if (list == null)
        {
            throw new EntityNotFoundException($"Cannot find todo list with id={request.ListId}");
        }

        int position = list.AddItem(request.Title);
        repository.Update(list);

        return position;
    }
}
