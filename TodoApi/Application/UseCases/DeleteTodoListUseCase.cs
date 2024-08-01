using TodoApi.Application.Persistence;
using TodoApi.Domain;
using TodoApi.Infrastructure.Persistence;

namespace TodoApi.Application.UseCases;

public class DeleteTodoListUseCase(IUnitOfWork unitOfWork, TodoListRepository repository)
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly TodoListRepository _repository = repository;

    public async Task Delete(int listId)
    {
        TodoList? list = await _repository.FindByIdAsync(listId);
        if (list != null)
        {
            _repository.Delete(list);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
