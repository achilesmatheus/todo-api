using Microsoft.EntityFrameworkCore;
using Todo.Data;
using Todo.Models;
using Todo.Repositories.Contracts;

namespace Todo.Repositories;

public class TaskRepository : GenericRepository<TaskModel>, ITaskRepository
{
    private readonly AppDbContext _context;
    public TaskRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TaskModel>> GetAllWithFilterAsync(int skip, int take)
    {
        return await _context.Tasks
            .AsNoTracking()
            .Skip(skip * take)
            .Take(take)
            .OrderBy(x => x.Id)
            .ToListAsync();
    }

    public async Task<IEnumerable<TaskModel>> GetByPeriodAsync(DateTime date)
    {
        return await _context.Set<TaskModel>()
            .Where(x => x.CreatedAt.Date == date.Date)
            .ToListAsync();
    }
}
