using Todo.Data;
using Todo.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddTransient<TokenService>();

var app = builder.Build();

app.MapControllers();

app.Run();