using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SANDBOX.Data;
using SANDBOX.Dtos.ProblemDTOs;
using SANDBOX.Dtos.UserDTOs;
using SANDBOX.Services.UsersService;
using System.Security.Claims;

namespace SANDBOX.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController(IUsersService userService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<UserResponse>>> GetAllUsers()
        {
            return Ok(await userService.GetAllUsers());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponse>> GetUser(int id)
        {
            var user = await userService.GetUser(id);
            if (user == null)
                return NotFound("User does not exist");
            return Ok(user);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserResponse>> UpdateUser(int id, UpdateUserRequest updatedUser)
        {
            var response = await userService.UpdateUser(id, updatedUser);
            if (response == null)
                return NotFound("User does not exist");
            return Ok(response);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            bool verdict = await userService.DeleteUser(id);
            if (verdict == false)
                return NotFound("User does not exist");
            return NoContent();
        }

        [HttpGet("{id}/solved")]
        public async Task<ActionResult<List<ProblemResponse>>> AllSolvedProblems(int id)
        {
            var list = await userService.AllSolvedProblems(id);
            if (list == null)
                return NotFound("User does not exist");
            return Ok(list);
        }
    }
}
