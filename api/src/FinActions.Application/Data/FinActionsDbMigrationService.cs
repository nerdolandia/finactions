
using FinActions.Application.Data.Seeds;
using FinActions.Domain.Shared.Data.Seeds;
using FinActions.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FinActions.Application.Data;

/// <summary>
/// Esta classe <see cref="FinActionsDbMigrationService"/> é responsável por aplicar as migrations no banco de dados 
/// e, em seguida, chamar o <see cref="IDataSeeder"/> para garantir que os dados essenciais sejam semeados automaticamente.
/// </summary>
/// <remarks>
/// <para>
/// Por que não usar a CLI do EF Core diretamente para aplicar as migrations? Quando você executa as migrations através da CLI 
/// (usando o comando <c>dotnet ef database update</c>), o EF Core apenas atualiza a estrutura do banco de dados, mas não se preocupa 
/// em inserir dados fixos necessários, como valores de <c>enum</c> ou tabelas de referência essenciais. Isso pode deixar seu banco de dados 
/// atualizado, mas sem os dados necessários para o funcionamento correto da aplicação.
/// </para>
/// <para>
/// O <see cref="FinActionsDbMigrationService"/> aplica as migrations e em seguida chama o <see cref="IDataSeeder"/> para semear 
/// os dados essenciais, garantindo que o banco de dados esteja não só atualizado, mas também corretamente populado.
/// </para>
/// </remarks>
public class FinActionsDbMigrationService : IFinActionsDbMigrationService
{
    private readonly ILogger<FinActionsDbMigrationService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IDataSeeder _dataSeeder;

    public FinActionsDbMigrationService(
        ILogger<FinActionsDbMigrationService> logger,
        IServiceProvider serviceProvider,
        IDataSeeder dataSeeder)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _dataSeeder = dataSeeder;
    }

    public async Task MigrateAsync(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }

        using var scope = _serviceProvider.CreateScope();

        using var appContext = scope.ServiceProvider.GetRequiredService<FinActionsDbContext>();

        await MigrateOnlyIfPendingMigrations(appContext, nameof(FinActionsDbContext), cancellationToken);

        await _dataSeeder.SeedAsync(scope, nameof(FinActionsDbContext), cancellationToken);
    }

    private async Task MigrateOnlyIfPendingMigrations(DbContext context, string dbContextName, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }

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
