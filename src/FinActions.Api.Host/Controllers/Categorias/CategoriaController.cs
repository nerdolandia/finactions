using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FinActions.Application.Categorias.Services;
using FinActions.Application.Categorias.Requests;
using FinActions.Application.Categorias.Responses;
using FinActions.Application.Base.Responses;
using FinActions.Domain.Shared.Security;
using FinActions.Domain.Shared.Extensions;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;

namespace FinActions.Api.Host.Controllers.Categorias;

[ApiController]
[Route("api/categoria")]
[Authorize]
public class CategoriaController : ControllerBase
{
    private readonly ICategoriaService _service;

    public CategoriaController(ICategoriaService service)
    {
        _service = service;
    }

    [Authorize(nameof(FinActionsPermissions.CategoriaConsultar))]
    [HttpGet]
    public async Task<Results<Ok<PagedResultDto<CategoriaResponseDto>>, ProblemHttpResult>> Obter(
        [FromQuery] GetCategoriaRequestDto requestDto)
    {
        var userRequest = new GetCategoriaRequestDto
        {
            UserId = HttpContext.User.GetUserId(),
            Skip = requestDto.Skip,
            Take = requestDto.Take
        };

        return await _service.ObterCategorias(userRequest);
    }

    [Authorize(nameof(FinActionsPermissions.CategoriaConsultar))]
    [HttpGet("{id:guid}")]
    public async Task<Results<Ok<CategoriaResponseDto>, ProblemHttpResult>> ObterPorId(
        [FromRoute][Required] Guid id)
        => await _service.ObterPorId(id, HttpContext.User.GetUserId());

    [Authorize(nameof(FinActionsPermissions.CategoriaCriar))]
    [HttpPost]
    public async Task<Results<Ok<CategoriaResponseDto>, ProblemHttpResult>> Insert(
        [FromBody] PostCategoriaRequestDto categoriaRequestDto)
        => await _service.Insert(categoriaRequestDto with { UserId = HttpContext.User.GetUserId() });

    [Authorize(nameof(FinActionsPermissions.CategoriaEditar))]
    [HttpPut]
    public async Task<Results<Ok<CategoriaResponseDto>, ProblemHttpResult>> Update(
        [FromBody] PostCategoriaRequestDto categoriaRequestDto)
        => await _service.Update(categoriaRequestDto with { UserId = HttpContext.User.GetUserId() });

    [Authorize(nameof(FinActionsPermissions.CategoriaRemover))]
    [HttpDelete("{id:guid}")]
    public async Task<Results<NoContent, ProblemHttpResult>> Delete(
        [FromRoute][Required] Guid id)
        => await _service.Delete(id, HttpContext.User.GetUserId());
}
