using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SANDBOX.Data;
using SANDBOX.Dtos.ProblemDTOs;
using SANDBOX.Dtos.UserDTOs;
using System.Security.Claims;

namespace SANDBOX.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController(AppDbContext _context) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<UserResponse>>> GetAllUsers()
        {
            return await _context.Users.Select(u => new UserResponse
            {
                Username = u.Username
            })
            .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponse>> GetUser(int id)
        {
            var user = await _context.Users
                .Where(u => u.Id == id)
                .Select(u => new UserResponse
                {
                    Username = u.Username
                })
                .FirstOrDefaultAsync();

            if (user == null)
                return NotFound("There is no user with the given id"); 
            
            return Ok(user);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ProblemResponse>> UpdateUser(int id, UpdateUserRequest updatedUser)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound("User with the given id does not exist");
            user.Username = updatedUser.Username;
            await _context.SaveChangesAsync();
            UserResponse response = new UserResponse
            {
                Username = user.Username
            };
            return Ok(response);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound("User with the given id does not exist");
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("solved")]
        public async Task<ActionResult<List<ProblemResponse>>> AllSolvedProblems()
        {
            var id = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!
            );
            var user = await _context.Users
                .Include(u => u.SolvedProblems)
                .FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                return NotFound("User does not exist");
            var list = user.SolvedProblems
                .Select(p => new ProblemResponse
                {
                    Name = p.Name
                })
                .ToList();
            return list;
        }
    }
}
