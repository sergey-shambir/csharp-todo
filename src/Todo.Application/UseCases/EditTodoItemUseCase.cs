using Todo.Application.Data;
using Todo.Application.Exceptions;
using Todo.Application.Persistence;
using Todo.Domain.Model;
using Todo.Domain.Repository;

namespace Todo.Application.UseCases;

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