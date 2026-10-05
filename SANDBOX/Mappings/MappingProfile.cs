using AutoMapper;
using SANDBOX.Dtos.GroupDTOs;
using SANDBOX.Dtos.ProblemDTOs;
using SANDBOX.Dtos.TagDTOs;
using SANDBOX.Dtos.UserDTOs;
using SANDBOX.Models;

namespace SANDBOX.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserResponse>();
            CreateMap<UpdateUserRequest, User>();

            CreateMap<Problem, ProblemResponse>();
            CreateMap<ProblemRequest, Problem>();

            CreateMap<Tag, TagResponse>();
            CreateMap<TagRequest, Tag>();

            CreateMap<Group, GroupResponse>();
            CreateMap<GroupRequest, Group>();

        }
    }
}
