namespace TodoApi.Application.Data;

public record class TodoListDetailedData(int Id, string Name, TodoItemData[] Items);
