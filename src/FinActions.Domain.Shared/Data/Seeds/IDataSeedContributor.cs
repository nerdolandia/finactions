using FinActions.Domain.Shared.DependencyInjection;

namespace FinActions.Domain.Shared.Data.Seeds;
/// <summary>
/// A interface IDataSeedContributor é responsável por preparar e inserir dados constantes no banco de dados, como tabelas que herdam valores de enums ou qualquer outro tipo de dado fixo e necessário para o funcionamento do sistema.
/// Ela foi criada para garantir que você não tenha que se preocupar em alimentar sua base de dados manualmente (porque, convenhamos, ninguém quer perder tempo com isso).
/// </summary>
public interface IDataSeedContributor : ITransientDependency
{
    Task SeedDataAsync();
}
