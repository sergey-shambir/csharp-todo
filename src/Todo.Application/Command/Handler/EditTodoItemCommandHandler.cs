using MediatR;
using Todo.Application.Exceptions;
using Todo.Application.Persistence;
using Todo.Domain.Model;
using Todo.Domain.Repository;

namespace Todo.Application.Command.Handler;

public class EditTodoItemCommandHandler(ITodoListRepository repository, IUnitOfWork unitOfWork) : IRequestHandler<EditTodoItemCommand>
{
    public async Task Handle(EditTodoItemCommand request, CancellationToken cancellationToken)
    {
        TodoList? list = await repository.FindByIdAsync(request.ListId);
        if (list == null)
        {
            throw new EntityNotFoundException($"Cannot find todo list with id={request.ListId}");
        }

        if (request.Title != null)
        {
            list.ChangeItemTitle(request.Position, request.Title);
        }
        if (request.IsCompleted != null)
        {
            list.ChangeItemStatus(request.Position, (bool)request.IsCompleted);
        }
        if (request.NewPosition != null)
        {   
            list.MoveItem(request.Position, (int)request.NewPosition);
        }
        repository.Update(list);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
