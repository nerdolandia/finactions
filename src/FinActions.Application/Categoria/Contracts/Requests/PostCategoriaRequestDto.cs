namespace FinActions.Application.Categoria.Requests;

public record struct PostCategoriaRequestDto(string descricao, Guid UserId);
