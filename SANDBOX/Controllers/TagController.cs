using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SANDBOX.Data;
using SANDBOX.Dtos;
using SANDBOX.Dtos.TagDTOs;
using SANDBOX.Models;

namespace SANDBOX.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TagController(AppDbContext _context) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<TagResponse>>> GetAllTags()
        {
            return await _context.Tags.Select(t => new TagResponse
            {
                Name = t.Name
            })
            .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TagResponse>> GetTag(int id)
        {
            var tag = await _context.Tags
                .Where(t => t.Id == id)
                .Select(u => new TagResponse
                {
                    Name = u.Name
                })
                .FirstOrDefaultAsync();

            if (tag == null)
                return NotFound("There is no user with the given id");

            return Ok(tag);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Teacher, Admin")]
        public async Task<ActionResult> UpdateTag(int id, TagRequest request)
        {
            var tag = await _context.Tags.FindAsync(id);
            if (tag == null)
                return NotFound("Tag with the given id does not exist");
            tag.Name = request.Name;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost]
        [Authorize(Roles = "Teacher, Admin")]
        public async Task<ActionResult> CreateTag(TagRequest request)
        {
            var newTag = new Tag
            {
                Name = request.Name
            };
            _context.Add(newTag);
            await _context.SaveChangesAsync();
            var response = new TagResponse
            {
                Name = request.Name
            };
            return CreatedAtAction(nameof(GetTag), new {id = newTag.Id}, response);
        }

        [HttpDelete]
        [Authorize(Roles = "Teacher, Admin")]

        public async Task<ActionResult> DeleteTag(TagRequest deletedTag)
        {
            var tag = await _context.Tags.Where(t => t.Name == deletedTag.Name).FirstOrDefaultAsync();
            if (tag == null)
                return NotFound("Tag does not exist");
            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
