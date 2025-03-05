using FinActions.Domain.Shared.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using FinActions.Application.Validations.Base;
namespace FinActions.Application.Validations.Categoria;

public interface ICategoriaValidator : IBaseValidator, ITransientDependency
{
}
