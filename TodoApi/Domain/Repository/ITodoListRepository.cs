using TodoApi.Domain;

namespace TodoApi.Domain.Repository;

public interface ITodoListRepository
{
    Task<TodoList?> FindByIdAsync(int id);
    void Add(TodoList list);
    void Update(TodoList list);
    void Delete(TodoList list);
}
