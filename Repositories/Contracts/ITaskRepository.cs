using Todo.Models;

namespace Todo.Repositories.Contracts;

public interface ITaskRepository : IGenericRepository<TaskModel>
{
    Task<IEnumerable<TaskModel>> GetByPeriodAsync(DateTime date);
}
