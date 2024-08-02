using MediatR;
using Todo.Application.Persistence;
using Todo.Domain.Model;
using Todo.Domain.Repository;

namespace Todo.Application.Command.Handler;

public class DeleteTodoItemCommandHandler(ITodoListRepository repository, IUnitOfWork unitOfWork) : IRequestHandler<DeleteTodoItemCommand>
{
    public async Task Handle(DeleteTodoItemCommand request, CancellationToken cancellationToken)
    {
        TodoList? list = await repository.FindByIdAsync(request.ListId);
        if (list == null)
        {
            return;
        }

        list.RemoveItem(request.Position);
        repository.Update(list);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
