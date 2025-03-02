public record struct ContaBancariaEntity : IBaseEntity
{
    public Guid Id { get; set; }
    public DateTimeOffset DataModificacao { get; set; }
    public DateTimeOffset DataCriacao { get; set; }
    public string Nome { get; set; }
    public decimal Saldo { get; set; }
    public TipoContaEnum TipoConta { get; set; }
}
