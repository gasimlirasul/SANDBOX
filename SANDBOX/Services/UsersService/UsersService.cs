using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SANDBOX.Data;
using SANDBOX.Dtos.ProblemDTOs;
using SANDBOX.Dtos.UserDTOs;
using SANDBOX.Exceptions;
using SANDBOX.Models;
using System.Security.Claims;

namespace SANDBOX.Services.UsersService
{
    public class UsersService(AppDbContext _context, IMapper _mapper, ILogger<UsersService> _logger) : IUsersService
    {
        public async Task<List<UserResponse>> GetAllUsers()
        {
            return await _context.Users
                .ProjectTo<UserResponse>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<UserResponse> GetUser(int id)
        {
            var user = await _context.Users
                .Where(u => u.Id == id)
                .ProjectTo<UserResponse>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            if (user == null)
                throw new NotFoundException("User does not exist");

            return user;
        }
        public async Task<UserResponse> UpdateUser(int id, UpdateUserRequest updatedUser)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                throw new NotFoundException("User does not exist");

            _mapper.Map(updatedUser, user);
            _logger.LogInformation("User updated successfully");

            await _context.SaveChangesAsync();
            var response = _mapper.Map<UserResponse>(user);
            return response;
        }
        public async Task DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                throw new NotFoundException("User does not exist");

            _logger.LogInformation("User deleted successfully");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
        public async Task<List<ProblemResponse>> AllSolvedProblems(int id)
        {
            var user = await _context.Users
                .Include(u => u.SolvedProblems)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                throw new NotFoundException("User does not exist");

            var list = _mapper.Map<List<ProblemResponse>>(user.SolvedProblems);
            return list;
        }
    }
}
