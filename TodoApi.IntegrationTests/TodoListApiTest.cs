
using TodoApi.Application.Data;
using TodoApi.IntegrationTests.Common;
using TodoApi.IntegrationTests.Gateways;

namespace TodoApi.IntegrationTests;

public class TodoListApiTest(IntegrationTestFixture<Program> fixture) : IClassFixture<IntegrationTestFixture<Program>>
{
    private readonly IntegrationTestFixture<Program> _fixture = fixture;
    private readonly TodoListTestApiGateway _gateway = new(fixture.HttpClient);

    [Fact]
    public async Task TestSingleListLifecycle()
    {
        TodoListDetailedData list = await _gateway.CreateTodoList("Home");
        Assert.Equal("Home", list.Name);

        await _gateway.AddTodoItem(list.Id, "Clean the room");
        await _gateway.AddTodoItem(list.Id, "Eat borscht");
        await _gateway.AddTodoItem(list.Id, "Make borscht");
        await _gateway.AddTodoItem(list.Id, "Feed the hamster");

        list = await _gateway.GetTodoList(list.Id);
        Assert.Equal(["Clean the room", "Eat borscht", "Make borscht", "Feed the hamster"], GetItemTitles(list));

        await _gateway.EditTodoItem(list.Id, 3, new EditTodoItemParams(Position: 2));

        list = await _gateway.GetTodoList(list.Id);
        Assert.Equal(["Clean the room", "Eat borscht", "Feed the hamster", "Make borscht"], GetItemTitles(list));

        await _gateway.EditTodoItem(list.Id, 1, new EditTodoItemParams(Position: 3, Title: "Eat borscht with soup cream"));

        list = await _gateway.GetTodoList(list.Id);
        Assert.Equal(["Clean the room", "Feed the hamster", "Make borscht", "Eat borscht with soup cream"], GetItemTitles(list));
        Assert.Equal([], GetCompletedItemPositions(list));

        await _gateway.EditTodoItem(list.Id, 0, new EditTodoItemParams(IsCompleted: true));
        await _gateway.EditTodoItem(list.Id, 2, new EditTodoItemParams(IsCompleted: true));

        list = await _gateway.GetTodoList(list.Id);
        Assert.Equal(["Clean the room", "Feed the hamster", "Make borscht", "Eat borscht with soup cream"], GetItemTitles(list)); // nothing changed
        Assert.Equal([0, 2], GetCompletedItemPositions(list));

        await _gateway.DeleteTodoItem(list.Id, 1);
        
        list = await _gateway.GetTodoList(list.Id);
        Assert.Equal(["Clean the room", "Make borscht", "Eat borscht with soup cream"], GetItemTitles(list));
        Assert.Equal([0, 1], GetCompletedItemPositions(list));

        await _gateway.DeleteTodoList(list.Id);
    }

    private static string[] GetItemTitles(TodoListDetailedData list)
    {
        return list.Items.Select(item => item.Title).ToArray();
    }

    private static int[] GetCompletedItemPositions(TodoListDetailedData list)
    {
        return list.Items.Where(item => item.IsComplete).Select(item => item.Position).ToArray();
    }
}
