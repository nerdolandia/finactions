using Serilog;

namespace FinActions.Api.Host.Extensions;

public static class LoggingExtensions
{
    public static IServiceCollection AddSerilogFromAppSettings(
        this IServiceCollection services,
        IConfiguration configuration)
    => services.AddSerilog(x => x.ReadFrom.Configuration(configuration));
}
