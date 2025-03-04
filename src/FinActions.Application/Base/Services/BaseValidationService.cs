using FinActions.Application.Validations.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using FinActions.Domain.Shared.DependencyInjection;

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

    public virtual BaseValidationService AddValidation<T>(Predicate<T> predicate, string mensagemErro)
    {
        if (!predicate((T)_validationObject))
        {
            IsValid = false;
            _validationModel.errors.Add(mensagemErro);
        }

        return this;
    }

    public virtual ProblemDetails Validate()
        =>  new ProblemDetails
        {
            Title = _validationModel.title,
            Status = StatusCodes.Status400BadRequest,
            Detail = _validationModel.errors.Aggregate("", (acc, err) => $"{acc}\n{err}")
        };
}
