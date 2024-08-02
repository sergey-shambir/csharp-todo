using MediatR;
using Microsoft.AspNetCore.Mvc;
using Todo.Application.Command;
using Todo.Application.Data;
using Todo.Application.Query;

namespace Todo.Api.Controllers;

[Route("api/todo-list")]
[ApiController]
public class TodoListController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IReadOnlyList<TodoListData>> ListTodoLists(string? search)
    {
        return await mediator.Send(new SearchTodoListsQuery(search));
    }

    [HttpGet("{listId:int}")]
    public async Task<ActionResult<TodoListDetailedData>> GetTodoList(int listId)
    {
        TodoListDetailedData? list = await mediator.Send(new GetTodoListQuery(listId));
        if (list == null)
        {
            return NotFound();
        }
        return list;
    }

    [HttpPost]
    public async Task<ActionResult<int>> CreateTodoList(CreateTodoListRequest request)
    {
        int listId = await mediator.Send(new CreateTodoListCommand(request.Name));
        return CreatedAtAction(nameof(GetTodoList), new { listId }, listId);
    }

    [HttpPost("{listId:int}")]
    public async Task<ActionResult<int>> AddTodoItem(int listId, AddTodoItemRequest request)
    {
        return await mediator.Send(new AddTodoItemCommand(listId, request.Title));
    }

    [HttpPatch("{listId:int}/{position:int}")]
    public async Task<ActionResult> EditTodoItem(int listId, int position, EditTodoItemRequest request)
    {
        await mediator.Send(new EditTodoItemCommand(listId, position, request.Title, request.IsCompleted, request.NewPosition));
        return Ok();
    }

    [HttpDelete("{listId:int}/{position:int}")]
    public async Task<ActionResult> DeleteTodoItem(int listId, int position)
    {
        await mediator.Send(new DeleteTodoItemCommand(listId, position));
        return NoContent();
    }

    [HttpDelete("{listId:int}")]
    public async Task<ActionResult> DeleteList(int listId)
    {
        await mediator.Send(new DeleteTodoListCommand(listId));
        return NoContent();
    }

    public record EditTodoItemRequest(string? Title = null, bool? IsCompleted = null, int? NewPosition = null);
    public record CreateTodoListRequest(string Name);
    public record AddTodoItemRequest(string Title);
}
