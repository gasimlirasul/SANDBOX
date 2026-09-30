using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Microsoft.IdentityModel.Tokens;
using SANDBOX.Data;
using SANDBOX.Dtos;
using SANDBOX.Dtos.UserDTOs;
using SANDBOX.Models;
using SANDBOX.Services.AuthService;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SANDBOX.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<ActionResult<UserResponse>> Register(UserDto request)
        {
            var response = await authService.Register(request);
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenResponse>> Login(UserDto request)
        {
            var response = await authService.Login(request);
            return Ok(response);
        }
    }
}
