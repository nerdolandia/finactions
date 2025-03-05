using FinActions.Application.Validations.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace FinActions.Application.Validations.Base;

public abstract class BaseValidator : IBaseValidator
{
    private bool _isValid { get; set; }
    protected private object _validationObject { get; set; }
    protected private object _validationEntity { get; set; }
    protected abstract string ModelValidationTitle { get; init; }
    protected abstract string EntityValidationTitle { get; init; }
    private ModelValidationDto _modelValidationDto = new();
    private EntityValidationDto _entityValidationDto { get; set; }

    public virtual IBaseValidator ModelToValidate(object validationObject)
    {
        _validationObject = validationObject;
        return this;
    }
    
    public virtual IBaseValidator EntityToValidate(object validationEntity)
    {
        _validationEntity = validationEntity;
        return this;
    }

    public IBaseValidator AddValidation<T>(Predicate<T> predicate, string mensagemErro, string campo = "")
    {
        if (!predicate((T)_validationObject))
        {
            _isValid = false;

            if (_modelValidationDto.errors.ContainsKey(campo))
                _modelValidationDto.errors[campo].Append(mensagemErro);
            else
                _modelValidationDto.errors.Add(campo, new string[] { mensagemErro });
        }

        return this;
    }

    public IBaseValidator AddEntityValidation<T>(Predicate<T> predicate, string mensagemErro, string type, int statusCode)
    {
        if (!predicate((T)_validationEntity))
        {
            _isValid = false;
            _entityValidationDto = new EntityValidationDto(EntityValidationTitle, mensagemErro, type, statusCode);
        }

        return this;
    }


    public virtual ProblemDetails ValidateModel(out bool isValid)
    {
        isValid = _isValid;
        return new ValidationProblemDetails
        {
            Title = _modelValidationDto.title,
            Status = StatusCodes.Status400BadRequest,
            Detail = $"Erro(s) de validação(ões) em {_modelValidationDto.errors.Count} itens",
            Errors = _modelValidationDto.errors
        };

    }

    public ProblemDetails ValidateEntity(out bool isValid)
    {
        isValid = _isValid;
        return new ProblemDetails
        {
            Title = _entityValidationDto.title,
            Detail = _entityValidationDto.description,
            Type = _entityValidationDto.type,
            Status = _entityValidationDto.statusCode
        };

    }
}
