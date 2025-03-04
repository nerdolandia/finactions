using System.ComponentModel.DataAnnotations;

namespace FinActions.Domain.Shared.Security;

public enum FinActionsPermissions
{
    [Display(Name = "Consultar Usuários")]
    UsuarioConsultar,
    [Display(Name = "Criar Usuários")]
    UsuarioCriar,
    [Display(Name = "Editar Usuários")]
    UsuarioEditar,

    [Display(Name = "Consultar Categorias")]
    CategoriaConsultar,
    [Display(Name = "Criar Categorias")]
    CategoriaCriar,
    [Display(Name = "Editar Categorias")]
    CategoriaEditar,
    [Display(Name = "Remover Categorias")]
    CategoriaRemover,

    [Display(Name = "Consultar Contas Bancárias")]
    ContaBancariaConsultar,
    [Display(Name = "Criar Contas Bancárias")]
    ContaBancariaCriar,
    [Display(Name = "Editar Contas Bancárias")]
    ContaBancariaEditar,
    [Display(Name = "Remover Contas Bancárias")]
    ContaBancariaRemover
}
