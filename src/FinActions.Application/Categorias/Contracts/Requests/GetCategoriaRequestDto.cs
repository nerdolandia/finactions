using FinActions.Application.Base.Requests;
namespace FinActions.Application.Categorias.Requests;

public sealed record GetCategoriaRequestDto : PagedResultRequestDto
{
    public string Nome { get; init; }
}
