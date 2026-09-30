using SANDBOX.Dtos.ProblemDTOs;
using SANDBOX.Dtos.UserDTOs;

namespace SANDBOX.Services.UsersService
{
    public interface IUsersService
    {
        Task<List<UserResponse>> GetAllUsers();
        Task<UserResponse?> GetUser(int id);
        Task<UserResponse?> UpdateUser(int id, UpdateUserRequest updatedUser);
        Task<bool> DeleteUser(int id);
        Task<List<ProblemResponse>?> AllSolvedProblems(int id);
    }
}
