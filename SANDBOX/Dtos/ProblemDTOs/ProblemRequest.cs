using System.ComponentModel.DataAnnotations;

namespace SANDBOX.Dtos.ProblemDTOs
{
    public class ProblemRequest
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; } = null!;
    }
}
