
using Todo.Application.Data;
using Todo.IntegrationTests.Common;
using Todo.IntegrationTests.Gateways;

namespace Todo.IntegrationTests;

public class TodoListApiTest(IntegrationTestFixture<Program> fixture) : IClassFixture<IntegrationTestFixture<Program>>
{
    private readonly TodoListTestApiGateway _gateway = new(fixture.HttpClient);

    [Fact]
    public async Task TestCreateEditDeleteTwoLists()
    {
        TodoListDetailedData todoHome = await _gateway.CreateTodoList("Home");
        await _gateway.AddTodoItem(todoHome.Id, "Clean the room");
        await _gateway.AddTodoItem(todoHome.Id, "Eat borscht");
        await _gateway.AddTodoItem(todoHome.Id, "Make borscht");
        await _gateway.AddTodoItem(todoHome.Id, "Make a milkshake");

        TodoListDetailedData todoShop = await _gateway.CreateTodoList("Shopping list");
        await _gateway.AddTodoItem(todoShop.Id, "Milk");
        await _gateway.AddTodoItem(todoShop.Id, "Croissant");
        await _gateway.EditTodoItem(todoShop.Id, 0, new EditTodoItemParams(Title: "Fresh milk"));

        Assert.Equal(["Home", "Shopping list"], GetTodoListsNames(await _gateway.ListTodoLists()));

        Assert.Equal(["Home"], GetTodoListsNames(await _gateway.ListTodoLists("borscht")));
        Assert.Equal(["Home", "Shopping list"], GetTodoListsNames(await _gateway.ListTodoLists("milk")));
        Assert.Equal([], GetTodoListsNames(await _gateway.ListTodoLists("irony")));

        todoHome = await _gateway.GetTodoList(todoHome.Id);
        Assert.Equal("Home", todoHome.Name);
        Assert.Equal(["Clean the room", "Eat borscht", "Make borscht", "Make a milkshake"], GetItemTitles(todoHome));

        todoShop = await _gateway.GetTodoList(todoShop.Id);
        Assert.Equal("Shopping list", todoShop.Name);
        Assert.Equal(["Fresh milk", "Croissant"], GetItemTitles(todoShop));

        await _gateway.DeleteTodoItem(todoShop.Id, 0);
        await _gateway.DeleteTodoList(todoHome.Id);

        Assert.Equal(["Shopping list"], GetTodoListsNames(await _gateway.ListTodoLists()));

        todoShop = await _gateway.GetTodoList(todoShop.Id);
        Assert.Equal("Shopping list", todoShop.Name);
        Assert.Equal(["Croissant"], GetItemTitles(todoShop));
    }

    private static string[] GetTodoListsNames(TodoListData[] lists)
    {
        return lists.Select(item => item.Name).ToArray();
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
