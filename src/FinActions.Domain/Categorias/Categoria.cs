using FinActions.Domain.Base;
using FinActions.Domain.Movimentacoes;

namespace FinActions.Domain.Categorias;
public sealed class Categoria : UserBaseEntity
{
    public string Nome { get; set; }
    public ICollection<Movimentacao> Movimentacoes { get; set; } = [];
}
