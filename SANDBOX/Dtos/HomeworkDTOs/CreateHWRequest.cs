namespace SANDBOX.Dtos.HomeworkDTOs
{
    public class CreateHWRequest
    {
        public string Name { get; set; } = "";
        public List<string> ProblemNames { get; set; } = [];

        public DateTime Deadline { get; set; }
    }
}
