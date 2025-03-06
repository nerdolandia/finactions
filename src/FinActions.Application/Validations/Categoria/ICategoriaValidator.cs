using FinActions.Domain.Shared.DependencyInjection;
using FinActions.Application.Validations.Base;
namespace FinActions.Application.Validations.Categoria;

public interface ICategoriaValidator : IBaseValidator, ITransientDependency
{
    ICategoriaValidator ModelObject(object validationObject);
    ICategoriaValidator DbEntityObject(object validationDbEntity);
    ICategoriaValidator ApplyInsertRules();
    ICategoriaValidator ApplyGetByIdRules();
    ICategoriaValidator ApplyUpdateRules();
    ICategoriaValidator ApplyDeleteRules();
}
