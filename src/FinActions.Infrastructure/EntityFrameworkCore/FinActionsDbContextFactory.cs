using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace FinActions.Infrastructure.EntityFrameworkCore;
public class FinActionsDbContextFactory : IDesignTimeDbContextFactory<FinActionsDbContext>
{
    public FinActionsDbContext CreateDbContext(string[] args)
    {
        string connectionString = GetConnectionString();

        var builder = new DbContextOptionsBuilder<FinActionsDbContext>()
                            .UseNpgsql(connectionString);

        return new FinActionsDbContext(builder.Options);
    }

    private static string GetConnectionString()
        => new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../FinActions.DbMigrator/"))
                .AddJsonFile("appsettings.json", optional: false)
                .Build()
                .GetConnectionString("Default");
}
