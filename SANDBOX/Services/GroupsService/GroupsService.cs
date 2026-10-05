using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SANDBOX.Data;
using SANDBOX.Dtos.GroupDTOs;
using SANDBOX.Dtos.HomeworkDTOs;
using SANDBOX.Dtos.UserDTOs;
using SANDBOX.Exceptions;
using SANDBOX.Models;

namespace SANDBOX.Services.GroupsService
{
    public class GroupsService(AppDbContext _context, IMapper _mapper, ILogger<GroupsService> _logger) : IGroupsService
    {
        public async Task<List<GroupResponse>> GetAllGroups()
        {
            return await _context.Groups
               .ProjectTo<GroupResponse>(_mapper.ConfigurationProvider)
               .ToListAsync();
        }
        public async Task<GroupResponse> GetGroup(int id)
        {
            var group = await _context.Groups
                .Where(g => g.Id == id)
                .ProjectTo<GroupResponse>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            if (group == null)
                throw new NotFoundException("Group does not exist");

            return group;
        }
        public async Task<GroupResponse> CreateGroup(GroupRequest request)
        {
            var newGroup = _mapper.Map<Group>(request);
            _context.Add(newGroup);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Group created successfully");

            var response = _mapper.Map<GroupResponse>(newGroup);
            return response;
        }
        public async Task<GroupResponse> UpdateGroup(int id, GroupRequest request)
        {
            var group = await _context.Groups.FindAsync(id);

            if (group == null)
                throw new NotFoundException("Group does not exist");

            _mapper.Map(request, group);
            _logger.LogInformation("Group {groupName} updated successfully", group.Name);

            await _context.SaveChangesAsync();
            var response = _mapper.Map<GroupResponse>(group);
            return response;
        }
        public async Task DeleteGroup(int id)
        {
            var group = await _context.Groups.FindAsync(id);

            if (group == null)
                throw new NotFoundException("Group does not exist");

            _logger.LogInformation("Group {groupName} deleted successfull", group.Name);

            _context.Remove(group);
            await _context.SaveChangesAsync();
        }
        public async Task<List<UserResponse>> GetUsers(int id)
        {
            var group = await _context.Groups
                .Include(g => g.Users)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (group == null)
                throw new NotFoundException("Group does not exist");

            var list = _mapper.Map<List<UserResponse>>(group.Users);
            return list;
        }
        public async Task AddUser(int id, string name)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == name);
            var group = await _context.Groups.FindAsync(id);

            if (user == null)
                throw new NotFoundException("User does not exist");

            if (group == null)
                throw new NotFoundException("Group does not exist");

            _logger.LogInformation("{Username} added to group {groupName} successfully", user.Username, group.Name);

            user.Groups.Add(group);
            await _context.SaveChangesAsync();
        }
        public async Task AddHW(int id, CreateHWRequest request)
        {
            var group = await _context.Groups.FindAsync(id);

            if (group == null)
                throw new NotFoundException("Group does not exist");

            var hw = _mapper.Map<Homework>(request);
            List<Problem> problems = [];
            foreach (string name in request.ProblemNames)
            {
                var problem = await _context.Problems
                                            .Where(p => p.Name == name)
                                            .FirstOrDefaultAsync();
                if (problem == null)
                {
                    throw new NotFoundException("Problem does not exist");
                }
                problems.Add(problem);
            }
            _context.Homeworks.Add(hw);
            foreach (Problem p in problems)
            {
                hw.Problems.Add(p);
            }
            group.HWs.Add(hw);

            _logger.LogInformation("Homework added to group {groupName} successfully", group.Name);

            await _context.SaveChangesAsync();
        }
        public async Task<List<HomeworkResponse>> GetHomeworks(int id)
        {
            var group = await _context.Groups
                .Include(g => g.HWs)
                .Where(g => g.Id == id)
                .FirstOrDefaultAsync();

            if (group == null)
                throw new NotFoundException("Group does not exist");

            var list = _mapper.Map<List<HomeworkResponse>>(group.HWs);
            return list;
        }
    }
}
