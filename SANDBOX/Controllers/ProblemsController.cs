using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SANDBOX.Data;
using SANDBOX.Dtos.ProblemDTOs;
using SANDBOX.Dtos.TagDTOs;
using SANDBOX.Models;
using SANDBOX.Services.ProblemsService;
using System.Security.Claims;

namespace SANDBOX.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProblemsController(IProblemsService problemService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<ProblemResponse>>> GetAllProblems()
        {
            return Ok(await problemService.GetAllProblems());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProblemResponse>> GetProblem(int id)
        {
            var problem = await problemService.GetProblem(id);
            return Ok(problem);
        }
        [HttpPost]
        [Authorize(Roles = "Teacher, Admin")]
        public async Task<ActionResult> CreateProblem(ProblemRequest request)
        {
            var response = await problemService.CreateProblem(request);
            return CreatedAtAction(nameof(GetProblem), new { id = response.Id }, response);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Teacher, Admin")]
        public async Task<ActionResult<ProblemResponse>> UpdateProblem(int id, ProblemRequest request)
        {
            var response = await problemService.UpdateProblem(id, request);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Teacher, Admin")]
        public async Task<ActionResult> DeleteProblem(int id)
        {
            await problemService.DeleteProblem(id);
            return NoContent();
        }

        [HttpPost("{id}/submission/{userId}")]
        public async Task<ActionResult> AcceptedSubmission(int id, int userId)
        {
            await problemService.AcceptedSubmission(id, userId);
            return Ok();
        }

        [HttpPost("tags")]
        public async Task<ActionResult> AddTag(string name, TagRequest request)
        {
            await problemService.AddTag(name, request);
            return Ok();
        }

        [HttpGet("tags")]
        public async Task<ActionResult<List<TagResponse>>> GetProblemTags(string name)
        {
            var tags = await problemService.GetProblemTags(name);
            return Ok(tags);
        }
    }
}
