using System.Net;

namespace Todo.Specs.Drivers;

public class ApiBadRequestException(string message, Dictionary<string, string[]> errors)
    : Exception(message)
{
    public Dictionary<string, string[]> Errors { get; } = errors;
}
