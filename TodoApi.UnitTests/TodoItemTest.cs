using TodoApi.Models;

namespace TodoApi.UnitTests;

public class TodoItemTest
{
    [Fact]
    public void TestEquals()
    {
        TodoItem item1 = new("Eat borscht");
        TodoItem item2 = new("Eat borscht");
        TodoItem item3 = new("Eat borscht with cream soup");

        Assert.Equal(item1, item2);
        Assert.NotEqual(item1, item3);
        Assert.NotEqual(item2, item3);

        item2.IsComplete = true;
        Assert.NotEqual(item1, item2);
        Assert.NotEqual(item1, item3);
        Assert.NotEqual(item2, item3);

        item2.Title = "Eat borscht with cream soup";
        item3.IsComplete = true;
        Assert.NotEqual(item1, item2);
        Assert.NotEqual(item1, item3);
        Assert.Equal(item2, item3);
    }
}
