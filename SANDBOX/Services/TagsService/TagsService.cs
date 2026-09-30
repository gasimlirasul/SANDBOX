using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SANDBOX.Data;
using SANDBOX.Dtos.TagDTOs;
using SANDBOX.Exceptions;
using SANDBOX.Models;

namespace SANDBOX.Services.TagsService
{
    public class TagsService(AppDbContext _context, IMapper _mapper) : ITagsService
    {
        public async Task<List<TagResponse>> GetAllTags()
        {
            return await _context.Tags
                .ProjectTo<TagResponse>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }
        public async Task<TagResponse> GetTag(int id)
        {
            var tag = await _context.Tags
                .Where(t => t.Id == id)
                .ProjectTo<TagResponse>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            if (tag == null)
                throw new NotFoundException("Tag does not exist");

            return tag;
        }
        public async Task<TagResponse> UpdateTag(int id, TagRequest request)
        {
            var tag = await _context.Tags.FindAsync(id);

            if (tag == null)
                throw new NotFoundException("Tag does not exist");

            _mapper.Map(request, tag);

            await _context.SaveChangesAsync();
            var response = _mapper.Map<TagResponse>(tag);
            return response;
        }
        public async Task<TagResponse> CreateTag(TagRequest request)
        {
            var newTag = _mapper.Map<Tag>(request);
            _context.Add(newTag);
            await _context.SaveChangesAsync();
            var response = _mapper.Map<TagResponse>(newTag);
            return response;
        }
        public async Task DeleteTag(TagRequest deletedTag)
        {
            var tag = await _context.Tags.Where(t => t.Name == deletedTag.Name).FirstOrDefaultAsync();

            if (tag == null)
                throw new NotFoundException("Tag does not exist");

            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();
        }
    }
}
