
using FinActions.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FinActions.Application.Data;

public class FinActionsDbMigrationService : IFinActionsDbMigrationService
{
    private readonly ILogger<FinActionsDbMigrationService> _logger;
    private readonly FinActionsDbContext _appContext;

    public FinActionsDbMigrationService(
        ILogger<FinActionsDbMigrationService> logger,
        FinActionsDbContext appContext)
    {
        _logger = logger;
        _appContext = appContext;
    }

    public async Task MigrateAsync(CancellationToken cancellationToken)
    {
        await MigrateOnlyIfPendingMigrations(_appContext, nameof(FinActionsDbContext), cancellationToken);
    }

    private async Task MigrateOnlyIfPendingMigrations(DbContext context, string dbContextName, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Obtendo migrations pendentes para aplicar no {dbContextName}", dbContextName);
        var pendingMigrations = await context.Database.GetPendingMigrationsAsync(cancellationToken);
        if (!pendingMigrations.Any())
        {
            _logger.LogWarning("Nenhuma migration está pendente para aplicar no {dbContextName}", dbContextName);
            return;
        }

        _logger.LogInformation("Aplicando migrations pendentes no {dbContextName}", dbContextName);
        await context.Database.MigrateAsync(cancellationToken);
        _logger.LogInformation("Migrations pendentes aplicadas com sucesso no {dbContextName}!", dbContextName);
    }

}
