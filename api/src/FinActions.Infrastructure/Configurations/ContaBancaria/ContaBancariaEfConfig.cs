using FinActions.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinActions.Infrastructure.Configurations;

public class ContaBancariaEfConfig : IEntityTypeConfiguration<ContaBancaria>
{
    public void Configure(EntityTypeBuilder<ContaBancaria> builder)
    {
        builder.ToTable("ContasBancarias");

        builder.Property(x => x.DataCriacao)
                .HasDefaultValue(DateTimeOffset.Now);

        builder.Property(x => x.DataModificacao)
                .ValueGeneratedOnUpdate()
                .HasDefaultValue(DateTimeOffset.Now);

        builder.Property(x => x.Nome)
                .HasMaxLength(150)
                .IsRequired();

        builder.Property(x => x.Saldo)
                .IsRequired();

        builder.Property(x => x.TipoConta)
                .IsRequired();
    }
}
