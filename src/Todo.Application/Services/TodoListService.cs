using Todo.Application.Data;
using Todo.Application.Exceptions;
using Todo.Application.Persistence;
using Todo.Domain.Model;
using Todo.Domain.Repository;

namespace Todo.Application.Services;

public class TodoListService(IUnitOfWork unitOfWork, ITodoListRepository repository)
{
    public async Task<int> CreateTodoList(string name)
    {
        TodoList list = new(name);
        repository.Add(list);
        await unitOfWork.SaveChangesAsync();

        return list.Id;
    }

    public async Task<int> AddTodoItem(int listId, string title)
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

    public async Task EditTodoItem(int listId, int position, EditTodoItemParams itemParams)
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

    public async Task DeleteTodoItem(int listId, int position)
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

    public async Task DeleteTodoList(int listId)
    {
        TodoList? list = await repository.FindByIdAsync(listId);
        if (list != null)
        {
            repository.Delete(list);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
