using FinActions.Application;
using FinActions.DbMigrator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

Console.WriteLine("Starting FinActions.DbMigrator");
await Host.CreateDefaultBuilder(args)
        .ConfigureServices((hostContext, services) =>
        {
            services.AddSerilog(x => x.ReadFrom.Configuration(hostContext.Configuration))
                .AddApplication(hostContext.Configuration)
                .AddHostedService<DbMigratorHostedService>();
        })
        .RunConsoleAsync();
