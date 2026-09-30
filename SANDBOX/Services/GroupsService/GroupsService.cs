using Microsoft.EntityFrameworkCore;
using SANDBOX.Data;
using SANDBOX.Dtos.GroupDTOs;
using SANDBOX.Dtos.HomeworkDTOs;
using SANDBOX.Dtos.UserDTOs;
using SANDBOX.Models;

namespace SANDBOX.Services.GroupsService
{
    public class GroupsService(AppDbContext _context) : IGroupsService
    {
        public async Task<List<GroupResponse>> GetAllGroups()
        {
            return await _context.Groups.Select(g => new GroupResponse
            {
                Name = g.Name
            })
           .ToListAsync();
        }
        public async Task<GroupResponse?> GetGroup(int id)
        {
            var group = await _context.Groups
                .Where(g => g.Id == id)
                .Select(g => new GroupResponse
                {
                    Name = g.Name
                })
                .FirstOrDefaultAsync();
            if (group == null)
                return null;
            return group;
        }
        public async Task<GroupResponse> CreateGroup(GroupRequest request)
        {
            Group newGroup = new Group
            {
                Name = request.Name
            };
            _context.Add(newGroup);
            await _context.SaveChangesAsync();

            var response = new GroupResponse
            {
                Id = newGroup.Id,
                Name = newGroup.Name
            };
            return response;
        }
        public async Task<GroupResponse?> UpdateGroup(int id, GroupRequest request)
        {
            var group = await _context.Groups.FindAsync(id);
            if (group == null)
                return null;
            group.Name = request.Name;
            await _context.SaveChangesAsync();
            var response = new GroupResponse
            {
                Name = group.Name
            };
            return response;
        }
        public async Task<bool> DeleteGroup(int id)
        {
            var group = await _context.Groups.FindAsync(id);
            if (group == null)
                return false;
            _context.Remove(group);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<List<UserResponse>?> GetUsers(int id)
        {
            var group = await _context.Groups
                .Include(g => g.Users)
                .FirstOrDefaultAsync(g => g.Id == id);
            if (group == null)
                return null;

            var list = group.Users
                .Select(u => new UserResponse
                {
                    Username = u.Username
                })
                .ToList();
            return list;
        }
        public async Task<bool> AddUser(int id, string name)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == name);
            var group = await _context.Groups.FindAsync(id);

            if (user == null)
                return false;

            if (group == null)
                return false;

            user.Groups.Add(group);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> AddHW(int id, CreateHWRequest request)
        {
            var group = await _context.Groups.FindAsync(id);
            if (group == null)
                return false;
            var hw = new Homework
            {
                Name = request.Name,
                Deadline = request.Deadline
            };
            List<Problem> problems = [];
            foreach (string name in request.ProblemNames)
            {
                var problem = await _context.Problems
                                            .Where(p => p.Name == name)
                                            .FirstOrDefaultAsync();
                if (problem == null)
                {
                    return false;
                }
                problems.Add(problem);
            }
            _context.Homeworks.Add(hw);
            foreach (Problem p in problems)
            {
                hw.Problems.Add(p);
            }
            group.HWs.Add(hw);

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<List<HomeworkResponse>?> GetHomeworks(int id)
        {
            var group = await _context.Groups
                .Include(g => g.HWs)
                .Where(g => g.Id == id)
                .FirstOrDefaultAsync();
            if (group == null)
                return null;

            var list = group.HWs
                .Select(h => new HomeworkResponse
                {
                    Name = h.Name,
                    Deadline = h.Deadline
                })
                .ToList();
            return list;
        }
    }
}
