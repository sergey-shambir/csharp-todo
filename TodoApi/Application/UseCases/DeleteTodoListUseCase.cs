using TodoApi.Application.Persistence;
using TodoApi.Domain;
using TodoApi.Domain.Repository;

namespace TodoApi.Application.UseCases;

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
