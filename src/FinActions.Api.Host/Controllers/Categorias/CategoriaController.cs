using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FinActions.Application.Categorias.Services;
using FinActions.Application.Categorias.Requests;
using FinActions.Application.Categorias.Responses;
using FinActions.Application.Base.Responses;
using FinActions.Domain.Shared.Security;
using FinActions.Domain.Shared.Extensions;
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
        var userId = HttpContext.User.GetUserId();
        var userRequest = new GetCategoriaRequestDto
        {
            Skip = requestDto.Skip,
            Take = requestDto.Take
        };

        return await _service.ObterCategorias(userRequest, userId);
    }

    [Authorize(nameof(FinActionsPermissions.CategoriaConsultar))]
    [HttpGet("{id:guid}")]
    public async Task<Results<Ok<CategoriaResponseDto>, ProblemHttpResult>> ObterPorId(
        [FromRoute][Required] Guid id)
        => await _service.ObterPorId(new IdsCategoriaRequestDto(id, HttpContext.User.GetUserId()));

    [Authorize(nameof(FinActionsPermissions.CategoriaCriar))]
    [HttpPost]
    public async Task<Results<Ok<CategoriaResponseDto>, ProblemHttpResult>> Insert(
        [FromBody] PostCategoriaRequestDto categoriaRequestDto)
        => await _service.Insert(categoriaRequestDto with { userId = HttpContext.User.GetUserId()});

    [Authorize(nameof(FinActionsPermissions.CategoriaEditar))]
    [HttpPut("{id:guid}")]
    public async Task<Results<Ok<CategoriaResponseDto>, ProblemHttpResult>> Update(
        [FromBody] PostCategoriaRequestDto categoriaRequestDto, 
        [FromRoute][Required] Guid id)
        => await _service.Update(categoriaRequestDto with {userId= HttpContext.User.GetUserId()}, id);

    [Authorize(nameof(FinActionsPermissions.CategoriaRemover))]
    [HttpDelete("{id:guid}")]
    public async Task<Results<NoContent, ProblemHttpResult>> Delete(
        [FromRoute][Required] Guid id)
        => await _service.Delete(new IdsCategoriaRequestDto(id, HttpContext.User.GetUserId()));
}
