using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

var builder = WebApplication.CreateBuilder(
    new WebApplicationOptions{
        Args = args,
        ContentRootPath = Environment.GetEnvironmentVariable("APP_CONTENT_ROOT_PATH") ?? Directory.GetCurrentDirectory(),
        WebRootPath = "public"
    }
);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddControllers();
builder.Services.AddDbContext<TodoApiDbContext>(
    options => options.UseSqlServer(connectionString)
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

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.UseFileServer();

app.Run();

public partial class Program { }
