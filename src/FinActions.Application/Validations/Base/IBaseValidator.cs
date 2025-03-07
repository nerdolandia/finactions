using Microsoft.AspNetCore.Mvc;
namespace FinActions.Application.Validations.Base;

public interface IBaseValidator
{
    ProblemDetails ValidateModel(out bool isValid);
    ProblemDetails ValidateEntity(out bool isValid);
}
