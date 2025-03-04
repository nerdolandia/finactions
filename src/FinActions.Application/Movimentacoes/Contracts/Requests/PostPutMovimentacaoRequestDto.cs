using System.Text.Json.Serialization;
using FinActions.Application.Base.Requests;
using FinActions.Domain.ContasBancarias;
using FinActions.Domain.Movimentacoes;

namespace FinActions.Application.Movimentacoes.Contracts.Requests;

public record struct PostPutMovimentacaoRequestDto
{
    public TipoMovimentacaoEnum TipoMovimentacao { get; set; }
    public string Descricao { get; set; }
    public string Tag { get; set; }
    public string Cor { get; set; }
    public decimal ValorMovimentado { get; set; }
    public DateTimeOffset DataMovimentacao { get; set; }
    public Guid ContaBancariaId { get; set; }
    public Guid CategoriaId { get; set; }
}
