namespace Todo.Repositories.Contracts;

public interface ITaskRepository<T> : IGenericRepository<T> where T : class
{
    Task<IEnumerable<T>> GetByPeriodAsync(DateTime date);
}
