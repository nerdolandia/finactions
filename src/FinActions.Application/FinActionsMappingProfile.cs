using AutoMapper;
using FinActions.Application.Categorias.Requests;
using FinActions.Application.Categorias.Responses;
using FinActions.Application.Identity.Contracts.Responses;
using FinActions.Domain.Categorias;
using FinActions.Domain.Identity;

namespace FinActions.Application
{
    public class FinActionsMappingProfile : Profile
    {
        public FinActionsMappingProfile()
        {
            CreateMap<AppUser, AppUserDto>().ReverseMap();

            CreateMap<Categoria, CategoriaResponseDto>().ReverseMap();
            CreateMap<Categoria, PostCategoriaRequestDto>().ReverseMap();
        }
    }
}
