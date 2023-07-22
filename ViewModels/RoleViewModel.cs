using System.ComponentModel.DataAnnotations;

namespace Todo.ViewModels;

public class RoleViewModel
{
    [Required]
    public string Name { get; set; }
}
