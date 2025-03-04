using AutoMapper;
using FinActions.Application.Categorias.Requests;
using FinActions.Application.Categorias.Responses;
using FinActions.Application.ContasBancarias.Contracts.Requests;
using FinActions.Application.ContasBancarias.Contracts.Responses;
using FinActions.Application.Identity.Contracts.Responses;
using FinActions.Application.Movimentacoes.Contracts.Requests;
using FinActions.Application.Movimentacoes.Contracts.Responses;
using FinActions.Domain.Categorias;
using FinActions.Domain.ContasBancarias;
using FinActions.Domain.Identity;
using FinActions.Domain.Movimentacoes;

namespace FinActions.Application
{
    public class FinActionsMappingProfile : Profile
    {
        public FinActionsMappingProfile()
        {
            CreateMap<AppUser, AppUserDto>().ReverseMap();

            CreateMap<Categoria, CategoriaResponseDto>().ReverseMap();
            CreateMap<Categoria, PostCategoriaRequestDto>().ReverseMap();

            CreateMap<ContaBancaria, ContaBancariaResponseDto>().ReverseMap();
            CreateMap<ContaBancaria, PostPutContaBancariaRequestDto>().ReverseMap();

            CreateMap<Movimentacao, MovimentacaoResponseDto>().ReverseMap();
            CreateMap<Movimentacao, PostPutMovimentacaoRequestDto>().ReverseMap();
        }
    }
}
