using AutoMapper;
using FinActions.Application.Identity.Contracts.Responses;
using FinActions.Domain.Identity;

namespace FinActions.Application
{
    public class FinActionsMappingProfile : Profile
    {
        public FinActionsMappingProfile()
        {
            CreateMap<AppUser, AppUserDto>()
                .ReverseMap();
        }
    }
}
