using FinActions.Application.Base;
namespace FinActions.Application.Categorias.Responses;

public sealed record CategoriaResponseDto : BaseEntityDto
{
    public string Nome { get; set; }
}
