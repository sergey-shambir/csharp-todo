namespace TodoApi.Application.Exceptions;

public class EntityNotFoundException(string message) : Exception(message)
{
}
