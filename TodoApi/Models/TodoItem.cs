using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models;

public class TodoItem
{
    public int? Id { get; set; }
    public required string Title { get; set; }
    public required bool IsComplete { get; set; }
}
