using TodoApi.Application.Exceptions;
using TodoApi.Application.Persistence;
using TodoApi.Domain;
using TodoApi.Infrastructure.Persistence;

namespace TodoApi.Application.UseCases;

public class AddTodoItemUseCase(IUnitOfWork unitOfWork, TodoListRepository repository)
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly TodoListRepository _repository = repository;

    public async Task<int> Add(int listId, string title)
    {
        TodoList? list = await _repository.FindByIdAsync(listId);
        if (list == null)
        {
            throw new EntityNotFoundException($"Cannot find todo list with id={listId}");
        }

        int position = list.AddItem(title);
        _repository.Update(list);
        await _unitOfWork.SaveChangesAsync();

        return position;
    }
}
