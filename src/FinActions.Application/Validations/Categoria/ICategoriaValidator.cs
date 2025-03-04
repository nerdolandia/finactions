using FinActions.Domain.Shared.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
namespace FinActions.Application.Validations.Categoria;

public interface ICategoriaValidator : ITransientDependency
{
    private const string TitleValidation = "Erro de validação da request de categorias";
    bool IsValid { get; set; }
    void ValidatorModel(object validationObject, string title = TitleValidation);
    ProblemDetails Validate();
}
