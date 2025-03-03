using FinActions.Application;
using FinActions.DbMigrator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

await Host.CreateDefaultBuilder(args)
        .ConfigureServices((hostContext, services) =>
        {
            services.AddApplication(hostContext.Configuration);
            services.AddHostedService<DbMigratorHostedService>();
        })
        .RunConsoleAsync();
