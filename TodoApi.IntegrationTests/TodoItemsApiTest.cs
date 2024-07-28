using Microsoft.AspNetCore.Mvc.Testing;

namespace TodoApi.IntegrationTests;

public class TodoItemsApiTest()
{

    [Test]
    public async Task Test1()
    {
        var client = factory.CreateClient();
        var response = await client.GetAsync("api/TodoItems");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        Assert.AreEqual("[]", content);
    }

    private readonly WebApplicationFactory<Program> factory = new();
}
