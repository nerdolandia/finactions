public record struct MovimentacoesEntity : IBaseEntity
{
    public Guid Id { get; set; }
    public DateTimeOffset DataModificacao { get; set; }
    public DateTimeOffset DataCriacao { get; set; }
    public TipoMovimentacaoEnum TipoMovimentacao { get; set; }
    public string Descricao { get; set; }
    public string? Tag { get; set; }
    public string? Cor { get; set; } 
    public Guid CategoriaId { get; set; }
    public Guid ContaId { get; set; }
    public decimal ValorMovimentado { get; set; }
    public DateTimeOffset DataMovimentacao { get; set; }
}
