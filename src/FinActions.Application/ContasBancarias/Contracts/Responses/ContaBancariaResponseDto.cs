using FinActions.Application.Base;
using FinActions.Domain.ContasBancarias;

namespace FinActions.Application.ContasBancarias.Contracts.Responses;

public sealed record ContaBancariaResponseDto : BaseEntityDto
{
    public string Nome { get; set; }
    public TipoContaEnum TipoConta { get; set; }
    public decimal Saldo { get; set; }
}
