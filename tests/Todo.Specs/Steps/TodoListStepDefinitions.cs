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

    [Given(@"(?:я )?создал список ""(.*)""")]
    public async Task ПустьЯСоздалСписок(string name)
    {
        var list = await _gateway.CreateTodoList(name);
        _lastListId = list.Id;
    }

    [Given(@"(?:я )?добавил задачу ""(.*)""")]
    public async Task ПустьЯДобавилЗадачу(string title)
    {
        await _gateway.AddTodoItem(LastListId, title);
    }

    [Given(@"(?:я )?переместил задачу №(.+) на позицию №(.+)")]
    public async Task ПустьЯПереместилЗадачу(int position, int newPosition)
    {
        await _gateway.EditTodoItem(LastListId, position, new EditTodoItemParams(Position: newPosition));
    }

    [Given(@"(?:я )?переименовал задачу №(.+) на ""(.+)""")]
    public async Task ПустьЯПереименовалЗадачуНа(int position, string newTitle)
    {
        await _gateway.EditTodoItem(LastListId, position, new EditTodoItemParams(Title: newTitle));
    }

    [Given("(?:я )?завершил задачу №(.+)")]
    public async Task ПустьЯЗавершилЗадачу(int position)
    {
        await _gateway.EditTodoItem(LastListId, position, new EditTodoItemParams(IsCompleted: true));
    }

    [Given("(?:я )?удалил задачу №(.+)")]
    public async Task ПустьЯУдалилЗадачу(int position)
    {
        await _gateway.DeleteTodoItem(LastListId, position);
    }

    [When("(?:я )?открыл созданный список")]
    public async Task КогдаЯОткрылСозданныйСписок()
    {
        _lastTodoList = await _gateway.GetTodoList(LastListId);
    }

    [Then(@"(?:я )?вижу задачи: ""(.*)""")]
    public void ТогдаЯВижуЗадачи(string tasksTitlesCommaSeparated)
    {
        string[] taskTitles = SplitCommaSeparatedList(tasksTitlesCommaSeparated);
        Assert.Equal(taskTitles, GetItemTitles(LastTodoList));
    }

    [Then(@"(?:я )?вижу завершённые задачи: ""(.*)""")]
    public void ТогдаЯВижуЗавершённыеЗадачи(string tasksTitlesCommaSeparated)
    {
        string[] taskTitles = SplitCommaSeparatedList(tasksTitlesCommaSeparated);
        Assert.Equal(taskTitles, GetCompletedItemTitles(LastTodoList));
    }

    private static string[] SplitCommaSeparatedList(string text)
    {
        return text.Split(",").Select(x => x.Trim()).Where(x => x != "").ToArray();
    }

    private static string[] GetItemTitles(TodoListDetailedData list)
    {
        return list.Items.Select(item => item.Title).ToArray();
    }

    private static string[] GetCompletedItemTitles(TodoListDetailedData list)
    {
        return list.Items.Where(item => item.IsComplete).Select(item => item.Title).ToArray();
    }
}
