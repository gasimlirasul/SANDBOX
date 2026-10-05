using System.ComponentModel.DataAnnotations;

namespace SANDBOX.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public List<Problem> SolvedProblems { get; set; } = null!;
        public List<Group> Groups { get; set; } = null!;

        public string Role { get; set; } = "Student";

        public string RefreshToken { get; set; } = null!;

        public DateTime? RefreshTokenExpiryDate { get; set; }

    }
}
