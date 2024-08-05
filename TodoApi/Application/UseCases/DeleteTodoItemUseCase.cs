using TodoApi.Application.Persistence;
using TodoApi.Domain;
using TodoApi.Domain.Repository;

namespace TodoApi.Application.UseCases;

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
