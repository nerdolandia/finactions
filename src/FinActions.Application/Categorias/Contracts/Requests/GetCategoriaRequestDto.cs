using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using FinActions.Application.Base.Requests;

namespace FinActions.Application.Categorias.Requests;

public sealed class GetCategoriaRequestDto : PagedResultRequestDto
{
    [IgnoreDataMember]
    public Guid UserId { get; set; }
    public string Nome { get; set; }
}
