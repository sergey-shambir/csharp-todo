using TechTalk.SpecFlow;
using Todo.Application.Data;
using Todo.Specs.Fixtures;
using Todo.Specs.Drivers;
using Xunit;

namespace Todo.Specs.Steps;

[Binding]
public sealed class TodoListStepDefinitions(TestServerFixture fixture)
{
    private readonly TestServerFixture _fixture = fixture;
    private readonly TodoListTestDriver _driver = new(fixture.HttpClient);

    private int? _createdListId;
    private TodoListDetailedData? _openedTodoList;
    private TodoListData[]? _lastTodoListSearchResults;

    private int CreatedListId
    {
        get => _createdListId ?? throw new InvalidOperationException("Список дел не ещё создан");
    }

    private TodoListDetailedData OpenedTodoList
    {
        get => _openedTodoList ?? throw new InvalidOperationException("Список дел ещё не открыт");
    }

    private TodoListData[] LastTodoListSearchResults
    {
        get => _lastTodoListSearchResults ?? throw new InvalidOperationException("Поиск списков дел не выполнялся");
    }

    [Given(@"(?:я )?создал список ""(.*)""")]
    public async Task ПустьЯСоздалСписок(string name)
    {
        var list = await _driver.CreateTodoList(name);
        _createdListId = list.Id;
    }

    [Given(@"(?:я )?добавил задачу ""(.*)""")]
    public async Task ПустьЯДобавилЗадачу(string title)
    {
        await _driver.AddTodoItem(CreatedListId, title);
    }

    [Given(@"(?:я )?переместил задачу №(.+) на позицию №(.+)")]
    public async Task ПустьЯПереместилЗадачу(int position, int newPosition)
    {
        await _driver.EditTodoItem(CreatedListId, position, new EditTodoItemParams(Position: newPosition));
    }

    [Given(@"(?:я )?переименовал задачу №(.+) на ""(.+)""")]
    public async Task ПустьЯПереименовалЗадачуНа(int position, string newTitle)
    {
        await _driver.EditTodoItem(CreatedListId, position, new EditTodoItemParams(Title: newTitle));
    }

    [Given("(?:я )?завершил задачу №(.+)")]
    public async Task ПустьЯЗавершилЗадачу(int position)
    {
        await _driver.EditTodoItem(CreatedListId, position, new EditTodoItemParams(IsCompleted: true));
    }

    [Given("(?:я )?удалил задачу №(.+)")]
    public async Task ПустьЯУдалилЗадачу(int position)
    {
        await _driver.DeleteTodoItem(CreatedListId, position);
    }

    [Given(@"я удалил список ""(.*)""")]
    public async Task ПустьЯУдалилСписок(string name)
    {
        var list = LastTodoListSearchResults.First(list => list.Name == name);
        await _driver.DeleteTodoList(list.Id);
    }

    [When("(?:я )?открыл созданный список")]
    public async Task КогдаЯОткрылСозданныйСписок()
    {
        _openedTodoList = await _driver.GetTodoList(CreatedListId);
    }

    [When("(?:я )?открыл списки задач")]
    public async Task КогдаЯОткрылСпискиЗадач()
    {
        _lastTodoListSearchResults = await _driver.ListTodoLists();
    }

    [When(@"(?:я )?открыл списки задач с фильтром ""(.*)""")]
    public async Task КогдаЯОткрылСпискиЗадачСФильтром(string searchQuery)
    {
        _lastTodoListSearchResults = await _driver.ListTodoLists(searchQuery);
    }

    [When(@"я перешёл к списку ""(.*)""")]
    public async Task КогдаЯПерешёлКСписку(string name)
    {
        var list = LastTodoListSearchResults.First(list => list.Name == name);
        _openedTodoList = await _driver.GetTodoList(list.Id);
    }

    [Then(@"(?:я )?вижу задачи: ""(.*)""")]
    public void ТогдаЯВижуЗадачи(string tasksTitlesCommaSeparated)
    {
        string[] taskTitles = SplitCommaSeparatedList(tasksTitlesCommaSeparated);
        Assert.Equal(taskTitles, GetItemTitles(OpenedTodoList));
    }

    [Then(@"(?:я )?вижу завершённые задачи: ""(.*)""")]
    public void ТогдаЯВижуЗавершённыеЗадачи(string tasksTitlesCommaSeparated)
    {
        string[] taskTitles = SplitCommaSeparatedList(tasksTitlesCommaSeparated);
        Assert.Equal(taskTitles, GetCompletedItemTitles(OpenedTodoList));
    }

    [Then(@"(?:я )?вижу списки: ""(.*)""")]
    public void TогдаЯВижуСписки(string listNamesCommaSeparated)
    {
        string[] taskTitles = SplitCommaSeparatedList(listNamesCommaSeparated);
        Assert.Equal(taskTitles, GetListNames(LastTodoListSearchResults));
    }

    private static string[] SplitCommaSeparatedList(string text)
    {
        return text.Split(",").Select(x => x.Trim()).Where(x => x != "").ToArray();
    }

    private static string[] GetItemTitles(TodoListDetailedData list)
    {
        return list.Items.Select(item => item.Title).ToArray();
    }

    private static string[] GetListNames(TodoListData[] lists)
    {
        return lists.Select(item => item.Name).ToArray();
    }

    private static string[] GetCompletedItemTitles(TodoListDetailedData list)
    {
        return list.Items.Where(item => item.IsComplete).Select(item => item.Title).ToArray();
    }
}
