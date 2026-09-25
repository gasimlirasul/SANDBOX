namespace SANDBOX.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = "";
        public string PasswordHash { get; set; } = "";
        public List<Problem> SolvedProblems { get; set; } = [];
        public List<Group> Groups { get; set; } = [];

        public string Role { get; set; } = "Student";

        public string RefreshToken { get; set; } = "";

        public DateTime? RefreshTokenExpiryDate { get; set; }

    }
}
