using Microsoft.EntityFrameworkCore;
using Todo.Models;

namespace Todo.Data;

public class AppDbContext : DbContext
{
    public DbSet<TaskModel> Tasks { get; set; }
    public DbSet<ListModel> Lists { get; set; }
    public DbSet<FolderModel> Folders { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite("DataSource=app.db;Cache=Shared");
    }
}
