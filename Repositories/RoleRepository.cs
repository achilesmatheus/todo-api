using Todo.Data;
using Todo.Models;
using Todo.Repositories.Contracts;

namespace Todo.Repositories;

public class RoleRepository : GenericRepository<RoleModel>, IRoleRepository
{
    private readonly AppDbContext _context;

    public RoleRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
}