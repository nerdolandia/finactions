using FinActions.Application.Base;
using FinActions.Application.Categorias.Responses;
using FinActions.Application.ContasBancarias.Contracts.Responses;
using FinActions.Domain.ContasBancarias;
using FinActions.Domain.Movimentacoes;

namespace FinActions.Application.Movimentacoes.Contracts.Responses;

public sealed record MovimentacaoResponseDto : BaseEntityDto
{
    public TipoMovimentacaoEnum TipoMovimentacao { get; set; }
    public string Descricao { get; set; }
    public string Tag { get; set; }
    public string Cor { get; set; }
    public decimal ValorMovimentado { get; set; }
    public DateTimeOffset DataMovimentacao { get; set; }
    public Guid ContaBancariaId { get; set; }
    public ContaBancariaResponseDto ContaBancaria { get; set; }
    public Guid CategoriaId { get; set; }
    public CategoriaResponseDto Categoria { get; set; }
}
