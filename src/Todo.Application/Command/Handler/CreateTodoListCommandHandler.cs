using MediatR;
using Todo.Application.Persistence;
using Todo.Domain.Model;
using Todo.Domain.Repository;

namespace Todo.Application.Command.Handler;

public class CreateTodoListCommandHandler(ITodoListRepository repository, IUnitOfWork unitOfWork) : IRequestHandler<CreateTodoListCommand, int>
{
    public async Task<int> Handle(CreateTodoListCommand request, CancellationToken cancellationToken)
    {
        TodoList list = new(request.Name);
        repository.Add(list);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return list.Id;
    }
}
