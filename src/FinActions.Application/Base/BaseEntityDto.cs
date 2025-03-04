namespace FinActions.Application.Base;

public abstract record BaseEntityDto
{
     Guid Id { get; set; }
     DateTimeOffset? DataModificacao { get; set; }
     DateTimeOffset DataCriacao { get; set; }
}
