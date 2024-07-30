
using TodoApi.IntegrationTests.Common;
using TodoApi.IntegrationTests.Gateways;

namespace TodoApi.IntegrationTests;

public class TodoListApiTest(IntegrationTestFixture<Program> fixture) : IClassFixture<IntegrationTestFixture<Program>>
{
    private readonly IntegrationTestFixture<Program> _fixture = fixture;
    private readonly TodoListTestApiGateway _gateway = new(fixture.HttpClient);

    [Fact]
    public async Task TestSingleListLifecycle()
    {
        var list = await _gateway.CreateTodoList("Home");
        Assert.Equal("Home", list.Name);
    }
}
