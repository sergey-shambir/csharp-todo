using System.Net.Http.Json;
using Newtonsoft.Json;
using TodoApi.IntegrationTests.Common;
using TodoApi.Models;

namespace TodoApi.IntegrationTests;

public class TodoItemsApiTest(IntegrationTestFixture<Program> fixture) : IClassFixture<IntegrationTestFixture<Program>>
{
    [Fact]
    public async Task Test1()
    {
        Assert.Equal([], await ListTodoItems());

        var item1 = await CreateTodoItem("Clean the room");
        var item2 = await CreateTodoItem("Make borscht");
        var item3 = await CreateTodoItem("Eat borscht");
        Assert.Equal([item1, item2, item3], await ListTodoItems());

        await DeleteTodoItem(item1.Id);
        Assert.Equal([item2, item3], await ListTodoItems());

        item3.Title = "Eat borscht with sour cream";
        await UpdateTodoItem(item3);
        Assert.Equal(item3, await GetTodoItem(item3.Id));
        Assert.Equal([item2, item3], await ListTodoItems());
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

    private IntegrationTestFixture<Program> _fixture = fixture;
}
