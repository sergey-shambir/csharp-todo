using Todo.Domain.Model;
using Todo.Application.Persistence;
using Todo.Domain.Repository;

namespace Todo.Application.UseCases;

public class DeleteTodoItemUseCase(IUnitOfWork unitOfWork, ITodoListRepository repository)
{
    public async Task Delete(int listId, int position)
    {
        TodoList? list = await repository.FindByIdAsync(listId);
        if (list == null)
        {
            return;
        }

        list.RemoveItem(position);
        repository.Update(list);
        await unitOfWork.SaveChangesAsync();
    }
}
