using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Authorization;
using FinActions.Application.Categoria.Services;
using FinActions.Application.Categoria.Requests;
using FinActions.Application.Categoria.Responses;
using FinActions.Application.Base.Responses;

namespace FinActions.Api.Host.Controllers;

[ApiController]
[Route("api/{controller}")]
[Authorize]
public class CategoriaController : ControllerBase
{
    private readonly ICategoriaService _service;
    private readonly Guid _userId;

    public CategoriaController(CategoriaService service)
    {
        _service = service;
        var claimValue = HttpContext.User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub).Value;
        var guidParse = Guid.TryParse(claimValue, out _userId);
    }

    [HttpGet]
    [Route("/")]
    public async Task<Results<Ok<PagedResultDto<CategoriaResponseDto>>, ProblemHttpResult>> Obter(GetCategoriaRequestDto requestDto)
    {
        var userRequest = new GetCategoriaRequestDto
        {
            UserId = _userId,
            Skip = requestDto.Skip,
            Take = requestDto.Take
        };

        return await _service.ObterCategorias(userRequest);
    }

    [HttpGet]
    [Route("/{id}")]
    public async Task<Results<Ok<CategoriaResponseDto>, ProblemHttpResult>> ObterPorId()
    {
        return await _service.ObterPorId(_userId);
    }

    [HttpPost]
    [Route("/")]
    public async Task<Results<Ok<CategoriaResponseDto>, ProblemHttpResult>> Insert(PostCategoriaRequestDto categoriaRequestDto)
        => await _service.Insert(categoriaRequestDto with { UserId = _userId });

    [HttpPut]
    [Route("/")]
    public async Task<Results<Ok<CategoriaResponseDto>, ProblemHttpResult>> Update(PostCategoriaRequestDto categoriaRequestDto)
        => await _service.Update(categoriaRequestDto with { UserId = _userId });

    [HttpDelete]
    [Route("/")]
    public async Task<Results<NoContent, ProblemHttpResult>> Delete(Guid id)
        => await _service.Delete(id);
}
