using FinActions.Application.Validations.Base;
using Microsoft.AspNetCore.Mvc;
using FinActions.Application.Categorias.Requests;
namespace FinActions.Application.Validations.Categoria;

public class CategoriaValidator : BaseValidator, ICategoriaValidator
{
    private object _validationObject { get; set; }
    private const string ErroTamanhoNome = "Número de caractéres para o nome da categoria ultrapassa os limites";
    private const string ErroQuantidadeQuery = "Número de categorias escolhidas para filtro está zerada";
    private const string TitleValidation = "Erro de validação da request de categorias";

    public override void ValidatorModel(object validationObject, string title = TitleValidation)
    {
        base.ValidatorModel(validationObject, title);
    }

    public override ProblemDetails Validate()
    {
        if (_validationObject is GetCategoriaRequestDto getCategoriaRequest)
        {
            ValidateGetRequest(getCategoriaRequest);
        }
        if (_validationObject is PostCategoriaRequestDto postCategoriaRequest)
        {
            ValidatePostRequest(postCategoriaRequest);
        }
        return base.Validate();
    }

    private void ValidateGetRequest(GetCategoriaRequestDto requestObject)
    {
        base.AddValidation<GetCategoriaRequestDto>(x => x.Nome.Length > 150,
                                                    ErroTamanhoNome,
                                                    nameof(requestObject.Nome))
            .AddValidation<GetCategoriaRequestDto>(x => x.Take == 0,
                                                    ErroQuantidadeQuery,
                                                    nameof(requestObject.Take));
    }

    private void ValidatePostRequest(PostCategoriaRequestDto requestObject)
    {
        base.AddValidation<PostCategoriaRequestDto>(x => x.Nome.Length > 150,
                                                    ErroTamanhoNome,
                                                    nameof(requestObject.Nome));
    }
}
