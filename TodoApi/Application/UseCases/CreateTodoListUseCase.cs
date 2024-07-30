using TodoApi.Application.Persistence;
using TodoApi.Domain;
using TodoApi.Infrastructure.Persistence;

namespace TodoApi.Application.UseCases;

public class CreateTodoListUseCase(IUnitOfWork unitOfWork, TodoListRepository repository)
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly TodoListRepository _repository = repository;

    public async Task<int> Create(string name)
    {
        TodoList list = new(name);
        _repository.Add(list);
        await _unitOfWork.SaveChangesAsync();

        return list.Id;
    }
}
