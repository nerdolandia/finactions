using FinActions.Application.Validations.Base;
using Microsoft.AspNetCore.Mvc;
using FinActions.Application.Categorias.Requests;
namespace FinActions.Application.Validations.Categoria;

public class CategoriaValidator : BaseValidator, ICategoriaValidator
{
    protected override string ModelValidationTitle { get; init; } = "Erro de validação da request de categorias";
    protected override string EntityValidationTitle { get; init; } = "Erro de validação do banco de dados";

    public override IBaseValidator ModelToValidate(object validationObject)
    {
        base.ModelToValidate(validationObject);
        return this;
    }

    public override IBaseValidator EntityToValidate(object validationEntity)
    {
        base.EntityToValidate(validationEntity);
        return this;
    }

    public override ProblemDetails ValidateModel(out bool isValid)
    {
        if (_validationObject is GetCategoriaRequestDto getCategoriaRequest)
            ValidateGetRequest(getCategoriaRequest);
        else if (_validationObject is PostCategoriaRequestDto postCategoriaRequest)
            ValidatePostRequest(postCategoriaRequest);

        return base.ValidateModel(out isValid);
    }

    private void ValidateGetRequest(GetCategoriaRequestDto requestObject)
    {
        base.AddValidation<GetCategoriaRequestDto>(x => x.Nome.Length > 150,
                                                    CategoriaValidatorConsts.ErroCategoriaJaExiste,
                                                    nameof(requestObject.Nome));
        base.AddValidation<GetCategoriaRequestDto>(x => x.Take == 0,
                                                    CategoriaValidatorConsts.ErroQuantidadeQuery,
                                                    nameof(requestObject.Take));
    }


    private void ValidatePostRequest(PostCategoriaRequestDto requestObject)
    {
        base.AddValidation<PostCategoriaRequestDto>(x => x.Nome.Length > 150,
                                                    CategoriaValidatorConsts.ErroTamanhoNome,
                                                    nameof(requestObject.Nome));

    }
}
