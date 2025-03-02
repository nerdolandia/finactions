using FinActions.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinActions.Infrastructure.Configurations;

public class MovimentacaoEfConfig : IEntityTypeConfiguration<Movimentacao>
{
    public void Configure(EntityTypeBuilder<Movimentacao> builder)
    {
        builder.ToTable("Movimentacoes");

        builder.HasOne(x => x.ContaBancaria)
                .WithOne(x => x.Movimentacao)
                .HasForeignKey<Movimentacao>(x => x.CategoriaId);

        builder.HasOne(x => x.ContaBancaria)
                .WithOne(x => x.Movimentacao)
                .HasForeignKey<Movimentacao>(x => x.ContaBancariaId);

        builder.Property(x => x.Descricao)
                .HasMaxLength(300);

        builder.Property(x => x.DataCriacao)
        .HasDefaultValue(DateTimeOffset.Now);

        builder.Property(x => x.DataModificacao)
                .ValueGeneratedOnUpdate()
                .HasDefaultValue(DateTimeOffset.Now);

    }
}
