

using FinActions.Domain.Shared.Data.Seeds;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FinActions.Application.Data.Seeds;

/// <summary>
/// Esta classe é como o maestro da orquestra de dados constantes. 
/// Ela é responsável por reunir todas as implementações de IDataSeedContributor e garantir que os dados certos sejam inseridos no banco de dados. 
/// Ela não inventa dados, mas orquestra quem vai ser responsável por inseri-los.
/// Afinal, nada melhor que alguém que organiza a bagunça para você, certo?
/// </summary>
public sealed class DataSeeder : IDataSeeder
{
    private readonly ILogger<DataSeeder> _logger;

    public DataSeeder(ILogger<DataSeeder> logger)
    {
        _logger = logger;
    }

    public async Task SeedAsync(
        IServiceScope scope,
        string dbContextName,
        CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }

        _logger.LogInformation("Obtendo os data seeds contributors para aplicar no {dbContextName}", dbContextName);
        var dataSeedContributors = scope.ServiceProvider.GetServices<IDataSeedContributor>();

        foreach (var dataSeedContributor in dataSeedContributors)
        {
            await dataSeedContributor.SeedDataAsync();
        }

        _logger.LogInformation("Data seeds contributors aplicados no {dbContextName} com sucesso!", dbContextName);
    }
}
