using Microsoft.AspNetCore.Mvc;
using Todo.Application.Data;
using Todo.Application.UseCases;
using Todo.Infrastructure;
using Todo.Infrastructure.Database;
using Todo.Infrastructure.Persistence;
using Todo.Infrastructure.Query;

namespace Todo.Api.Controllers;

[Route("api/todo-list")]
[ApiController]
public class TodoListController(TodoApiDbContext context) : ControllerBase
{
    private readonly TodoApiDbContext _context = context;
    private readonly TodoListRepository _repository = new(context);

    [HttpGet]
    public async Task<IReadOnlyList<TodoListData>> ListTodoLists(string? search)
    {
        SearchTodoListsQueryHandler handler = new(_context);
        return await handler.Search(search);
    }

    [HttpGet("{listId}")]
    public async Task<ActionResult<TodoListDetailedData>> GetTodoList(int listId)
    {
        GetTodoListQueryHandler handler = new(_context);
        TodoListDetailedData? list = await handler.Get(listId);
        if (list == null)
        {
            return NotFound();
        }
        return list;
    }

    [HttpPost]
    public async Task<ActionResult<int>> CreateTodoList(CreateTodoListRequest request)
    {
        CreateTodoListUseCase useCase = new(_context, _repository);
        int listId = await useCase.Create(request.Name);

        return CreatedAtAction(nameof(GetTodoList), new { listId }, listId);
    }

    [HttpPost("{listId}")]
    public async Task<ActionResult<int>> AddTodoItem(int listId, AddTodoItemRequest request)
    {
        AddTodoItemUseCase useCase = new(_context, _repository);
        int position = await useCase.Add(listId, request.Title);

        return position;
    }

    [HttpPatch("{listId:int}/{position:int}")]
    public async Task<ActionResult> EditTodoItem(int listId, int position, EditTodoItemParams itemParams)
    {
        EditTodoItemUseCase useCase = new(_context, _repository);
        await useCase.Edit(listId, position, itemParams);
        return Ok();
    }

    [HttpDelete("{listId:int}/{position:int}")]
    public async Task<ActionResult> DeleteTodoItem(int listId, int position)
    {
        DeleteTodoItemUseCase useCase = new(_context, _repository);
        await useCase.Delete(listId, position);
        return NoContent();
    }

    [HttpDelete("{listId:int}")]
    public async Task<ActionResult> DeleteList(int listId)
    {
        DeleteTodoListUseCase useCase = new(_context, _repository);
        await useCase.Delete(listId);
        return NoContent();
    }

    public record class CreateTodoListRequest(string Name);
    public record class AddTodoItemRequest(string Title);
}
