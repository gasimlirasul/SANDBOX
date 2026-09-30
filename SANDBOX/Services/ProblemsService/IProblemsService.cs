using SANDBOX.Dtos.ProblemDTOs;
using SANDBOX.Dtos.TagDTOs;

namespace SANDBOX.Services.ProblemsService
{
    public interface IProblemsService
    {
        Task<List<ProblemResponse>> GetAllProblems();
        Task<ProblemResponse?> GetProblem(int id);
        Task<ProblemResponse> CreateProblem(ProblemRequest request);
        Task<ProblemResponse?> UpdateProblem(int id, ProblemRequest request);
        Task<bool> DeleteProblem(int id);
        Task<bool> AcceptedSubmission(int id, int userId);
        Task<bool> AddTag(string name, TagRequest request);
        Task<List<TagResponse>?> GetProblemTags(string name);
    }
}
