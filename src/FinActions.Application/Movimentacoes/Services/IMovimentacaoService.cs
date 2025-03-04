
using FinActions.Application.Base.Responses;
using FinActions.Application.Movimentacoes.Contracts.Requests;
using FinActions.Application.Movimentacoes.Contracts.Responses;
using FinActions.Domain.Shared.DependencyInjection;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FinActions.Application.Movimentacoes.Services;

public interface IMovimentacaoService : ITransientDependency
{
    Task<Results<Ok<PagedResultDto<MovimentacaoResponseDto>>, ProblemHttpResult>> ObterMovimentacaos(
        GetMovimentacaoRequestDto getRequest,
        Guid userId);
    Task<Results<Ok<MovimentacaoResponseDto>, ProblemHttpResult>> ObterPorId(Guid MovimentacaoId, Guid userId);
    Task<Results<Ok<MovimentacaoResponseDto>, ProblemHttpResult>> Insert(
        PostPutMovimentacaoRequestDto insertRequest,
        Guid userId);
    Task<Results<Ok<MovimentacaoResponseDto>, ProblemHttpResult>> Update(
        Guid MovimentacaoId,
        Guid userId,
        PostPutMovimentacaoRequestDto updateRequest);
    Task<Results<NoContent, ProblemHttpResult>> Delete(Guid MovimentacaoId, Guid userId);
}
