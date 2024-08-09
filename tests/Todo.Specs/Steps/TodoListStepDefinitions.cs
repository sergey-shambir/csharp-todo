using Microsoft.Build.Framework;
using TechTalk.SpecFlow;
using Todo.Application.Data;
using Todo.Specs.Context;
using Todo.Specs.Fixtures;
using Todo.Specs.Drivers;
using Xunit;

namespace Todo.Specs.Steps;

[Binding]
public sealed class TodoListStepDefinitions(TodoListContext context, TestServerFixture fixture)
{
    private readonly TestServerFixture _fixture = fixture;
    private readonly TodoListTestDriver _driver = new(fixture.HttpClient);

    [Given(@"(?:я )?создал список ""(.*)""")]
    public async Task ПустьЯСоздалСписок(string name)
    {
        try
        {
            var list = await _driver.CreateTodoList(name);
            context.OnListCreated(list);
        }
        catch (ApiBadRequestException e)
        {
            context.LastException = e;
        }
    }

    [Given(@"(?:я )?добавил задачи ""(.*)""")]
    public async Task ПустьЯДобавилЗадачи(string tasksTitles)
    {
        foreach (string title in SplitCommaSeparatedList(tasksTitles))
        {
            await _driver.AddTodoItem(context.OpenedListId, title);
        }
    }

    [Given(@"(?:я )?добавил задачу ""(.*)""")]
    public async Task ПустьЯДобавилЗадачу(string tasksTitle)
    {
        try
        {
            await _driver.AddTodoItem(context.OpenedListId, tasksTitle);
        }
        catch (ApiBadRequestException e)
        {
            context.LastException = e;
        }
    }

    [When(@"(?:я )?удалил список ""(.*)""")]
    public async Task КогдаЯУдалилСписок(string name)
    {
        int listId = context.GetListIdByName(name);
        await _driver.DeleteTodoList(listId);
        context.OnListDeleted(name);
    }

    [When(@"(?:я )?переименовал задачу №(.+) на ""(.+)""")]
    public async Task КогдаЯПереименовалЗадачуНа(int position, string newTitle)
    {
        try
        {
            await _driver.EditTodoItem(context.OpenedListId, position, newTitle: newTitle);
        }
        catch (ApiBadRequestException e)
        {
            context.LastException = e;
        }
    }

    [When(@"(?:я )?переместил задачу №(.+) на позицию №(.+)")]
    public async Task КогдаЯПереместилЗадачу(int position, int newPosition)
    {
        await _driver.EditTodoItem(context.OpenedListId, position, newPosition: newPosition);
    }

    [When("(?:я )?завершил задачу №(.+)")]
    public async Task КогдаЯЗавершилЗадачу(int position)
    {
        await _driver.EditTodoItem(context.OpenedListId, position, newIsCompleted: true);
    }

    [When("(?:я )?удалил задачу №(.+)")]
    public async Task КогдаЯУдалилЗадачу(int position)
    {
        await _driver.DeleteTodoItem(context.OpenedListId, position);
    }

    [When(@"я перешёл к списку ""(.*)""")]
    public void КогдаЯПерешёлКСписку(string name)
    {
        context.OnListOpened(name);
    }

    [Then(@"(?:я )?вижу (\d+) задач(?:и|у|): ""(.*)""")]
    public async Task ТогдаЯВижуЗадачи(int taskCount, string taskTitles)
    {
        TodoListDetailedData list = await _driver.GetTodoList(context.OpenedListId);
        Assert.Equal(taskCount, list.Items.Length);
        Assert.Equal(SplitCommaSeparatedList(taskTitles), GetItemTitles(list));
        AssertItemPositionsAreConsistent(list);
    }

    [Then(@"(?:я )?вижу завершённые задачи: ""(.*)""")]
    public async Task ТогдаЯВижуЗавершённыеЗадачи(string tasksTitlesCommaSeparated)
    {
        TodoListDetailedData list = await _driver.GetTodoList(context.OpenedListId);
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

    [Then(@"вижу (?:ошибку|ошибки) валидации поля ""(.*)"": ""(.*)""")]
    public void ТогдаВижуОшибкуВалидацииПоля(string fieldName, string fieldErrors)
    {
        Assert.IsType<ApiBadRequestException>(context.LastException);
        if (context.LastException is ApiBadRequestException ex)
        {
            string actualFieldErrors = String.Join(", ", ex.Errors[fieldName]);
            Assert.Equal(fieldErrors, actualFieldErrors);
        }
    }

    private static void AssertItemPositionsAreConsistent(TodoListDetailedData list)
    {
        for (int i = 0, iSize = list.Items.Length; i < iSize; ++i)
        {
            Assert.Equal(i, list.Items[i].Position);
        }
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