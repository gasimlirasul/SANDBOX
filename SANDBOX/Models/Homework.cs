namespace SANDBOX.Models
{
    public class Homework
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public List<Problem> Problems { get; set; } = null!;
        public int GroupId { get; set; }
        public Group? group { get; set; }

        public DateTime Deadline { get; set; }
    }
}
