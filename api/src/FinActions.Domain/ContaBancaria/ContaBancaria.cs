namespace FinActions.Domain;
sealed public class ContaBancaria : IBaseEntity
{
    public DateTimeOffset DataCriacao { get; set; }
    public DateTimeOffset? DataModificacao { get; set; }
    public required Guid Id { get; set; }
    public required string Nome { get; set; }
    public required TipoContaEnum TipoConta { get; set; }
    public required decimal Saldo { get; set; }
    public Movimentacao Movimentacao { get; set; }
}
