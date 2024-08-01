using TodoApi.Application.Persistence;
using TodoApi.Domain;
using TodoApi.Infrastructure.Persistence;

namespace TodoApi.Application.UseCases;

public class DeleteTodoItemUseCase(IUnitOfWork unitOfWork, TodoListRepository repository)
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly TodoListRepository _repository = repository;

    public async Task Delete(int listId, int position)
    {
        TodoList? list = await _repository.FindByIdAsync(listId);
        if (list == null)
        {
            return;
        }

        list.RemoveItem(position);
        _repository.Update(list);
        await _unitOfWork.SaveChangesAsync();
    }
}
