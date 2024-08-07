using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Todo.Infrastructure.Database;

namespace Todo.Specs.Fixtures;

public class TestServerFixture : IDisposable
{
    private IDbContextTransaction? _dbTransaction;

    public HttpClient HttpClient { get; }

    public TestServerFixture()
    {
        WebApplicationFactory<Program> factory = new();
        factory = factory.WithWebHostBuilder(b =>
        {
            b.UseEnvironment("Development");
            b.ConfigureTestServices(services =>
            {
                var descriptor = services.Single(d => d.ServiceType == typeof(DbContextOptions<TodoApiDbContext>));
                services.AddSingleton(descriptor.ServiceType, descriptor.ImplementationFactory!);
                services.AddSingleton<TodoApiDbContext>();

                services.AddLogging(builder => builder.AddConsole().AddFilter(level => level >= LogLevel.Warning));
            });
        });

        var dbContext = factory.Services.GetRequiredService<TodoApiDbContext>();
        _dbTransaction = dbContext.Database.BeginTransaction();

        HttpClient = factory.CreateClient();
    }

    public void Dispose()
    {
        if (_dbTransaction != null)
        {
            _dbTransaction.Rollback();
            _dbTransaction.Dispose();
            _dbTransaction = null;
        }

        GC.SuppressFinalize(this);
    }

    ~TestServerFixture()
    {
        Dispose();
    }
}