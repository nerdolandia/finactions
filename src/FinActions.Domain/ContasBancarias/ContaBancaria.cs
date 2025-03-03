using FinActions.Domain.Base;
using FinActions.Domain.Movimentacoes;

namespace FinActions.Domain.ContasBancarias;
public sealed class ContaBancaria : UserBaseEntity
{
    public string Nome { get; set; }
    public TipoContaEnum TipoConta { get; set; }
    public decimal Saldo { get; set; }
    public ICollection<Movimentacao> Movimentacoes { get; set; } = [];
}
