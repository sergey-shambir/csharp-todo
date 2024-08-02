using Microsoft.EntityFrameworkCore;
using Todo.Infrastructure.Database;
using Todo.Domain.Repository;
using Todo.Infrastructure.Database.Repository;
using Todo.Application.Command.Handler;
using Todo.Infrastructure.Query;
using Todo.Infrastructure.MediatR;
using Todo.Application.Persistence;

var builder = WebApplication.CreateBuilder(
    new WebApplicationOptions
    {
        Args = args,
        ContentRootPath = Environment.GetEnvironmentVariable("APP_CONTENT_ROOT_PATH") ?? Directory.GetCurrentDirectory(),
        WebRootPath = "public"
    }
);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<TodoApiDbContext>(
    options => options.UseNpgsql(connectionString)
);

builder.Services.AddScoped<ITodoListRepository, TodoListRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblyContaining(typeof(CreateTodoListCommandHandler));
    config.RegisterServicesFromAssemblyContaining(typeof(SearchTodoListsQueryHandler));
    config.AddOpenBehavior(typeof(TodoPipelineBehavior<,>));
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<TodoApiDbContext>();
    await dbContext.Database.MigrateAsync();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.UseFileServer();

app.Run();

public partial class Program { }
