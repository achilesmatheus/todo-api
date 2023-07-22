using System.Text.Json.Serialization;

namespace Todo.Models;

public class TaskModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public bool Done { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
