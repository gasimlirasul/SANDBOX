using System.ComponentModel.DataAnnotations;

namespace SANDBOX.Dtos.UserDTOs
{
    public class UpdateUserRequest
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Username { get; set; } = null!;
    }
}
