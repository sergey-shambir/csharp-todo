using System.Net.Http.Json;
using Newtonsoft.Json;
using TodoApi.Application.Data;

namespace TodoApi.IntegrationTests.Gateways;

public class TodoListTestApiGateway(HttpClient httpClient)
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<TodoListDetailedData> GetTodoList(int listId)
    {
        var response = await _httpClient.GetAsync($"api/todo-list/{listId}");
        await EnsureSuccessStatusCode(response);

        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<TodoListDetailedData>(content)
            ?? throw new ArgumentException($"Unexpected JSON response: {content}");
    }

    public async Task<TodoListDetailedData> CreateTodoList(string name)
    {
        var response = await _httpClient.PostAsJsonAsync("api/todo-list", new { name });
        await EnsureSuccessStatusCode(response);

        var itemUrl = response.Headers.Location;
        response = await _httpClient.GetAsync(itemUrl);
        await EnsureSuccessStatusCode(response);

        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<TodoListDetailedData>(content)
            ?? throw new ArgumentException($"Unexpected JSON response: {content}");
    }

    public async Task<int> AddTodoItem(int listId, string title)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/todo-list/{listId}", new { title });
        await EnsureSuccessStatusCode(response);

        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<int>(content);
    }

    public async Task EditTodoItem(int listId, int position, EditTodoItemParams itemParams)
    {
        var response = await _httpClient.PatchAsJsonAsync($"api/todo-list/{listId}/{position}", itemParams);
        await EnsureSuccessStatusCode(response);
    }

    public async Task DeleteTodoItem(int listId, int position)
    {
        var response = await _httpClient.DeleteAsync($"api/todo-list/{listId}/{position}");
        await EnsureSuccessStatusCode(response);
    }

    public async Task DeleteTodoList(int listId)
    {
        var response = await _httpClient.DeleteAsync($"api/todo-list/{listId}");
        await EnsureSuccessStatusCode(response);
    }

    private static async Task EnsureSuccessStatusCode(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            Assert.Fail(content);
        }
    }
}
