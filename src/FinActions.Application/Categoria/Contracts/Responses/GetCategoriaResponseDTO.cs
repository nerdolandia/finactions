namespace FinActions.Application.Categoria.Responses;
public record struct CategoriaResponseDto(
        Guid Id,
        string Nome,
        DateTimeOffset dataCriacao
        );
