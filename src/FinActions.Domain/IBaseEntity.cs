public interface IBaseEntity
{
    Guid Id { get; set; } 
    DateTimeOffset? DataModificacao { get; set; }
    DateTimeOffset DataCriacao { get; set; }
}
