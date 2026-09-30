using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SANDBOX.Data;
using SANDBOX.Dtos.ProblemDTOs;
using SANDBOX.Dtos.TagDTOs;
using SANDBOX.Exceptions;
using SANDBOX.Models;
using System.Security.Claims;

namespace SANDBOX.Services.ProblemsService
{
    public class ProblemsService(AppDbContext _context, IMapper _mapper) : IProblemsService
    {
        public async Task<List<ProblemResponse>> GetAllProblems()
        {
            return await _context.Problems
                .ProjectTo<ProblemResponse>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }
        public async Task<ProblemResponse> GetProblem(int id)
        {
            var problem = await _context.Problems
                .Where(p => p.Id == id)
                .ProjectTo<ProblemResponse>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            if (problem == null)
                throw new NotFoundException("Problem does not exist");

            return problem;
        }
        public async Task<ProblemResponse> CreateProblem(ProblemRequest request)
        {
            var newProblem = _mapper.Map<Problem>(request);

            _context.Problems.Add(newProblem);
            await _context.SaveChangesAsync();

            var response = _mapper.Map<ProblemResponse>(newProblem);
            return response;
        }
        public async Task<ProblemResponse> UpdateProblem(int id, ProblemRequest request)
        {
            var problem = await _context.Problems.FindAsync(id);

            if (problem == null)
                throw new NotFoundException("Problem does not exist");

            _mapper.Map(request, problem);
            
            await _context.SaveChangesAsync();
            var response = _mapper.Map<ProblemResponse>(problem);
            return response;
        }
        public async Task DeleteProblem(int id)
        {
            var problem = await _context.Problems.FindAsync(id);

            if (problem == null)
                throw new NotFoundException("Problem does not exist");

            _context.Problems.Remove(problem);
            await _context.SaveChangesAsync();
        }
        public async Task AcceptedSubmission(int id, int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            var problem = await _context.Problems.FindAsync(id);

            if (problem == null)
                throw new NotFoundException("Problem does not exist");

            if (user == null)
                throw new NotFoundException("Tag does not exist");

            user.SolvedProblems.Add(problem);
            await _context.SaveChangesAsync();
        }
        public async Task AddTag(string name, TagRequest request)
        {
            var tag = await _context.Tags.Where(t => t.Name == request.Name).FirstOrDefaultAsync();
            var problem = await _context.Problems.Where(p => p.Name == name).FirstOrDefaultAsync();

            if (problem == null)
                throw new NotFoundException("Problem does not exist");

            if (tag == null)
                throw new NotFoundException("Tag does not exist");

            problem.Tags.Add(tag);
            await _context.SaveChangesAsync();
        }
        public async Task<List<TagResponse>> GetProblemTags(string name)
        {
            var problem = await _context.Problems
                .Include(p => p.Tags)
                .Where(p => p.Name == name)
                .FirstOrDefaultAsync();

            if (problem == null)
                throw new NotFoundException("Problem does not exist");

            var tags = _mapper.Map<List<TagResponse>>(problem.Tags);
            return tags;
        }
    }
}
