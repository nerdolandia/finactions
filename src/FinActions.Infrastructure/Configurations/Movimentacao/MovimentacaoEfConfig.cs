using FinActions.Domain.Movimentacoes;
using FinActions.Domain.Shared.Movimentacoes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinActions.Infrastructure.Configurations;

public class MovimentacaoEfConfig : IEntityTypeConfiguration<Movimentacao>
{
    public void Configure(EntityTypeBuilder<Movimentacao> builder)
    {
        builder.ToTable("Movimentacoes");

        builder.HasOne(x => x.Categoria)
                .WithMany(x => x.Movimentacoes)
                .HasForeignKey(x => new { x.CategoriaId, x.UserId })
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

        builder.HasOne(x => x.ContaBancaria)
                .WithMany(x => x.Movimentacoes)
                .HasForeignKey(x => new { x.ContaBancariaId, x.UserId })
                .OnDelete(DeleteBehavior.Restrict);

        builder.Property(x => x.DataCriacao)
                .HasDefaultValueSql("NOW()");

        builder.Property(x => x.Descricao)
                .HasMaxLength(MovimentacaoConsts.DescricaoMaxLength)
                .IsRequired();

        builder.Property(x => x.TipoMovimentacao)
                .IsRequired();

        builder.Property(x => x.ValorMovimentado)
                .HasColumnType("money")
                .IsRequired();

        builder.HasOne(x => x.User)
                .WithMany(x => x.Movimentacoes)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

        builder.HasKey(x => new { x.UserId, x.Id });

        builder.Property(x => x.IsDeleted)
                .HasDefaultValue(false);
    }
}
