namespace Todo.Application.Data;

public record TodoListDetailedData(int Id, string Name, TodoItemData[] Items);
