using Todo.Application.Exceptions;
using Todo.Application.Persistence;
using Todo.Domain.Model;
using Todo.Domain.Repository;

namespace Todo.Application.UseCases;

public class AddTodoItemUseCase(IUnitOfWork unitOfWork, ITodoListRepository repository)
{
    public async Task<int> Add(int listId, string title)
    {
        TodoList? list = await repository.FindByIdAsync(listId);
        if (list == null)
        {
            throw new EntityNotFoundException($"Cannot find todo list with id={listId}");
        }

        int position = list.AddItem(title);
        repository.Update(list);
        await unitOfWork.SaveChangesAsync();

        return position;
    }
}
