using SANDBOX.Dtos.GroupDTOs;
using SANDBOX.Dtos.HomeworkDTOs;
using SANDBOX.Dtos.UserDTOs;

namespace SANDBOX.Services.GroupsService
{
    public interface IGroupsService
    {
        Task<List<GroupResponse>> GetAllGroups();
        Task<GroupResponse?> GetGroup(int id);
        Task<GroupResponse> CreateGroup(GroupRequest request);
        Task<GroupResponse?> UpdateGroup(int id, GroupRequest request);
        Task<bool> DeleteGroup(int id);
        Task<List<UserResponse>?> GetUsers(int id);
        Task<bool> AddUser(int id, string name);
        Task<bool> AddHW(int id, CreateHWRequest request);
        Task<List<HomeworkResponse>?> GetHomeworks(int id);
    }
}
