using FinActions.Application.Validations.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace FinActions.Application.Base.Services;

public abstract class BaseValidationService 
{
    public bool IsValid { get; set; }
    private ValidationModel _validationModel { get; set; }
    private readonly object _validationObject;

    public BaseValidationService(object validationObject, string errorTitle)
    {
        _validationModel.title = errorTitle;
        _validationObject = validationObject;
    }

    public virtual BaseValidationService AddValidation<T>(Predicate<T> predicate, string mensagemErro, string campo = "")
    {
        if (!predicate((T)_validationObject))
        {
            IsValid = false;

            if(_validationModel.errors.ContainsKey(campo))
                _validationModel.errors[campo].Append(mensagemErro);
            else
                _validationModel.errors.Add(campo, new string[] {mensagemErro});
        }

        return this;
    }

    public virtual ProblemDetails Validate()
        =>  new ValidationProblemDetails
        {
            Title = _validationModel.title,
            Status = StatusCodes.Status400BadRequest,
            Detail = $"Erro de validações em {_validationModel.errors.Count} itens",
            Errors = _validationModel.errors
        };
}
