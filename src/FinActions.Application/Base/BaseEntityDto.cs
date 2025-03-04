namespace FinActions.Application.Base;

public abstract record BaseEntityDto
{
     Guid Id { get; init; }
     DateTimeOffset? DataModificacao { get; init; }
     DateTimeOffset DataCriacao { get; init; }
}
