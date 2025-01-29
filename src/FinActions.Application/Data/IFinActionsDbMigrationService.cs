using FinActions.Domain.Shared.DependencyInjection;

namespace FinActions.Application.Data;

public interface IFinActionsDbMigrationService : ITransientDependency
{
    Task MigrateAsync(CancellationToken cancellationToken);
}
