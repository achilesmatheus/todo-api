namespace Todo.ViewModels;

public class ResultViewModel<T>
{
    public T? Data { get; private set; }
    public IList<string>? Errors { get; private set; } = new List<string>();

    public ResultViewModel(T data, IList<string> errors)
    {
        Data = data;
        Errors = errors;
    }

    public ResultViewModel(T data)
    {
        Data = data;
        Errors = null;
    }

    public ResultViewModel(IList<string> errors)
    {
        Errors = errors;
    }

    public ResultViewModel(string error)
    {
        Errors.Add(error);
    }
}
