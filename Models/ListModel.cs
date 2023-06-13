namespace Todo.Models;

public class ListModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<TaskModel> Tasks { get; set; }
}
