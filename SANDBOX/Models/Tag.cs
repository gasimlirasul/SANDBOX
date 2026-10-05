namespace SANDBOX.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public List<Problem> Problems { get; set; } = null!;
    }
}
