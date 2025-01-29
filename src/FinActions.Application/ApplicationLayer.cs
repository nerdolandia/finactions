using System.Reflection;
using FinActions.Domain.Identity;
using FinActions.Domain.Shared.DependencyInjection;
using FinActions.Infrastructure.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinActions.Application;

public static class ApplicationLayer
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        RegisterApplicationAndDomainDependencies(services);

        services.AddDbContext<FinActionsDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("Default"));
        });

        services.AddIdentityCore<AppUser>()
            .AddRoles<IdentityRole<Guid>>()
            .AddEntityFrameworkStores<FinActionsDbContext>();

        return services;
    }

    private static void RegisterApplicationAndDomainDependencies(IServiceCollection services)
    {
        var singleton = typeof(ISingletonDependency);
        var scoped = typeof(IScopedDependency);
        var transient = typeof(ITransientDependency);

        var dependencies = typeof(ApplicationLayer).Assembly.GetTypes().Where(e => e.IsClass && !e.IsAbstract);

        foreach (var dependency in dependencies)
        {
            var contracts = dependency.GetInterfaces();
            foreach (var contract in contracts)
            {
                if (singleton.IsAssignableFrom(dependency) && singleton.IsAssignableFrom(contract))
                {
                    services.AddSingleton(contract, dependency);
                }
                else if (scoped.IsAssignableFrom(dependency) && scoped.IsAssignableFrom(contract))
                {
                    services.AddScoped(contract, dependency);
                }
                else if (transient.IsAssignableFrom(dependency) && transient.IsAssignableFrom(contract))
                {
                    services.AddTransient(contract, dependency);
                }
            }
        }
    }
}
