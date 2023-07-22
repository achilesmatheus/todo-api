namespace Todo.Models
{
    public class RoleModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public IList<UserModel> Users { get; set; }
    }
}