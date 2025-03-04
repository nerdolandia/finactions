using System.ComponentModel.DataAnnotations;
using FinActions.Application.Base.Responses;
using FinActions.Application.ContasBancarias.Contracts.Requests;
using FinActions.Application.ContasBancarias.Contracts.Responses;
using FinActions.Application.ContasBancarias.Contracts.Services;
using FinActions.Domain.Shared.Extensions;
using FinActions.Domain.Shared.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace FinActions.Api.Host.Controllers.ContasBancarias;

[ApiController]
[Route("api/conta-bancaria")]
[Authorize]
public class ContaBancariaController : ControllerBase
{
    private readonly IContaBancariaService _service;

    public ContaBancariaController(IContaBancariaService service)
    {
        _service = service;
    }

    [Authorize(nameof(FinActionsPermissions.ContaBancariaConsultar))]
    [HttpGet]
    public async Task<Results<Ok<PagedResultDto<ContaBancariaResponseDto>>, ProblemHttpResult>> Obter(
        [FromQuery] GetContaBancariaRequestDto requestDto)
    {
        var userRequest = new GetContaBancariaRequestDto
        {
            UserId = User.GetUserId(),
            Skip = requestDto.Skip,
            Take = requestDto.Take
        };

        return await _service.ObterContaBancarias(userRequest, User.GetUserId());
    }

    [Authorize(nameof(FinActionsPermissions.ContaBancariaConsultar))]
    [HttpGet("{id:guid}")]
    public async Task<Results<Ok<ContaBancariaResponseDto>, ProblemHttpResult>> ObterPorId(
        [FromRoute][Required] Guid id)
        => await _service.ObterPorId(id, User.GetUserId());

    [Authorize(nameof(FinActionsPermissions.ContaBancariaCriar))]
    [HttpPost]
    public async Task<Results<Ok<ContaBancariaResponseDto>, ProblemHttpResult>> Insert(
        [FromBody] PostPutContaBancariaRequestDto contaBancariaRequestDto)
        => await _service.Insert(contaBancariaRequestDto, User.GetUserId());

    [Authorize(nameof(FinActionsPermissions.ContaBancariaEditar))]
    [HttpPut("{id:guid}")]
    public async Task<Results<Ok<ContaBancariaResponseDto>, ProblemHttpResult>> Update(
        [FromRoute] Guid id,
        [FromBody] PostPutContaBancariaRequestDto contaBancariaRequestDto)
        => await _service.Update(id, User.GetUserId(), contaBancariaRequestDto);

    [Authorize(nameof(FinActionsPermissions.ContaBancariaRemover))]
    [HttpDelete("{id:guid}")]
    public async Task<Results<NoContent, ProblemHttpResult>> Delete(
        [FromRoute][Required] Guid id)
        => await _service.Delete(id, User.GetUserId());
}
