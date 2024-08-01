using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Todo.Infrastructure.Database;

namespace Todo.Specs.Drivers;

public class TestServerDriver : IDisposable
{
    private const string SQL_CONNECTION = "Host=127.0.0.1;Database=todo;Username=todoapi;Password=em4xooNu";

    private IDbContextTransaction? _dbTransaction;

    public HttpClient HttpClient { get; }

    TestServerDriver()
    {
        WebApplicationFactory<Program> factory = new();
        factory = factory.WithWebHostBuilder(b =>
        {
            b.ConfigureTestServices(services =>
            {
                // Instantiate DB context once to wrap the SUT with transaction (see below).
                var options = new DbContextOptionsBuilder<TodoApiDbContext>()
                .UseNpgsql(SQL_CONNECTION)
                .Options;
                services.AddSingleton(options);
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

    ~TestServerDriver()
    {
        Dispose();
    }
}
