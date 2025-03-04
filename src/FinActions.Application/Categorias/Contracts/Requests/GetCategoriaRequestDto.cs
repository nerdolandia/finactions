using FinActions.Application.Base.Requests;

namespace FinActions.Application.Categorias.Requests;

public sealed record GetCategoriaRequestDto : UserPagedResultRequestDto
{
    public string Nome { get; init; }
}
