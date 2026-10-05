using System.ComponentModel.DataAnnotations;

namespace SANDBOX.Dtos.GroupDTOs
{
    public class GroupRequest
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; } = null!;
    }
}
