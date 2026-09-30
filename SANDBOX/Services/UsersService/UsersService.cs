using Microsoft.EntityFrameworkCore;
using SANDBOX.Data;
using SANDBOX.Dtos.ProblemDTOs;
using SANDBOX.Dtos.UserDTOs;
using SANDBOX.Models;
using System.Security.Claims;

namespace SANDBOX.Services.UsersService
{
    public class UsersService(AppDbContext _context) : IUsersService
    {
        public async Task<List<UserResponse>> GetAllUsers()
        {
            return await _context.Users.Select(u => new UserResponse
            {
                Username = u.Username
            })
            .ToListAsync();
        }

        public async Task<UserResponse?> GetUser(int id)
        {
            var user = await _context.Users
                .Where(u => u.Id == id)
                .Select(u => new UserResponse
                {
                    Username = u.Username
                })
                .FirstOrDefaultAsync();
            if (user == null)
                return null;
            return user;
        }
        public async Task<UserResponse?> UpdateUser(int id, UpdateUserRequest updatedUser)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return null;
            user.Username = updatedUser.Username;
            await _context.SaveChangesAsync();
            UserResponse response = new UserResponse
            {
                Username = user.Username
            };
            return response;
        }
        public async Task<bool> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return false;
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<List<ProblemResponse>?> AllSolvedProblems(int id)
        {
            var user = await _context.Users
                .Include(u => u.SolvedProblems)
                .FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                return null;
            var list = user.SolvedProblems
                .Select(p => new ProblemResponse
                {
                    Name = p.Name
                })
                .ToList();
            return list;
        }
    }
}
