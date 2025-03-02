using FinActions.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinActions.Infrastructure.Configurations;
public class CategoriaEfConfig : IEntityTypeConfiguration<Categoria>
{
    public void Configure(EntityTypeBuilder<Categoria> builder)
    {
        builder.ToTable("Categorias");

        builder.Property(x => x.DataCriacao)
                .HasDefaultValue(DateTimeOffset.Now);

        builder.Property(x => x.DataModificacao)
                .ValueGeneratedOnUpdate()
                .HasDefaultValue(DateTimeOffset.Now);
    }
}
