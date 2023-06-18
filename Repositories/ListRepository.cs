using Microsoft.EntityFrameworkCore;
using Todo.Data;
using Todo.Models;

namespace Todo.Repositories;

public class ListRepository : GenericRepository<ListModel>
{
    private readonly AppDbContext _context;

    public ListRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public override async Task<IEnumerable<ListModel>> GetAllAsync()
    {
        return await _context.Lists
            .AsNoTracking()
            .Include(l => l.Tasks)
            .ToListAsync();
    }

    public override async Task<ListModel> GetByIdAsync(int id)
    {
        return await _context.Lists
            .Include(l => l.Tasks)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}
