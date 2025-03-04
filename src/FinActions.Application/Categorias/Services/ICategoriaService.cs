using FinActions.Domain.Shared.DependencyInjection;
using FinActions.Application.Categorias.Requests;
using FinActions.Application.Categorias.Responses;
using FinActions.Application.Base.Responses;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FinActions.Application.Categorias.Services;

public interface ICategoriaService : ITransientDependency
{
    Task<Results<Ok<PagedResultDto<CategoriaResponseDto>>, ProblemHttpResult>> ObterCategorias(GetCategoriaRequestDto categoriaRequestDto, Guid userId);
    Task<Results<Ok<CategoriaResponseDto>, ProblemHttpResult>> ObterPorId(Guid categoriaId, Guid userId);
    Task<Results<Ok<CategoriaResponseDto>, ProblemHttpResult>> Insert(PostCategoriaRequestDto categoriaRequestDto, Guid userId);
    Task<Results<Ok<CategoriaResponseDto>, ProblemHttpResult>> Update(PostCategoriaRequestDto categoriaRequestDto, Guid userId, Guid id);
    Task<Results<NoContent, ProblemHttpResult>> Delete(Guid categoriaId, Guid userId);
}
