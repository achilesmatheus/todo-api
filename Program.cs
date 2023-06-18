using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using Todo.Data;
using Todo.Models;
using Todo.Repositories;
using Todo.Repositories.Contracts;
using Todo.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers();

builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddTransient<TokenService>();

builder.Services.AddTransient<ITaskRepository, TaskRepository>();
builder.Services.AddTransient<IListRepository, ListRepository>();
builder.Services.AddTransient<IFolderRepository, FolderRepository>();

var app = builder.Build();

app.MapControllers();

app.Run();