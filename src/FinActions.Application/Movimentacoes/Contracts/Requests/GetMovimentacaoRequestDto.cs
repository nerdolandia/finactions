using System.Text.Json.Serialization;
using FinActions.Application.Base.Requests;
using FinActions.Domain.ContasBancarias;
using FinActions.Domain.Movimentacoes;

namespace FinActions.Application.Movimentacoes.Contracts.Requests;

public sealed record GetMovimentacaoRequestDto : PagedResultRequestDto
{
    public TipoMovimentacaoEnum? TipoMovimentacao { get; set; }
    public string Descricao { get; set; }
    public string Tag { get; set; }
    public DateTimeOffset? DataMovimentacaoInicio { get; set; }
    public DateTimeOffset? DataMovimentacaoFim { get; set; }
    public Guid? ContaBancariaId { get; set; }
    public Guid? CategoriaId { get; set; }
}
