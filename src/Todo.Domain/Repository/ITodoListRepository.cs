using Todo.Domain.Model;

namespace Todo.Domain.Repository;

public interface ITodoListRepository
{
    Task<TodoList?> FindByIdAsync(int id);
    void Add(TodoList list);
    void Update(TodoList list);
    void Delete(TodoList list);
}
