using FinActions.Domain.Base;

namespace FinActions.Application.Categorias.Responses;

public sealed class CategoriaResponseDto : BaseEntityDto
{
    public string Nome { get; set; }
}
