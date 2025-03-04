using System.Text.Json.Serialization;

namespace FinActions.Domain.Base;

public abstract class BaseEntityDto
{
    public Guid Id { get; set; }
    public DateTimeOffset? DataModificacao { get; set; }
    public DateTimeOffset DataCriacao { get; set; }

    [JsonIgnore]
    public bool IsDeleted { get; set; }
}
