using TodoApi.Application.Data;
using TodoApi.Application.Exceptions;
using TodoApi.Application.Persistence;
using TodoApi.Domain;
using TodoApi.Domain.Repository;

namespace TodoApi.Application.UseCases;

public class EditTodoItemUseCase(IUnitOfWork unitOfWork, ITodoListRepository repository)
{
    public async Task Edit(int listId, int position, EditTodoItemParams itemParams)
    {
        TodoList? list = await repository.FindByIdAsync(listId);
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
        repository.Update(list);
        await unitOfWork.SaveChangesAsync();
    }
}
