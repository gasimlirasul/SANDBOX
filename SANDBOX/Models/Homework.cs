namespace SANDBOX.Models
{
    public class Homework
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public List<Problem> Problems { get; set; } = [];
        public int GroupId { get; set; }
        public Group? group { get; set; }

        public DateTime Deadline { get; set; }
    }
}
