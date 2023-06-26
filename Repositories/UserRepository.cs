using Todo.Data;
using Todo.Models;
using Todo.Repositories.Contracts;

namespace Todo.Repositories;

public class UserRepository : GenericRepository<UserModel>, IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
}