using System.Text.Json.Serialization;
using FinActions.Domain.ContasBancarias;

namespace FinActions.Application.ContasBancarias.Contracts.Requests;

public record struct PostPutContaBancariaRequestDto
{
    public string Nome { get; set; }
    public TipoContaEnum TipoConta { get; set; }
    public decimal Saldo { get; set; }
    [JsonIgnore]
    public Guid UserId { get; set; }
}
