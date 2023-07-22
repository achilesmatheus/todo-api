using System.ComponentModel.DataAnnotations;

namespace Todo.ViewModels;

public class TaskViewModel
{
    [Required]
    public string Title { get; set; }

    public string Description { get; set; }
}
