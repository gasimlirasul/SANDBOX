namespace SANDBOX.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public List<User> Users { get; set; } = null!;

        public List<Homework> HWs { get; set; } = null!;
    }
}
