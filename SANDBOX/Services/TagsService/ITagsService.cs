using SANDBOX.Dtos.TagDTOs;

namespace SANDBOX.Services.TagsService
{
    public interface ITagsService
    {
        Task<List<TagResponse>> GetAllTags();
        Task<TagResponse> GetTag(int id);
        Task<TagResponse> UpdateTag(int id, TagRequest request);
        Task<TagResponse> CreateTag(TagRequest request);
        Task DeleteTag(TagRequest deletedTag);
    }
}
