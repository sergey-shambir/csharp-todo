using Microsoft.AspNetCore.Mvc;
using Todo.Application.Data;
using Todo.Application.Services;
using Todo.Infrastructure.Database;
using Todo.Infrastructure.Persistence;
using Todo.Infrastructure.Query;

namespace Todo.Api.Controllers;

[Route("api/todo-list")]
[ApiController]
public class TodoListController(TodoApiDbContext context) : ControllerBase
{
    private readonly TodoListService _service = new(context, new TodoListRepository(context));
    private readonly TodoListQueryService _queryService = new(context);

    [HttpGet]
    public async Task<IReadOnlyList<TodoListData>> ListTodoLists(string? search)
    {
        return await _queryService.SearchTodoLists(search);
    }

    [HttpGet("{listId:int}")]
    public async Task<ActionResult<TodoListDetailedData>> GetTodoList(int listId)
    {
        TodoListDetailedData? list = await _queryService.FindTodoList(listId);
        if (list == null)
        {
            return NotFound();
        }
        return list;
    }

    [HttpPost]
    public async Task<ActionResult<int>> CreateTodoList(CreateTodoListRequest request)
    {
        int listId = await _service.CreateTodoList(request.Name);

        return CreatedAtAction(nameof(GetTodoList), new { listId }, listId);
    }

    [HttpPost("{listId:int}")]
    public async Task<ActionResult<int>> AddTodoItem(int listId, AddTodoItemRequest request)
    {
        int position = await _service.AddTodoItem(listId, request.Title);

        return position;
    }

    [HttpPatch("{listId:int}/{position:int}")]
    public async Task<ActionResult> EditTodoItem(int listId, int position, EditTodoItemParams itemParams)
    {
        await _service.EditTodoItem(listId, position, itemParams);
        return Ok();
    }

    [HttpDelete("{listId:int}/{position:int}")]
    public async Task<ActionResult> DeleteTodoItem(int listId, int position)
    {
        await _service.DeleteTodoItem(listId, position);
        return NoContent();
    }

    [HttpDelete("{listId:int}")]
    public async Task<ActionResult> DeleteList(int listId)
    {
        await _service.DeleteTodoList(listId);
        return NoContent();
    }

    public record CreateTodoListRequest(string Name);
    public record AddTodoItemRequest(string Title);
}
