using FinActions.Domain.Categorias;
using FinActions.Domain.ContasBancarias;

namespace FinActions.Domain.Movimentacoes;
public sealed class Movimentacao : IBaseEntity
{
    public Guid Id { get; set; }
    public DateTimeOffset? DataModificacao { get; set; }
    public DateTimeOffset DataCriacao { get; set; }
    public TipoMovimentacaoEnum TipoMovimentacao { get; set; }
    public string Descricao { get; set; }
    public string Tag { get; set; }
    public string Cor { get; set; }
    public decimal ValorMovimentado { get; set; }
    public DateTimeOffset DataMovimentacao { get; set; }

    public Guid ContaBancariaId { get; set; }
    public ContaBancaria ContaBancaria { get; set; }
    public Guid CategoriaId { get; set; }
    public Categoria Categoria { get; set; }

}
