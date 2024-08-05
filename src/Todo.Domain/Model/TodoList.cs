namespace Todo.Domain.Model;

public class TodoList(string name)
{
    public int Id { get; init; }
    public string Name { get; init; } = name;
    private readonly List<TodoItem> _items = [];

    public IReadOnlyList<TodoItem> Items => _items.AsReadOnly();

    public int AddItem(string title)
    {
        int position = _items.Count;
        _items.Add(new TodoItem(title, Id, position));
        return position;
    }

    public void ToggleItem(int position)
    {
        ValidateItemPosition(position);
        TodoItem item = _items[position];
        item.IsComplete = !item.IsComplete;
    }

    public void ChangeItemTitle(int position, string title)
    {
        ValidateItemPosition(position);
        _items[position].Title = title;
    }

    public void ChangeItemStatus(int position, bool isComplete)
    {
        ValidateItemPosition(position);
        _items[position].IsComplete = isComplete;
    }

    public void MoveItem(int fromPosition, int toPosition)
    {
        ValidateItemPosition(fromPosition);
        ValidateItemPosition(toPosition);

        int step = toPosition < fromPosition ? 1 : -1;
        for (int i = toPosition; i != fromPosition; i += step)
        {
            SwapItems(i, fromPosition);
        }
    }

    public void RemoveItem(int position)
    {
        ValidateItemPosition(position);
        _items.RemoveAt(position);
        for (int i = position, iSize = _items.Count; i < iSize; ++i)
        {
            _items[i].Position = i;
        }
    }

    public override bool Equals(object? obj)
    {
        if (obj is TodoList other)
        {
            return Id == other.Id && Name == other.Name;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    private void SwapItems(int position1, int position2)
    {
        (_items[position1], _items[position2]) = (_items[position2], _items[position1]);
        _items[position1].Position = position1;
        _items[position2].Position = position2;
    }

    private void ValidateItemPosition(int position)
    {
        if (position >= _items.Count)
        {
            throw new ArgumentOutOfRangeException($"Invalid item position {position} in list with id={Id}");
        }
    }
}
