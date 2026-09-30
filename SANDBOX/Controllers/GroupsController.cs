using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SANDBOX.Data;
using SANDBOX.Dtos.GroupDTOs;
using SANDBOX.Dtos.HomeworkDTOs;
using SANDBOX.Dtos.UserDTOs;
using SANDBOX.Models;
using SANDBOX.Services.GroupsService;
using System.Runtime.CompilerServices;

namespace SANDBOX.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GroupsController(IGroupsService groupsService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<GroupResponse>>> GetAllGroups()
        {
            return Ok(await groupsService.GetAllGroups());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GroupResponse>> GetGroup(int id)
        {
            var group = await groupsService.GetGroup(id);
            return Ok(group);
        }

        [HttpPost]
        [Authorize(Roles = "Teacher, Admin")]
        public async Task<ActionResult> CreateGroup(GroupRequest request)
        {
            var response = await groupsService.CreateGroup(request);
            return CreatedAtAction(nameof(GetGroup), new { id = response.Id }, response);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Teacher, Admin")]
        public async Task<ActionResult<GroupResponse>> UpdateGroup(int id, GroupRequest request)
        {
            var response = await groupsService.UpdateGroup(id, request);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Teacher, Admin")]
        public async Task<ActionResult> DeleteGroup(int id)
        {
            await groupsService.DeleteGroup(id);
            return NoContent();
        }

        [HttpGet("{id}/users")]
        [Authorize(Roles = "Teacher, Admin")]
        public async Task<ActionResult<List<UserResponse>>> GetUsers(int id)
        {
            var list = await groupsService.GetUsers(id);
            return Ok(list);
        }

        [HttpPost("{id}/users")] 
        [Authorize(Roles = "Teacher, Admin")]
        public async Task<ActionResult> AddUser(int id, string name)
        {
            await groupsService.AddUser(id, name);
            return Ok();
        }

        [HttpPost("{id}/hws")]
        [Authorize(Roles = "Teacher, Admin")]
        public async Task<ActionResult> AddHW(int id, CreateHWRequest request)
        {
            await groupsService.AddHW(id, request);
            return Ok();
        }
        [HttpGet("{id}/hws")]
        public async Task<ActionResult<List<HomeworkResponse>>> GetHomeworks(int id)
        {
            var list = await groupsService.GetHomeworks(id);
            return Ok(list);
        }
    }
}
