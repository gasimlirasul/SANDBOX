namespace SANDBOX.Models
{
    public class Problem
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public List<User> UsersWhoSolved { get; set; } = null!;

        public List<Tag> Tags { get; set; } = null!;
        public List<Homework> HWs { get; set; } = null!;
    }
}
