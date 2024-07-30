using TodoApi.Application.Data;
using TodoApi.Application.Exceptions;
using TodoApi.Application.Persistence;
using TodoApi.Domain;
using TodoApi.Infrastructure.Persistence;

namespace TodoApi.Application.UseCases;

public class EditTodoItemUseCase(IUnitOfWork unitOfWork, TodoListRepository repository)
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly TodoListRepository _repository = repository;

    public async Task Edit(int listId, int position, EditTodoItemParams itemParams)
    {
        TodoList? list = await _repository.FindByIdAsync(listId);
        if (list == null)
        {
            throw new EntityNotFoundException($"Cannot find todo list with id={listId}");
        }

        if (itemParams.Title != null)
        {
            list.ChangeItemTitle(position, itemParams.Title);
        }
        if (itemParams.IsCompleted != null)
        {
            list.ChangeItemStatus(position, (bool)itemParams.IsCompleted);
        }
        if (itemParams.Position != null)
        {
            list.MoveItem(position, (int)itemParams.Position);
        }
        _repository.Update(list);
        await _unitOfWork.SaveChangesAsync();
    }
}
