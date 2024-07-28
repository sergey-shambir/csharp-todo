using System.ComponentModel.DataAnnotations;
using Microsoft.VisualStudio.TextTemplating;

namespace TodoApi.Models;

public class TodoItem(string title)
{
    public int? Id { get; set; }
    public string Title { get; set; } = title;
    public bool IsComplete { get; set; } = false;

    public override bool Equals(object? obj)
    {
        if (obj is TodoItem other)
        {
            return Id == other.Id && Title == other.Title && IsComplete == other.IsComplete;
        }
        return false;
    }

    public override string ToString()
    {
        return $"TodoItem(Id={Id}, Title={Title}, IsComplete={IsComplete})";
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
