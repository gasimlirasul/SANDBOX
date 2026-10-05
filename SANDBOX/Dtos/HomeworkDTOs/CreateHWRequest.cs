using System.ComponentModel.DataAnnotations;

namespace SANDBOX.Dtos.HomeworkDTOs
{
    public class CreateHWRequest
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; } = null!;
        public List<string> ProblemNames { get; set; } = null!;

        public DateTime Deadline { get; set; }
    }
}
