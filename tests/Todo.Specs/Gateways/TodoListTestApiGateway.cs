using System.Net.Http.Json;
using System.Web;
using Newtonsoft.Json;
using Todo.Application.Data;
using Xunit;

namespace Todo.Specs.Gateways;

public class TodoListTestApiGateway(HttpClient httpClient)
{
    public async Task<TodoListData[]> ListTodoLists(string? searchQuery = null)
    {
        string uri = "api/todo-list";
        if (searchQuery != null)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["search"] = searchQuery;
            uri = $"{uri}?{query}";
        }

        HttpResponseMessage response = await httpClient.GetAsync(uri);
        await EnsureSuccessStatusCode(response);

        string content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<TodoListData[]>(content)
            ?? throw new ArgumentException($"Unexpected JSON response: {content}");
    }

    public async Task<TodoListDetailedData> GetTodoList(int listId)
    {
        HttpResponseMessage response = await httpClient.GetAsync($"api/todo-list/{listId}");
        await EnsureSuccessStatusCode(response);

        string content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<TodoListDetailedData>(content)
            ?? throw new ArgumentException($"Unexpected JSON response: {content}");
    }

    public async Task<TodoListDetailedData> CreateTodoList(string name)
    {
        HttpResponseMessage response = await httpClient.PostAsJsonAsync("api/todo-list", new { name });
        await EnsureSuccessStatusCode(response);

        Uri? itemUrl = response.Headers.Location;
        response = await httpClient.GetAsync(itemUrl);
        await EnsureSuccessStatusCode(response);

        string content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<TodoListDetailedData>(content)
            ?? throw new ArgumentException($"Unexpected JSON response: {content}");
    }

    public async Task AddTodoItem(int listId, string title)
    {
        HttpResponseMessage response = await httpClient.PostAsJsonAsync($"api/todo-list/{listId}", new { title });
        await EnsureSuccessStatusCode(response);
    }

    public async Task EditTodoItem(int listId, int position, EditTodoItemParams itemParams)
    {
        HttpResponseMessage response = await httpClient.PatchAsJsonAsync($"api/todo-list/{listId}/{position}", itemParams);
        await EnsureSuccessStatusCode(response);
    }

    public async Task DeleteTodoItem(int listId, int position)
    {
        HttpResponseMessage response = await httpClient.DeleteAsync($"api/todo-list/{listId}/{position}");
        await EnsureSuccessStatusCode(response);
    }

    public async Task DeleteTodoList(int listId)
    {
        HttpResponseMessage response = await httpClient.DeleteAsync($"api/todo-list/{listId}");
        await EnsureSuccessStatusCode(response);
    }

    private static async Task EnsureSuccessStatusCode(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            Assert.Fail($"HTTP status code {response.StatusCode}: {content}");
        }
    }
}
