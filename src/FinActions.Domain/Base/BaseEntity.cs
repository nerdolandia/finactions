namespace FinActions.Domain.Base;
public abstract class BaseEntity
{
    public Guid Id { get; set; } 
    public DateTimeOffset? DataModificacao { get; set; }
    public DateTimeOffset DataCriacao { get; set; }
    public bool IsDeleted { get; set; }
}
