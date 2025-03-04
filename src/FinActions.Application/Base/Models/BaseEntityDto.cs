namespace FinActions.Application.Base.Models;

public abstract record BaseEntityDto
{
    public Guid Id { get; init; }
    public DateTimeOffset? DataModificacao { get; init; }
    public DateTimeOffset DataCriacao { get; init; }
}
