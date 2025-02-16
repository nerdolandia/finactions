using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinActions.Infrastructure.Configurations.Identity;

public class IdentityUserClaimEfConfig : IEntityTypeConfiguration<IdentityUserClaim<Guid>>
{
    public void Configure(EntityTypeBuilder<IdentityUserClaim<Guid>> builder)
    {
        builder.ToTable("UsersClaims");

        builder.Property(u => u.ClaimType)
            .HasMaxLength(256);

        builder.Property(u => u.ClaimValue)
            .HasMaxLength(1024);
    }

}
