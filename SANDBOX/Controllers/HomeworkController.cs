using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SANDBOX.Data;
using SANDBOX.Dtos.HomeworkDTOs;

namespace SANDBOX.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class HomeworkController(AppDbContext _context) : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<HomeworkResponse>> GetHW(int id)
        {
            var hw = await _context.Homeworks.FindAsync(id);
            if (hw == null)
                return NotFound("Homework does not exist");

            var response = new HomeworkResponse
            {
                Name = hw.Name,
                Deadline = hw.Deadline
            };
            return Ok(response);
        }
    }
}
