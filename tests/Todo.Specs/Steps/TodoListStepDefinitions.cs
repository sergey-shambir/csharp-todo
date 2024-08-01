using TechTalk.SpecFlow;
using Todo.Application.Data;
using Todo.Specs.Drivers;
using Todo.Specs.Gateways;
using Xunit;

namespace Todo.Specs.Steps;

[Binding]
public sealed class TodoListStepDefinitions(TestServerDriver driver)
{
    private readonly TestServerDriver _driver = driver;
    private readonly TodoListTestApiGateway _gateway = new(driver.HttpClient);

    private int? _lastListId;
    private TodoListDetailedData? _lastTodoList;

    private int LastListId
    {
        get => _lastListId ?? throw new InvalidOperationException("Список дел не ещё создан");
    }

    private TodoListDetailedData LastTodoList
    {
        get => _lastTodoList ?? throw new InvalidOperationException("Список дел ещё не открыт");
    }

    [Given(@"я создал список ""(.*)""")]
    public async Task ПустьЯСоздалСписок(string name)
    {
        var list = await _gateway.CreateTodoList(name);
        _lastListId = list.Id;
    }

    [Given(@"добавил задачу ""(.*)""")]
    public async Task ПустьЯДобавилЗадачу(string title)
    {
        await _gateway.AddTodoItem(LastListId, title);
    }

    [When("я открыл созданный список")]
    public async Task КогдаЯОткрылСозданныйСписок()
    {
        _lastTodoList = await _gateway.GetTodoList(LastListId);
    }

    [Then(@"вижу задачи: ""(.*)""")]
    public void ТогдаЯВижуЗадачи(string tasksTitlesCommaSeparated)
    {
        string[] taskTitles = tasksTitlesCommaSeparated
            .Split(",")
            .Select(x => x.Trim())
            .ToArray();

        Assert.Equal(taskTitles, GetItemTitles(LastTodoList));
    }

    private static string[] GetItemTitles(TodoListDetailedData list)
    {
        return list.Items.Select(item => item.Title).ToArray();
    }
}
