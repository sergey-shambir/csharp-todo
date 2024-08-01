namespace TodoApi.Application.Data;

public record class EditTodoItemParams(string? Title = null, bool? IsCompleted = null, int? Position = null);
