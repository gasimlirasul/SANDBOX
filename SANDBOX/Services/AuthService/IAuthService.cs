using SANDBOX.Dtos;
using SANDBOX.Dtos.UserDTOs;

namespace SANDBOX.Services.AuthService
{
    public interface IAuthService
    {
        Task<UserResponse?> Register(UserDto request);
        Task<TokenResponse?> Login(UserDto request);
    }
}
