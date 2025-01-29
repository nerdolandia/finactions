using FinActions.Domain.Shared.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace FinActions.Application.Data.Seeds;

/// <summary>
/// A interface IDataSeeder é como o maestro da orquestra de dados constantes. 
/// Ela é responsável por reunir todas as implementações de IDataSeedContributor e garantir que os dados certos sejam inseridos no banco de dados. 
/// Ela não inventa dados, mas orquestra quem vai ser responsável por inseri-los.
/// Afinal, nada melhor que alguém que organiza a bagunça para você, certo?
/// </summary>
public interface IDataSeeder : ITransientDependency
{
    Task SeedAsync(IServiceScope serviceProvider, string dbContextName, CancellationToken cancellationToken);
}
