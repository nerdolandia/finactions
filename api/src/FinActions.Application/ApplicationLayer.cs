using System.Reflection;
using FinActions.Application.Identity.Services;
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

        services.AddIdentity<AppUser, IdentityRole<Guid>>()
            .AddRoles<IdentityRole<Guid>>()
            .AddEntityFrameworkStores<FinActionsDbContext>()
            .AddDefaultTokenProviders();

        services.AddTransient<IEmailSender<AppUser>, DefaultMessageEmailSender>();

        services.AddHttpContextAccessor();

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
            var contracts = dependency
                                .GetInterfaces()
                                .Where(x => x != singleton
                                        && x != scoped
                                        && x != transient);

            if (!contracts.Any())
            {
                if (singleton.IsAssignableFrom(dependency))
                {
                    services.AddSingleton(dependency);
                }
                else if (scoped.IsAssignableFrom(dependency))
                {
                    services.AddScoped(dependency);
                }
                else if (transient.IsAssignableFrom(dependency))
                {
                    services.AddTransient(dependency);
                }
                continue;
            }

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
