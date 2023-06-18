using Microsoft.EntityFrameworkCore;
using Todo.Data.FluentMapping;
using Todo.Models;

namespace Todo.Data;

public class AppDbContext : DbContext

{
    private const string CONNECTION_STRING = "Server=localhost;Database=Todo;User ID=sa;Password=1425;Trust Server Certificate=true";

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<TaskModel> Tasks { get; set; }
    public DbSet<ListModel> Lists { get; set; }
    public DbSet<FolderModel> Folders { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlServer(CONNECTION_STRING);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new TaskMap());
        modelBuilder.ApplyConfiguration(new ListMap());
        modelBuilder.ApplyConfiguration(new FolderMap());
    }
}
