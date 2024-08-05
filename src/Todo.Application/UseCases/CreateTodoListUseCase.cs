using Todo.Application.Persistence;
using Todo.Domain.Model;
using Todo.Domain.Repository;

namespace Todo.Application.UseCases;

public class CreateTodoListUseCase(IUnitOfWork unitOfWork, ITodoListRepository repository)
{
    public async Task<int> Create(string name)
    {
        TodoList list = new(name);
        repository.Add(list);
        await unitOfWork.SaveChangesAsync();

        return list.Id;
    }
}