using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinActions.Infrastructure.Configurations.Identity;

public class IdentityUserTokenEfConfig : IEntityTypeConfiguration<IdentityUserToken<Guid>>
{
    public void Configure(EntityTypeBuilder<IdentityUserToken<Guid>> builder)
    {
        builder.ToTable("UsersTokens");

        builder.Property(u => u.Name)
            .HasMaxLength(128);

        builder.Property(u => u.LoginProvider)
            .HasMaxLength(64);
    }

}
