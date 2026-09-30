using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SANDBOX.Data;
using SANDBOX.Dtos;
using SANDBOX.Dtos.TagDTOs;
using SANDBOX.Models;
using SANDBOX.Services.TagsService;

namespace SANDBOX.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TagsController(ITagsService tagsService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<TagResponse>>> GetAllTags()
        {
            return Ok(await tagsService.GetAllTags());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TagResponse>> GetTag(int id)
        {
            var tag = await tagsService.GetTag(id);
            if (tag == null)
                return NotFound("There is no user with the given id");
            return Ok(tag);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Teacher, Admin")]
        public async Task<ActionResult> UpdateTag(int id, TagRequest request)
        {
            var response = await tagsService.UpdateTag(id, request);
            if (response == null)
                return NotFound("Tag with the given id does not exist");
            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = "Teacher, Admin")]
        public async Task<ActionResult> CreateTag(TagRequest request)
        {
            var response = await tagsService.CreateTag(request);
            return CreatedAtAction(nameof(GetTag), new {id = response.Id}, response);
        }

        [HttpDelete]
        [Authorize(Roles = "Teacher, Admin")]
        public async Task<ActionResult> DeleteTag(TagRequest deletedTag)
        {
            bool success = await tagsService.DeleteTag(deletedTag);
            if (success == false)
                return NotFound("Tag does not exist");
            return NoContent();
        }
    }
}
