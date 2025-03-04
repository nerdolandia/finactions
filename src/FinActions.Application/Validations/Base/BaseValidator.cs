using FinActions.Application.Validations.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace FinActions.Application.Validations.Base;

public abstract class BaseValidator 
{
    public bool IsValid { get; set; }
    private ValidationModel _validationModel { get; set; }
    private object _validationObject { get; set; }

    public virtual void ValidatorModel(object validationObject, string title)
    {
        _validationModel.title = title;
        _validationObject = validationObject;
    }

    public virtual BaseValidator AddValidation<T>(Predicate<T> predicate, string mensagemErro, string campo = "")
    {
        if (!predicate((T)_validationObject))
        {
            IsValid = false;

            if (_validationModel.errors.ContainsKey(campo))
                _validationModel.errors[campo].Append(mensagemErro);
            else
                _validationModel.errors.Add(campo, new string[] { mensagemErro });
        }

        return this;
    }

    public virtual ProblemDetails Validate()
        => new ValidationProblemDetails
        {
            Title = _validationModel.title,
            Status = StatusCodes.Status400BadRequest,
            Detail = $"Erro(s) de validação(ões) em {_validationModel.errors.Count} itens",
            Errors = _validationModel.errors
        };
}
