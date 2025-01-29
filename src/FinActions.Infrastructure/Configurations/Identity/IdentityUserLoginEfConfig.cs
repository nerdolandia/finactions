using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinActions.Infrastructure.Configurations.Identity;

public class IdentityUserLoginEfConfig : IEntityTypeConfiguration<IdentityUserLogin<Guid>>
{
    public void Configure(EntityTypeBuilder<IdentityUserLogin<Guid>> builder)
    {
        builder.ToTable("UsersLogins");

        builder.Property(u => u.LoginProvider)
            .HasMaxLength(64);

        builder.Property(u => u.ProviderKey)
            .HasMaxLength(196);

        builder.Property(u => u.ProviderDisplayName)
            .HasMaxLength(128);
    }
}
