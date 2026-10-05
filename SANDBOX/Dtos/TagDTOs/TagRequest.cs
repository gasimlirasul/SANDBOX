using System.ComponentModel.DataAnnotations;

namespace SANDBOX.Dtos.TagDTOs
{
    public class TagRequest
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; } = null!;
    }
}
