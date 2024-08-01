namespace Todo.Domain.Model;

public class TodoItem(string title, int listId, int position)
{
    public int Id { get; internal set; }
    public string Title { get; internal set; } = title;
    public bool IsComplete { get; internal set; } = false;
    public int ListId { get; internal set; } = listId;
    public int Position { get; internal set; } = position;

    public override bool Equals(object? obj)
    {
        if (obj is TodoItem other)
        {
            return Id == other.Id && Title == other.Title && IsComplete == other.IsComplete;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
