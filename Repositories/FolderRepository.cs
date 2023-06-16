using Todo.Data;
using Todo.Models;

namespace Todo.Repositories;

public class FolderRepository : GenericRepository<TaskModel>
{
    public FolderRepository(AppDbContext context) : base(context)
    {
    }
}
