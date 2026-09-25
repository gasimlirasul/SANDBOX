using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SANDBOX.Data;
using SANDBOX.Dtos.UserDTOs;

namespace SANDBOX.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LeaderboardController(AppDbContext _context) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<UserRankingResponse>>> GetLeaderboard()
        {
            var list = await _context.Users
                .OrderByDescending(u => u.SolvedProblems.Count())
                .Select(u => new UserRankingResponse
                {
                    Username = u.Username,
                    NumberOfSolvedProblems = u.SolvedProblems.Count()
                })
                .ToListAsync();
            return list;
        }
    }
}
