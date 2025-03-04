using FinActions.Application.Base.Requests;

namespace FinActions.Application.Categoria.Requests;
public sealed class GetCategoriaRequestDto : PagedResultRequestDto
{
    public Guid UserId { get; set; }
}
