public record struct CategoriaEntity : IBaseEntity
{
    public Guid Id { get; set; }
    public DateTimeOffset DataModificacao { get; set; }
    public DateTimeOffset DataCriacao { get; set; }
    public string Nome { get; set; }
}
