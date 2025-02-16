namespace FinActions.Api.Host.Extensions;

public static class CorsExtensions
{
    public static IApplicationBuilder UseCustomCors(this WebApplication applicationBuilder)
    {
        applicationBuilder.UseCors(policyBuilder =>
        {
            policyBuilder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
                .SetIsOriginAllowed(x => x.EndsWith("localhost"));
        });

        return applicationBuilder;
    }
}
