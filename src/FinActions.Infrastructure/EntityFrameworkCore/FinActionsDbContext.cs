using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FinActions.Infrastructure.EntityFrameworkCore;

public class FinActionsDbContext : IdentityDbContext
{
    public FinActionsDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        // Melhorar performance de índices varchar
        configurationBuilder
            .Properties<string>()
            .AreUnicode(false)
            .HaveMaxLength(300);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }
}
