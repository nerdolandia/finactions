using FinActions.Application.Base.Responses;
using FinActions.Application.ContasBancarias.Contracts.Requests;
using FinActions.Application.ContasBancarias.Contracts.Responses;
using FinActions.Domain.Shared.DependencyInjection;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FinActions.Application.ContasBancarias.Contracts.Services;

public interface IContaBancariaService : ITransientDependency
{
    Task<Results<Ok<PagedResultDto<ContaBancariaResponseDto>>, ProblemHttpResult>> ObterContaBancarias(
        GetContaBancariaRequestDto getRequest,
        Guid userId);
    Task<Results<Ok<ContaBancariaResponseDto>, ProblemHttpResult>> ObterPorId(Guid contaBancariaId, Guid userId);
    Task<Results<Ok<ContaBancariaResponseDto>, ProblemHttpResult>> Insert(
        PostPutContaBancariaRequestDto insertRequest,
        Guid userId);
    Task<Results<Ok<ContaBancariaResponseDto>, ProblemHttpResult>> Update(
        Guid contaBancariaId,
        Guid userId,
        PostPutContaBancariaRequestDto updateRequest);
    Task<Results<NoContent, ProblemHttpResult>> Delete(Guid contaBancariaId, Guid userId);
}
