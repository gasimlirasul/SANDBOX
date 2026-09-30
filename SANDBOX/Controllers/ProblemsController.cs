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
        [HttpGet("all")]
        public async Task<ActionResult<List<ProblemResponse>>> GetAllProblems()
        {
            return Ok(await problemService.GetAllProblems());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProblemResponse>> GetProblem(int id)
        {
            var problem = await problemService.GetProblem(id);
            if (problem == null)
                return NotFound("Problem with the given id does not exist");
            return Ok(problem);
        }
        [HttpPost]
        [Authorize(Roles = "Teacher")]
        public async Task<ActionResult> CreateProblem(ProblemRequest request)
        {
            var response = await problemService.CreateProblem(request);
            return CreatedAtAction(nameof(GetProblem), new { id = response.Id }, response);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Teacher")]
        public async Task<ActionResult<ProblemResponse>> UpdateProblem(int id, ProblemRequest request)
        {
            var response = await problemService.UpdateProblem(id, request);
            if (response == null)
                return NotFound("Problem does not exist");
            return Ok(response);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Teacher")]
        public async Task<ActionResult> DeleteProblem(int id)
        {
            bool success = await problemService.DeleteProblem(id);
            if (success == false)
                return NotFound("Problem does not exist");
            return NoContent();
        }

        [HttpPost("{id}/submit/{userId}")]
        public async Task<ActionResult> AcceptedSubmission(int id, int userId)
        {
            bool success = await problemService.AcceptedSubmission(id, userId);
            if (success == false)
                return NotFound("User or problem does not exist");
            return Ok();
        }


        [HttpPost("tags")]
        public async Task<ActionResult> AddTag(string name, TagRequest request)
        {
            bool success = await problemService.AddTag(name, request);
            if (success == false)
                return NotFound("Problem or tag does not exist");
            return Ok();
        }

        
        [HttpGet("tags")]
        public async Task<ActionResult<List<TagResponse>>> GetProblemTags(string name)
        {
            var tags = await problemService.GetProblemTags(name);
            if (tags == null)
                return NotFound("Problem does not exist");
            return Ok(tags);
        }
    }
}
