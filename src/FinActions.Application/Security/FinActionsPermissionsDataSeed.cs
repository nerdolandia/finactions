using FinActions.Domain.Shared.Extensions;
using FinActions.Domain.Identity;
using FinActions.Domain.Shared.Data.Seeds;
using FinActions.Domain.Shared.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using FinActions.Domain.Shared.Identity;

namespace FinActions.Application.Security;

public class FinActionsPermissionsDataSeed : IDataSeedContributor
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ILogger<FinActionsPermissionsDataSeed> _logger;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;

    public FinActionsPermissionsDataSeed(
        ILogger<FinActionsPermissionsDataSeed> logger,
        RoleManager<IdentityRole<Guid>> roleManager,
        UserManager<AppUser> userManager)
    {
        _logger = logger;
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public async Task SeedDataAsync()
    {
        var adminRole = new IdentityRole<Guid>(FinActionsRolesConsts.Administrador.GetDisplayName());
        if (!(await _roleManager.CreateAsync(adminRole)).Succeeded)
        {
            var erroCriarRole = "Algo de errado, não está certo com a role!";
            _logger.LogError("{message}", erroCriarRole);
            throw new Exception(erroCriarRole);
        }

        var claims = new Claim[]
        {
            new Claim(
                FinActionsPermissions.UsuarioConsultar.ToString(),
                PermissionsConsts.Allow,
                ClaimValueTypes.String),
            new Claim(
                FinActionsPermissions.UsuarioCriar.ToString(),
                PermissionsConsts.Allow,
                ClaimValueTypes.String),
            new Claim(
                FinActionsPermissions.UsuarioEditar.ToString(),
                PermissionsConsts.Allow,
                ClaimValueTypes.String)
        };

        foreach (var claim in claims)
        {
            if (!(await _roleManager.AddClaimAsync(adminRole, claim)).Succeeded)
            {
                var erroCriarRoleClaims = "Algo de errado, não está certo com as role claims!";
                _logger.LogError("{message}", erroCriarRoleClaims);
                throw new Exception(erroCriarRoleClaims);
            }
        }

        var adminUser = await _userManager.FindByNameAsync(AppUserConsts.AdminUserNameDefaultValue);
        await _userManager.AddToRoleAsync(adminUser, FinActionsRolesConsts.Administrador.GetDisplayName());
    }

}
