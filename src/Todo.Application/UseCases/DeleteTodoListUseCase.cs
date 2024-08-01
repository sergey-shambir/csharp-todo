using Todo.Application.Persistence;
using Todo.Domain.Model;
using Todo.Domain.Repository;

namespace Todo.Application.UseCases;

public class DeleteTodoListUseCase(IUnitOfWork unitOfWork, ITodoListRepository repository)
{
    public async Task Delete(int listId)
    {
        TodoList? list = await repository.FindByIdAsync(listId);
        if (list != null)
        {
            repository.Delete(list);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
