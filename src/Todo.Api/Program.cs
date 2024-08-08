using Microsoft.EntityFrameworkCore;
using Todo.Infrastructure.Database;

var builder = WebApplication.CreateBuilder(
    new WebApplicationOptions
    {
        Args = args,
        WebRootPath = "public"
    }
);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddControllers();
builder.Services.AddDbContext<TodoApiDbContext>(
    options => options.UseNpgsql(connectionString)
);
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

public partial class Program
{
}