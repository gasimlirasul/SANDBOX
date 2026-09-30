using Microsoft.EntityFrameworkCore;
using SANDBOX.Data;
using SANDBOX.Dtos.ProblemDTOs;
using SANDBOX.Dtos.TagDTOs;
using SANDBOX.Models;
using System.Security.Claims;

namespace SANDBOX.Services.ProblemsService
{
    public class ProblemsService(AppDbContext _context) : IProblemsService
    {
        public async Task<List<ProblemResponse>> GetAllProblems()
        {
            return await _context.Problems.Select(p => new ProblemResponse
            {
                Name = p.Name
            })
            .ToListAsync();
        }
        public async Task<ProblemResponse?> GetProblem(int id)
        {
            var problem = await _context.Problems
                .Where(p => p.Id == id)
                .Select(p => new ProblemResponse
                {
                    Name = p.Name
                })
                .FirstOrDefaultAsync();

            if (problem == null)
                return null;

            return problem;
        }
        public async Task<ProblemResponse> CreateProblem(ProblemRequest request)
        {
            Problem newProblem = new Problem
            {
                Name = request.Name,
            };

            _context.Problems.Add(newProblem);
            await _context.SaveChangesAsync();

            ProblemResponse response = new ProblemResponse
            {
                Id = newProblem.Id,
                Name = request.Name
            };
            return response;
        }
        public async Task<ProblemResponse?> UpdateProblem(int id, ProblemRequest request)
        {
            var problem = await _context.Problems.FindAsync(id);
            if (problem == null)
                return null;
            problem.Name = request.Name;
            await _context.SaveChangesAsync();
            ProblemResponse response = new ProblemResponse
            {
                Name = problem.Name
            };
            return response;
        }
        public async Task<bool> DeleteProblem(int id)
        {
            var problem = await _context.Problems.FindAsync(id);
            if (problem == null)
                return false;
            _context.Problems.Remove(problem);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> AcceptedSubmission(int id, int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            var problem = await _context.Problems.FindAsync(id);

            if (problem == null)
                return false;
            if (user == null)
                return false;
            user.SolvedProblems.Add(problem);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> AddTag(string name, TagRequest request)
        {
            var tag = await _context.Tags.Where(t => t.Name == request.Name).FirstOrDefaultAsync();
            var problem = await _context.Problems.Where(p => p.Name == name).FirstOrDefaultAsync();
            if (tag == null)
                return false;
            if (problem == null)
                return false;

            problem.Tags.Add(tag);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<List<TagResponse>?> GetProblemTags(string name)
        {
            var problem = await _context.Problems
                .Include(p => p.Tags)
                .Where(p => p.Name == name)
                .FirstOrDefaultAsync();
            if (problem == null)
                return null;
            var tags = problem.Tags.Select(t => new TagResponse
            {
                Name = t.Name
            })
            .ToList();
            return tags;
        }
    }
}
