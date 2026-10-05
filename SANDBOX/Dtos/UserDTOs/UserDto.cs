using System.ComponentModel.DataAnnotations;

namespace SANDBOX.Dtos.UserDTOs
{
    public class UserDto
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Username { get; set; } = null!;
        
        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Password { get; set; } = null!;
    }
}
