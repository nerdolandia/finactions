using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinActions.Infrastructure.Configurations.Identity;

public class IdentityRoleClaimEfConfig : IEntityTypeConfiguration<IdentityRoleClaim<Guid>>
{
    public void Configure(EntityTypeBuilder<IdentityRoleClaim<Guid>> builder)
    {
        builder.ToTable("RolesClaims");

        builder.Property(u => u.ClaimType)
            .HasMaxLength(256);

        builder.Property(u => u.ClaimValue)
            .HasMaxLength(1024);
    }

}
