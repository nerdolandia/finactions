using FinActions.Domain.Shared.DependencyInjection;
using FinActions.Application.Categoria.Requests;
using FinActions.Application.Categoria.Responses;
using FinActions.Application.Base.Responses;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FinActions.Application.Categoria.Services;
public interface ICategoriaService : ITransientDependency
{
    Task<Results<Ok<PagedResultDto<CategoriaResponseDto>>, ProblemHttpResult>> ObterCategorias(GetCategoriaRequestDto categoriaRequestDto);


    Task<Results<Ok<CategoriaResponseDto>, ProblemHttpResult>> ObterPorId(Guid id);

    Task<Results<Created<CategoriaResponseDto>, ProblemHttpResult>> Insert(PostCategoriaRequestDto categoriaRequestDto);

    Task<Results<Ok<CategoriaResponseDto>, ProblemHttpResult>> Update(PostCategoriaRequestDto categoriaRequestDto);
    
    Task<Results<NoContent, ProblemHttpResult>> Delete(Guid id);
}
