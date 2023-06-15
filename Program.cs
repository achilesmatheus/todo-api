using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using Todo.Data;
using Todo.Repositories;
using Todo.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddTransient<TokenService>();
builder.Services.AddTransient<TaskRepository>();

var app = builder.Build();

app.MapControllers();

app.Run();