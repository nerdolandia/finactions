using FinActions.Domain.Movimentacoes;

namespace FinActions.Domain.Categorias;
public sealed class Categoria : IBaseEntity
{
    public DateTimeOffset DataCriacao { get; set; }
    public DateTimeOffset? DataModificacao { get; set; }
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public ICollection<Movimentacao> Movimentacoes { get; set; } = [];
}
