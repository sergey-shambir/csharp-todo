using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TodoApi.Infrastructure;

namespace TodoApi.IntegrationTests.Common;

public class IntegrationTestFixture<TProgram> : IDisposable
where TProgram : class
{
    private const string SQL_CONNECTION = "Host=127.0.0.1;Database=todo;Username=todoapi;Password=em4xooNu";

    public IntegrationTestFixture()
    {
        WebApplicationFactory<Program> factory = new();
        factory = factory.WithWebHostBuilder((IWebHostBuilder b) =>
        {
            b.ConfigureTestServices(services =>
            {
                // Instantiate DB context once to wrap the SUT with transaction (see below).
                var options = new DbContextOptionsBuilder<TodoApiDbContext>()
                .UseNpgsql(SQL_CONNECTION)
                .Options;
                services.AddSingleton(options);
                services.AddSingleton<TodoApiDbContext>();

                // Add logging to see the reason of 500 (Internal Server Error)
                services.AddLogging(builder => builder.AddJsonConsole().AddFilter(level => level >= LogLevel.Trace));
            });
        });

        var dbContext = factory.Services.GetRequiredService<TodoApiDbContext>();
        _dbTransaction = dbContext.Database.BeginTransaction();
        _httpClient = factory.CreateClient();
    }

    public HttpClient HttpClient
    {
        get => _httpClient;
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

    private readonly HttpClient _httpClient;
    private IDbContextTransaction? _dbTransaction;
}
