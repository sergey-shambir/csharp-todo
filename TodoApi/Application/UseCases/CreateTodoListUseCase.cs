using TodoApi.Application.Persistence;
using TodoApi.Domain;
using TodoApi.Domain.Repository;

namespace TodoApi.Application.UseCases;

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