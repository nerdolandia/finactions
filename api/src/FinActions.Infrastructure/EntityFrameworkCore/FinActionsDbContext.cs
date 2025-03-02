using FinActions.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FinActions.Infrastructure.EntityFrameworkCore;

public class FinActionsDbContext : IdentityDbContext<AppUser,
                                        IdentityRole<Guid>,
                                        Guid,
                                        IdentityUserClaim<Guid>,
                                        IdentityUserRole<Guid>,
                                        IdentityUserLogin<Guid>,
                                        IdentityRoleClaim<Guid>,
                                        IdentityUserToken<Guid>>
{
    public FinActionsDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        // Melhorar performance de índices varchar
        configurationBuilder
            .Properties<string>()
            .AreUnicode(false);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
#if DEBUG
        optionsBuilder.EnableDetailedErrors();
        optionsBuilder.EnableSensitiveDataLogging();
#endif

        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureFinActions();
    }
}
