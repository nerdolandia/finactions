using System.ComponentModel.DataAnnotations;

namespace FinActions.Domain.Shared.Security;

public enum FinActionsPermissions
{
    [Display(Name = "Consultar Usuários")]
    UsuarioConsultar,

    [Display(Name = "Criar Usuários")]
    UsuarioCriar,

    [Display(Name = "Editar Usuários")]
    UsuarioEditar
}
