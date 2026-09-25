namespace SANDBOX.Models
{
    public class Problem
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public List<User> UsersWhoSolved { get; set; } = [];

        public List<Tag> Tags { get; set; } = [];
        public List<Homework> HWs { get; set; } = [];
    }
}
