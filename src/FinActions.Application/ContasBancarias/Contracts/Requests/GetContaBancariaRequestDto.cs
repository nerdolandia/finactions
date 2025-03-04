using System.Text.Json.Serialization;
using FinActions.Application.Base.Requests;
using FinActions.Domain.ContasBancarias;

namespace FinActions.Application.ContasBancarias.Contracts.Requests;

public sealed record GetContaBancariaRequestDto : PagedResultRequestDto
{
    [JsonIgnore]
    public Guid UserId { get; set; }
    public string Nome { get; set; }
    public TipoContaEnum? TipoConta { get; set; }
}
