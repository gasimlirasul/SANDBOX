using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SANDBOX.Data;
using SANDBOX.Dtos.GroupDTOs;
using SANDBOX.Dtos.HomeworkDTOs;
using SANDBOX.Dtos.UserDTOs;
using SANDBOX.Models;
using System.Runtime.CompilerServices;

namespace SANDBOX.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GroupController(AppDbContext _context) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<GroupResponse>>> GetAllGroups()
        {
            return await _context.Groups.Select(g => new GroupResponse
            {
                Name = g.Name
            })
            .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GroupResponse>> GetGroup(int id)
        {
            var group = await _context.Groups
                .Where(g => g.Id == id)
                .Select(g => new GroupResponse
                {
                    Name = g.Name
                })
                .FirstOrDefaultAsync();
            if (group == null)
                return NotFound("Group does not exist");
            return Ok(group);
        }

        [HttpPost]
        [Authorize(Roles = "Teacher, Admin")]
        public async Task<ActionResult> CreateGroup(GroupRequest request)
        {
            var group = new Group
            {
                Name = request.Name
            };
            _context.Add(group);
            await _context.SaveChangesAsync();

            var response = new GroupResponse
            {
                Name = group.Name
            };
            return CreatedAtAction(nameof(GetGroup), new { id = group.Id }, response);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Teacher, Admin")]
        public async Task<ActionResult<GroupResponse>> UpdateGroup(int id, GroupRequest request)
        {
            var group = await _context.Groups.FindAsync(id);
            if (group == null)
                return NotFound("Group does not exist");
            group.Name = request.Name;
            await _context.SaveChangesAsync();
            var response = new GroupResponse
            {
                Name = group.Name
            };
            return Ok(response);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Teacher, Admin")]
        public async Task<ActionResult> DeleteGroup(int id)
        {
            var group = await _context.Groups.FindAsync(id);
            if (group == null)
                return NotFound("Group does not exist");
            _context.Remove(group);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("{id}/users")]
        [Authorize(Roles = "Teacher, Admin")]
        public async Task<ActionResult<List<UserResponse>>> GetUsers(int id)
        {
            var group = await _context.Groups
                .Include(g => g.Users)
                .FirstOrDefaultAsync(g => g.Id == id);
            if (group == null)
                return NotFound("Group does not exist");

            var list = group.Users
                .Select(u => new UserResponse
                {
                    Username = u.Username
                })
                .ToList();
            return Ok(list);
        }

        [HttpPost("{id}/add-user")]
        [Authorize(Roles = "Teacher, Admin")]
        public async Task<ActionResult> AddUser(int id, string name)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == name);
            var group = await _context.Groups.FindAsync(id);

            if (user == null)
                return NotFound("User does not exist");

            if (group == null)
                return NotFound("Group does not exist");

            user.Groups.Add(group);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("{id}/add-hw")]
        [Authorize(Roles = "Teacher, Admin")]
        public async Task<ActionResult> AddHW(int id, CreateHWRequest request)
        {
            var group = await _context.Groups.FindAsync(id);
            if (group == null)
                return NotFound("Group does not exist");
            var hw = new Homework
            {
                Name = request.Name,
                Deadline = request.Deadline
            };
            List<Problem> problems = [];
            foreach(string name in request.ProblemNames)
            {
                var problem = await _context.Problems
                                            .Where(p => p.Name == name)
                                            .FirstOrDefaultAsync();
                if (problem == null)
                {
                    return BadRequest("Invalid problem name sequence");
                }
                problems.Add(problem);
            }
            _context.Homeworks.Add(hw);
            foreach (Problem p in problems)
            {
                hw.Problems.Add(p);
            }
            group.HWs.Add(hw);

            await _context.SaveChangesAsync();
            return Ok("Homework added");
        }
        [HttpGet("{id}/all-hws")]
        public async Task<ActionResult<List<HomeworkResponse>>> GetHomeworks(int id)
        {
            var group = await _context.Groups
                .Include(g => g.HWs)
                .Where(g => g.Id == id)
                .FirstOrDefaultAsync();
            if (group == null)
                return NotFound("Group does not exist");

            var list = group.HWs
                .Select(h => new HomeworkResponse
                {
                    Name = h.Name,
                    Deadline = h.Deadline
                })
                .ToList();
            return Ok(list);
        }
    }
}
