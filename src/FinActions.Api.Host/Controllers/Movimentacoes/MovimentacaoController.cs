using System.ComponentModel.DataAnnotations;
using FinActions.Application.Base.Responses;
using FinActions.Application.ContasBancarias.Contracts.Requests;
using FinActions.Application.ContasBancarias.Contracts.Responses;
using FinActions.Application.ContasBancarias.Contracts.Services;
using FinActions.Application.Movimentacoes.Contracts.Requests;
using FinActions.Application.Movimentacoes.Contracts.Responses;
using FinActions.Application.Movimentacoes.Services;
using FinActions.Domain.Shared.Extensions;
using FinActions.Domain.Shared.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace FinActions.Api.Host.Controllers.Movimentacoes;

[ApiController]
[Route("api/movimentacao")]
[Authorize]
public class MovimentacaoController : ControllerBase
{
    private readonly IMovimentacaoService _service;

    public MovimentacaoController(IMovimentacaoService service)
    {
        _service = service;
    }

    [Authorize(nameof(FinActionsPermissions.MovimentacaoConsultar))]
    [HttpGet]
    public async Task<Results<Ok<PagedResultDto<MovimentacaoResponseDto>>, ProblemHttpResult>> Obter(
        [FromQuery] GetMovimentacaoRequestDto requestDto)
    {
        var userRequest = new GetMovimentacaoRequestDto
        {
            Skip = requestDto.Skip,
            Take = requestDto.Take
        };

        return await _service.ObterMovimentacaos(userRequest, User.GetUserId());
    }

    [Authorize(nameof(FinActionsPermissions.MovimentacaoConsultar))]
    [HttpGet("{id:guid}")]
    public async Task<Results<Ok<MovimentacaoResponseDto>, ProblemHttpResult>> ObterPorId(
        [FromRoute][Required] Guid id)
        => await _service.ObterPorId(id, User.GetUserId());

    [Authorize(nameof(FinActionsPermissions.MovimentacaoCriar))]
    [HttpPost]
    public async Task<Results<Ok<MovimentacaoResponseDto>, ProblemHttpResult>> Insert(
        [FromBody] PostPutMovimentacaoRequestDto MovimentacaoRequestDto)
        => await _service.Insert(MovimentacaoRequestDto, User.GetUserId());

    [Authorize(nameof(FinActionsPermissions.MovimentacaoEditar))]
    [HttpPut("{id:guid}")]
    public async Task<Results<Ok<MovimentacaoResponseDto>, ProblemHttpResult>> Update(
        [FromRoute] Guid id,
        [FromBody] PostPutMovimentacaoRequestDto MovimentacaoRequestDto)
        => await _service.Update(id, User.GetUserId(), MovimentacaoRequestDto);

    [Authorize(nameof(FinActionsPermissions.MovimentacaoRemover))]
    [HttpDelete("{id:guid}")]
    public async Task<Results<NoContent, ProblemHttpResult>> Delete(
        [FromRoute][Required] Guid id)
        => await _service.Delete(id, User.GetUserId());
}
