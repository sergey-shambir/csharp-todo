using Todo.Domain.Model;

namespace Todo.UnitTests;

public class TodoListTest
{
    [Fact]
    public void TestMoveItemsInList()
    {
        TodoList list = new("Today");
        list.AddItem("Cleanup the room");
        list.AddItem("Eat borscht");
        list.AddItem("Feed the hamster");
        list.AddItem("Wash the dishes");
        Assert.Equal(["Cleanup the room", "Eat borscht", "Feed the hamster", "Wash the dishes"], GetItemTitles(list));
        AssertItemPositionsConsistent(list);

        list.MoveItem(0, 2);
        Assert.Equal(["Eat borscht", "Feed the hamster", "Cleanup the room", "Wash the dishes"], GetItemTitles(list));
        AssertItemPositionsConsistent(list);

        list.MoveItem(2, 0);
        Assert.Equal(["Cleanup the room", "Eat borscht", "Feed the hamster", "Wash the dishes"], GetItemTitles(list));
        AssertItemPositionsConsistent(list);

        list.MoveItem(2, 3);
        Assert.Equal(["Cleanup the room", "Eat borscht", "Wash the dishes", "Feed the hamster"], GetItemTitles(list));
        AssertItemPositionsConsistent(list);

        list.MoveItem(2, 2);
        Assert.Equal(["Cleanup the room", "Eat borscht", "Wash the dishes", "Feed the hamster"], GetItemTitles(list));
        AssertItemPositionsConsistent(list);

        list.MoveItem(3, 0);
        Assert.Equal(["Feed the hamster", "Cleanup the room", "Eat borscht", "Wash the dishes"], GetItemTitles(list));
        AssertItemPositionsConsistent(list);
    }

    [Fact]
    public void TestToggleItemsInList()
    {
        TodoList list = new("Today");
        list.AddItem("Cleanup the room");
        list.AddItem("Eat borscht");
        list.AddItem("Feed the hamster");
        list.AddItem("Wash the dishes");
        Assert.Equal([], GetCompletedItemPositions(list));

        list.ToggleItem(2);
        Assert.Equal([2], GetCompletedItemPositions(list));

        list.ToggleItem(3);
        Assert.Equal([2, 3], GetCompletedItemPositions(list));

        list.ToggleItem(2);
        Assert.Equal([3], GetCompletedItemPositions(list));

        list.ToggleItem(0);
        Assert.Equal([0, 3], GetCompletedItemPositions(list));
    }

    [Fact]
    public void TestEditItemsInList()
    {
        TodoList list = new("Today");
        list.AddItem("Cleanup the room");
        list.AddItem("Eat borscht");
        list.AddItem("Feed the hamster");
        list.AddItem("Wash the dishes");
        Assert.Equal(["Cleanup the room", "Eat borscht", "Feed the hamster", "Wash the dishes"], GetItemTitles(list));

        list.ChangeItemTitle(2, "Feed the dog");
        list.ChangeItemTitle(1, "Eat borscht completely");
        Assert.Equal(["Cleanup the room", "Eat borscht completely", "Feed the dog", "Wash the dishes"], GetItemTitles(list));
    }

    private static string[] GetItemTitles(TodoList list)
    {
        return list.Items.Select(item => item.Title).ToArray();
    }

    private static int[] GetCompletedItemPositions(TodoList list)
    {
        return list.Items.Where(item => item.IsComplete).Select(item => item.Position).ToArray();
    }

    private static void AssertItemPositionsConsistent(TodoList list)
    {
        for (int i = 0, iSize = list.Items.Count; i < iSize; ++i)
        {
            Assert.Equal(i, list.Items[i].Position);
        }
    }
}
