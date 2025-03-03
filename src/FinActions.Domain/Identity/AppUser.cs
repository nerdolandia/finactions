using FinActions.Domain.Categorias;
using FinActions.Domain.ContasBancarias;
using FinActions.Domain.Movimentacoes;
using Microsoft.AspNetCore.Identity;

namespace FinActions.Domain.Identity;

public sealed class AppUser : IdentityUser<Guid>
{
    public ICollection<ContaBancaria> ContasBancarias { get; set; } = [];
    public ICollection<Movimentacao> Movimentacoes { get; set; } = [];
    public ICollection<Categoria> Categorias { get; set; } = [];
}
