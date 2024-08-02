using MediatR;
using Todo.Infrastructure.Database;
using Microsoft.Extensions.DependencyInjection;

namespace Todo.Infrastructure.MediatR;

public class TodoPipelineBehavior<TRequest, TResponse>(IServiceProvider serviceProvider) : IPipelineBehavior<TRequest, TResponse>
where TRequest : class
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var context = serviceProvider.GetRequiredService<TodoApiDbContext>();
        var response = await next();
        await context.SaveChangesAsync(cancellationToken);

        return response;
    }
}
