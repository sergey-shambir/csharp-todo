namespace Todo.Application.Data;

public record EditTodoItemParams(string? Title = null, bool? IsCompleted = null, int? Position = null);
