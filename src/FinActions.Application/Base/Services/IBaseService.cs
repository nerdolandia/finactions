using FinActions.Domain.Shared.DependencyInjection;
using FinActions.Application.Base.Responses;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FinActions.Application.Base.Services;
public interface IBaseService<TResponse, TGetRequest, TPostRequest> : ITransientDependency
{
    Task<Results<Ok<PagedResultDto<TResponse>>, ProblemHttpResult>> ObterCategorias(TGetRequest categoriaRequestDto);
    Task<Results<Ok<TResponse>, ProblemHttpResult>> ObterPorId(Guid id);
    Task<Results<Ok<TResponse>, ProblemHttpResult>> Insert(TPostRequest categoriaRequestDto);
    Task<Results<Ok<TResponse>, ProblemHttpResult>> Update(TPostRequest categoriaRequestDto);
    Task<Results<NoContent, ProblemHttpResult>> Delete(Guid id);
}
