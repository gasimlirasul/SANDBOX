using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SANDBOX.Data;
using SANDBOX.Dtos.ProblemDTOs;
using SANDBOX.Dtos.TagDTOs;
using SANDBOX.Models;
using System.Security.Claims;

namespace SANDBOX.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProblemController(AppDbContext _context) : ControllerBase
    {
        [HttpGet("all")]
        public async Task<ActionResult<List<ProblemResponse>>> GetAllProblems()
        {
            return await _context.Problems.Select(p => new ProblemResponse
            {
                Name = p.Name
            })
            .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProblemResponse>> GetProblem(int id)
        {
            var problem = await _context.Problems
                .Where(p => p.Id == id)
                .Select(p => new ProblemResponse
                {
                    Name = p.Name
                })
                .FirstOrDefaultAsync();

            if (problem == null)
                return NotFound("Problem with the given id does not exist");

            return Ok(problem);
        }
        [HttpPost]
        [Authorize(Roles = "Teacher")]
        public async Task<ActionResult> CreateProblem(ProblemRequest request)
        {
            Problem newProblem = new Problem
            {
                Name = request.Name,
            };

            _context.Problems.Add(newProblem);
            await _context.SaveChangesAsync();

            ProblemResponse response = new ProblemResponse
            {
                Name = request.Name,
            };
            return CreatedAtAction(nameof(GetProblem), new { id = newProblem.Id }, response);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Teacher")]
        public async Task<ActionResult<ProblemResponse>> UpdateProblem(int id, ProblemRequest request)
        {
            var problem = await _context.Problems.FindAsync(id);
            if (problem == null)
                return NotFound("Problem with the given id does not exist");
            problem.Name = request.Name;
            await _context.SaveChangesAsync();
            ProblemResponse response = new ProblemResponse
            {
                Name = problem.Name
            };
            return Ok(response);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Teacher")]
        public async Task<ActionResult> DeleteProblem(int id)
        {
            var problem = await _context.Problems.FindAsync(id);
            if (problem == null)
                return NotFound("Problem with the given id does not exist");
            _context.Problems.Remove(problem);
            await _context.SaveChangesAsync();
            return NoContent();
        }


        
        [HttpPost("{id}/submit")]
        public async Task<ActionResult> AcceptedSubmission(int id)
        {
            var userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!
            );

            var user = await _context.Users.FindAsync(userId);
            var problem = await _context.Problems.FindAsync(id);

            if (problem == null)
                return NotFound("Problem with the given id does not exist");
            if (user == null)
                return NotFound("User with the given id does not exist");

            user.SolvedProblems.Add(problem);
            await _context.SaveChangesAsync();
            return Ok("Accepted");
        }


        [HttpPost("add-tag")]
        public async Task<ActionResult> AddTag(string name, TagRequest request)
        {
            var tag = await _context.Tags.Where(t => t.Name == request.Name).FirstOrDefaultAsync();
            var problem = await _context.Problems.Where(p => p.Name == name).FirstOrDefaultAsync();
            if (tag == null)
                return NotFound("Tag does not exist");
            if (problem == null)
                return NotFound("Problem does not exist");

            problem.Tags.Add(tag);
            await _context.SaveChangesAsync();
            return Ok();
        }

        
        [HttpGet("tags")]
        public async Task<ActionResult<List<ProblemResponse>>> GetProblemTags(string name)
        {
            var problem = await _context.Problems
                .Include(p => p.Tags)
                .Where(p => p.Name == name)
                .FirstOrDefaultAsync();
            if (problem == null)
                return NotFound("Problem does not exist");
            var tags = problem.Tags.Select(t => new TagResponse
            {
                Name = t.Name
            })
            .ToList();
            return Ok(tags);
        }
    }
}
