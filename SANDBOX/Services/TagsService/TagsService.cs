using Microsoft.EntityFrameworkCore;
using SANDBOX.Data;
using SANDBOX.Dtos.TagDTOs;
using SANDBOX.Models;

namespace SANDBOX.Services.TagsService
{
    public class TagsService(AppDbContext _context) : ITagsService
    {
        public async Task<List<TagResponse>> GetAllTags()
        {
            return await _context.Tags.Select(t => new TagResponse
            {
                Name = t.Name
            })
            .ToListAsync();
        }
        public async Task<TagResponse?> GetTag(int id)
        {
            var tag = await _context.Tags
                .Where(t => t.Id == id)
                .Select(u => new TagResponse
                {
                    Name = u.Name
                })
                .FirstOrDefaultAsync();

            if (tag == null)
                return null;

            return tag;
        }
        public async Task<TagResponse?> UpdateTag(int id, TagRequest request)
        {
            var tag = await _context.Tags.FindAsync(id);
            if (tag == null)
                return null;
            tag.Name = request.Name;
            await _context.SaveChangesAsync();
            TagResponse response = new TagResponse
            {
                Name = tag.Name
            };
            return response;
        }
        public async Task<TagResponse> CreateTag(TagRequest request)
        {
            var newTag = new Tag
            {
                Name = request.Name
            };
            _context.Add(newTag);
            await _context.SaveChangesAsync();
            var response = new TagResponse
            {
                Id = newTag.Id,
                Name = newTag.Name
            };
            return response;
        }
        public async Task<bool> DeleteTag(TagRequest deletedTag)
        {
            var tag = await _context.Tags.Where(t => t.Name == deletedTag.Name).FirstOrDefaultAsync();
            if (tag == null)
                return false;
            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
