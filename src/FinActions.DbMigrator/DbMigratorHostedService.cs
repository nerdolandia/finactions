using FinActions.Application.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FinActions.DbMigrator;
public sealed class DbMigratorHostedService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly ILogger<DbMigratorHostedService> _logger;

    public DbMigratorHostedService(
        IServiceProvider serviceProvider,
        ILogger<DbMigratorHostedService> logger,
        IHostApplicationLifetime hostApplicationLifetime)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _hostApplicationLifetime = hostApplicationLifetime;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();

        _logger.LogInformation("Iniciando DbMigrator");

        try
        {
            var migrationService = scope.ServiceProvider.GetRequiredService<IFinActionsDbMigrationService>();
            await migrationService.MigrateAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao rodar migrations!");
        }
        finally
        {
            _logger.LogInformation("Finalizando DbMigrator");
            _hostApplicationLifetime.StopApplication();
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
