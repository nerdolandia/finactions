using FinActions.Application.Base.Requests;
namespace FinActions.Application.Categorias.Requests;

public sealed record PostCategoriaRequestDto : UserResultRequestDto
{
    public string Nome { get; init; }
}
