namespace Todo.ViewModels;

public class ResultViewModel<T>
{
    public T Data { get; private set; }
    public List<string> Errors { get; private set; }
}
