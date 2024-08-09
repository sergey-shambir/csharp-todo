using Todo.Application.Data;

namespace Todo.Specs.Context;

public class TodoListContext
{
    private readonly Dictionary<string, int> _listNameToIdMap = new();
    private int? _openedListId;

    public int OpenedListId => _openedListId ?? throw new ArgumentException("No todo lists created");
    public Exception? LastException { get; set; }

    public int GetListIdByName(string listName)
    {
        if (!_listNameToIdMap.TryGetValue(listName, out int listId))
        {
            throw new ArgumentException($"No todo list with name {listName}");
        }

        return listId;
    }

    public void OnListCreated(TodoListDetailedData list)
    {
        _listNameToIdMap[list.Name] = list.Id;
        _openedListId = list.Id;
    }

    public void OnListOpened(string name)
    {
        _openedListId = GetListIdByName(name);
    }

    public void OnListDeleted(string name)
    {
        if (_listNameToIdMap.TryGetValue(name, out int listId))
        {
            if (_openedListId == listId)
            {
                _openedListId = null;
            }

            _listNameToIdMap.Remove(name);
        }
    }
}
