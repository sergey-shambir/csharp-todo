using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.ContentModel;
using NuGet.Protocol;
using TodoApi.IntegrationTests.Common;
using TodoApi.Models;

namespace TodoApi.IntegrationTests;

[TestFixture]
public class TodoItemsApiTest
{
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _fixture = new();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _fixture.Dispose();
    }

    [Test]
    public async Task Test1()
    {
        Assert.That(await ListTodoItems(), Is.EqualTo(Array.Empty<TodoItem>()));

        var item1 = await CreateTodoItem("Clean the room");
        var item2 = await CreateTodoItem("Make borscht");
        var item3 = await CreateTodoItem("Eat borscht");
        Assert.That(await ListTodoItems(), Is.EqualTo(new TodoItem[] { item1, item2, item3 }));

        await DeleteTodoItem(item1.Id);
        Assert.That(await ListTodoItems(), Is.EqualTo(new TodoItem[] { item2, item3 }));

        item3.Title = "Eat borscht with sour cream";
        await UpdateTodoItem(item3);
        Assert.That(await GetTodoItem(item3.Id), Is.EqualTo(item3));
        Assert.That(await ListTodoItems(), Is.EqualTo(new TodoItem[] { item2, item3 }));
    }

    private async Task<TodoItem[]> ListTodoItems()
    {
        var response = await HttpClient.GetAsync("api/TodoItems");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<TodoItem[]>(content) ?? throw new ArgumentException($"Unexpected JSON response: {content}");
    }

    private async Task<TodoItem> CreateTodoItem(string title)
    {
        TodoItem item = new(title);
        var response = await HttpClient.PostAsJsonAsync("api/TodoItems", item);
        response.EnsureSuccessStatusCode();

        var itemUrl = response.Headers.Location;
        response = await HttpClient.GetAsync(itemUrl);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<TodoItem>(content) ?? throw new ArgumentException($"Unexpected JSON response: {content}");
    }

    private async Task UpdateTodoItem(TodoItem item)
    {
        var response = await HttpClient.PutAsJsonAsync($"api/TodoItems/{item.Id}", item);
        response.EnsureSuccessStatusCode();
    }

    private async Task<TodoItem> GetTodoItem(int? id)
    {
        var response = await HttpClient.GetAsync($"api/TodoItems/{id}");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<TodoItem>(content) ?? throw new ArgumentException($"Unexpected JSON response: {content}");
    }

    private async Task DeleteTodoItem(int? id)
    {
        var response = await HttpClient.DeleteAsync($"api/TodoItems/{id}");
        response.EnsureSuccessStatusCode();
    }

    private HttpClient HttpClient
    {
        get => _fixture.HttpClient;
    }

    private IntegrationTestFixture<Program> _fixture;
}
