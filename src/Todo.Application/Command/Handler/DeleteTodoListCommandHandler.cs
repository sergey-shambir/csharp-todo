using MediatR;
using Todo.Application.Persistence;
using Todo.Domain.Model;
using Todo.Domain.Repository;

namespace Todo.Application.Command.Handler;

public class DeleteTodoListCommandHandler(ITodoListRepository repository, IUnitOfWork unitOfWork) : IRequestHandler<DeleteTodoListCommand>
{
    public async Task Handle(DeleteTodoListCommand request, CancellationToken cancellationToken)
    {
        TodoList? list = await repository.FindByIdAsync(request.ListId);
        if (list != null)
        {
            repository.Delete(list);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
