namespace FinActions.Domain;
sealed public class Movimentacao : IBaseEntity
{
    public required Guid Id { get; set; }
    public DateTimeOffset? DataModificacao { get; set; }
    public DateTimeOffset DataCriacao { get; set; }
    public required TipoMovimentacaoEnum TipoMovimentacao { get; set; }
    public required string Descricao { get; set; }
    public string? Tag { get; set; }
    public string? Cor { get; set; }
    public required decimal ValorMovimentado { get; set; }
    public required DateTimeOffset DataMovimentacao { get; set; }

    public Guid ContaBancariaId { get; set; }
    public ContaBancaria ContaBancaria { get; set; }
    public Guid CategoriaId { get; set; }
    public Categoria Categoria { get; set; }

}
