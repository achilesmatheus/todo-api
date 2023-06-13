namespace Todo.Models;

public class FolderModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<ListModel> Lists { get; set; }
}
