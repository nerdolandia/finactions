using System.Text.Json.Serialization;
using AutoMapper.Configuration.Annotations;

namespace FinActions.Application.Categorias.Requests;

public record struct PostCategoriaRequestDto()
{
    public string Nome { get; set; }

    [JsonIgnore]
    public Guid UserId { get; set; }
}
