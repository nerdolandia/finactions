namespace FinActions.Application.Validations.Categoria;
public struct CategoriaValidatorConsts
{
    public const string ErroTamanhoNome = "Número de caractéres para o nome da categoria ultrapassa os limites";
    public const string ErroQuantidadeQuery = "Número de categorias escolhidas para filtro está zerada";
    public const string ErroNaoFoiEncontradoCategoria = "Categoria não foi encontrada";
    public const string ErroCategoriaJaExiste = "Não foi possível adicionar a categoria pois a mesma já existe";
}
