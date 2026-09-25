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
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SANDBOX.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(AppDbContext _context, IConfiguration configuration) : ControllerBase
    {


        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDto request)
        {
            if(await _context.Users.AnyAsync(u => u.Username == request.Username))
            {
                return BadRequest("There is another user with the given username");
            }
            var user = new User();
            var hashedPassword = new PasswordHasher<User>()
                .HashPassword(user, request.Password);

            user.Username = request.Username;
            user.PasswordHash = hashedPassword;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok(new UserResponse { Username = user.Username});
        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenResponse>> Login(UserDto request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
            if(user == null)
            {
                return BadRequest("Username or password is incorrect");
            }
            if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.Password) ==
                PasswordVerificationResult.Failed)
            {
                return BadRequest("Username or password is incorrect");
            }

            var response = new TokenResponse
            {
                AccessToken = CreateToken(user),
                RefreshToken = await GenerateAndSaveRefreshToken(user)
            };

            return Ok(response);
        }

        private string CreateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private async Task<string> GenerateAndSaveRefreshToken(User user)
        {
            var refreshToken = CreateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryDate = DateTime.UtcNow.AddDays(7);
            await _context.SaveChangesAsync();
            return refreshToken;
        }
        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration.GetValue<string>("AppSettings:Token")!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: configuration.GetValue<string>("AppSettings:Issuer"),
                audience: configuration.GetValue<string>("AppSettings:Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}
