using FinActions.Application.Base.Services;
using Microsoft.AspNetCore.Mvc;
using FinActions.Application.Categorias.Requests;
namespace FinActions.Application.Validations.Categoria;

public class CategoriaValidationService : BaseValidationService 
{
    private readonly object _validationObject;

    public CategoriaValidationService(object validationObject, string errorListTitle) : base(validationObject, errorListTitle)
    {
        _validationObject = validationObject;
    }
    
    public override ProblemDetails Validate()
    {
        if (_validationObject is GetCategoriaRequestDto)
        {
            ValidateGetRequest();
        }
        if (_validationObject is PostCategoriaRequestDto)
        {
            ValidatePostRequest();
        }
        return base.Validate();
    }
    private void ValidateGetRequest()
    {
        base.AddValidation<GetCategoriaRequestDto>(x => x.Nome.Length > 150, "Número de caractéres para o nome da categoria ultrapassa os limites")
            .AddValidation<GetCategoriaRequestDto>(x => x.Take == 0, "Número de categorias escolhidas para filtro está zerada");
    }

    private void ValidatePostRequest()
    {
        base.AddValidation<PostCategoriaRequestDto>(x => x.Nome.Length > 150, "Número de caractéres para o nome da categoria ultrapassa os limites");
    }
}
