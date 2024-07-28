using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using TodoApi.Models;

namespace TodoApi.IntegrationTests.Common;

class IntegrationTestFixture<TStartup> : IDisposable
where TStartup : class
{
    public IntegrationTestFixture()
    {
        IWebHostBuilder builder = WebHost.CreateDefaultBuilder().UseStartup<TStartup>();
        TestServer server = new(builder);
        _httpClient = server.CreateClient();
        _services = server.Host.Services;
        _dbContext = _services.GetRequiredService<TodoApiDbContext>();
        _dbTransaction = _dbContext.Database.BeginTransaction();
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
    }

    private readonly IServiceProvider _services;
    private readonly HttpClient _httpClient;
    private readonly TodoApiDbContext _dbContext;
    private IDbContextTransaction? _dbTransaction;
}
