using Microsoft.Build.Framework;
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
    private readonly Context _context = new();

    [Given(@"(?:я )?создал список ""(.*)""")]
    public async Task ПустьЯСоздалСписок(string name)
    {
        var list = await _driver.CreateTodoList(name);
        _context.OnListCreated(list);
    }

    [Given(@"(?:я )?добавил задачи ""(.*)""")]
    public async Task ПустьЯДобавилЗадачи(string tasksTitles)
    {
        foreach (string title in SplitCommaSeparatedList(tasksTitles))
        {
            await _driver.AddTodoItem(_context.OpenedListId, title);
        }
    }

    [When(@"(?:я )?удалил список ""(.*)""")]
    public async Task КогдаЯУдалилСписок(string name)
    {
        int listId = _context.GetListIdByName(name);
        await _driver.DeleteTodoList(listId);
        _context.OnListDeleted(name);
    }

    [When(@"(?:я )?переименовал задачу №(.+) на ""(.+)""")]
    public async Task КогдаЯПереименовалЗадачуНа(int position, string newTitle)
    {
        await _driver.EditTodoItem(_context.OpenedListId, position, new EditTodoItemParams(Title: newTitle));
    }

    [When(@"(?:я )?переместил задачу №(.+) на позицию №(.+)")]
    public async Task КогдаЯПереместилЗадачу(int position, int newPosition)
    {
        await _driver.EditTodoItem(_context.OpenedListId, position, new EditTodoItemParams(Position: newPosition));
    }

    [When("(?:я )?завершил задачу №(.+)")]
    public async Task КогдаЯЗавершилЗадачу(int position)
    {
        await _driver.EditTodoItem(_context.OpenedListId, position, new EditTodoItemParams(IsCompleted: true));
    }

    [When("(?:я )?удалил задачу №(.+)")]
    public async Task КогдаЯУдалилЗадачу(int position)
    {
        await _driver.DeleteTodoItem(_context.OpenedListId, position);
    }

    [When(@"я перешёл к списку ""(.*)""")]
    public void КогдаЯПерешёлКСписку(string name)
    {
        _context.OnListOpened(name);
    }

    [Then(@"(?:я )?вижу (\d+) задач(?:и|у|): ""(.*)""")]
    public async Task ТогдаЯВижуЗадачи(int taskCount, string taskTitles)
    {
        TodoListDetailedData list = await _driver.GetTodoList(_context.OpenedListId);
        Assert.Equal(taskCount, list.Items.Length);
        Assert.Equal(SplitCommaSeparatedList(taskTitles), GetItemTitles(list));
    }

    [Then(@"(?:я )?вижу завершённые задачи: ""(.*)""")]
    public async Task ТогдаЯВижуЗавершённыеЗадачи(string tasksTitlesCommaSeparated)
    {
        TodoListDetailedData list = await _driver.GetTodoList(_context.OpenedListId);
        string[] taskTitles = SplitCommaSeparatedList(tasksTitlesCommaSeparated);
        Assert.Equal(taskTitles, GetCompletedItemTitles(list));
    }

    [Then(@"вижу (\d+) спис(?:ок|ка|ки): ""(.*)""")]
    public async Task TогдаВижуСписок(int listsCount, string listNamesCommaSeparated)
    {
        string[] taskTitles = SplitCommaSeparatedList(listNamesCommaSeparated);
        TodoListData[] lists = await _driver.ListTodoLists("");
        Assert.Equal(listsCount, lists.Length);
        Assert.Equal(taskTitles, GetListNames(lists));
    }

    [Then(@"с фильтром ""(.*)"" вижу списки: ""(.*)""")]
    public async Task TогдаСФильтромВижуСписки(string searchQuery, string listNamesCommaSeparated)
    {
        string[] taskTitles = SplitCommaSeparatedList(listNamesCommaSeparated);
        TodoListData[] lists = await _driver.ListTodoLists(searchQuery);
        Assert.Equal(taskTitles, GetListNames(lists));
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

    private class Context
    {
        public int OpenedListId => _openedListId ?? throw new ArgumentException("No todo lists created");

        public int GetListIdByName(string listName)
        {
            if (!_listNameToIdMap.TryGetValue(listName, out int listId))
            {
                throw new ArgumentException($"No todo list with name {listName}");
            }

            return listId;
        }

        public void OnListCreated(TodoListDetailedData list)
        {
            _listNameToIdMap[list.Name] = list.Id;
            _openedListId = list.Id;
        }

        public void OnListOpened(string name)
        {
            _openedListId = GetListIdByName(name);
        }

        public void OnListDeleted(string name)
        {
            if (_listNameToIdMap.TryGetValue(name, out int listId))
            {
                if (_openedListId == listId)
                {
                    _openedListId = null;
                }

                _listNameToIdMap.Remove(name);
            }
        }

        private readonly Dictionary<string, int> _listNameToIdMap = new();
        private int? _openedListId;
    };
}