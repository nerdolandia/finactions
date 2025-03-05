using Microsoft.AspNetCore.Mvc;
namespace FinActions.Application.Validations.Base;

public interface IBaseValidator
{
    IBaseValidator ModelToValidate(object validationObject);
    IBaseValidator EntityToValidate(object validationEntity);
    IBaseValidator AddValidation<T>(Predicate<T> predicate, string mensagemErro, string campo);
    IBaseValidator AddEntityValidation<T>(Predicate<T> predicate, string mensagemErro, string type, int statusCode);
    ProblemDetails ValidateModel(out bool isValid);
    ProblemDetails ValidateEntity(out bool isValid);
}
