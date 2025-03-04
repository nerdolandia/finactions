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
        if (!await _roleManager.RoleExistsAsync(adminRole.Name)
            && !(await _roleManager.CreateAsync(adminRole)).Succeeded)
        {
            var erroCriarRole = "Algo de errado, não está certo com a role!";
            _logger.LogError("{message}", erroCriarRole);
            throw new Exception(erroCriarRole);
        }
        adminRole = await _roleManager.FindByNameAsync(FinActionsRolesConsts.Administrador.GetDisplayName());

        var claims = new List<Claim>();

        foreach (var permission in PermissionsConsts.Permissions)
        {
            claims.Add(new(permission.ToString(), PermissionsConsts.Allow, ClaimValueTypes.String));
        }

        var adminUser = await _userManager.FindByNameAsync(AppUserConsts.AdminUserNameDefaultValue);
        var roleClaims = await _roleManager.GetClaimsAsync(adminRole);
        var userClaims = await _userManager.GetClaimsAsync(adminUser);

        // TODO: Sim, estou recriando os claims a cada execução do DbMigrator, depois melhoramos isso
        foreach (var claim in claims)
        {
            if (!roleClaims.Any(x => x.Type == claim.Type))
            {
                if (!(await _roleManager.AddClaimAsync(adminRole, claim)).Succeeded)
                {
                    var erroCriarRoleClaims = "Algo de errado, não está certo com as role claims!";
                    _logger.LogError("{message}", erroCriarRoleClaims);
                    throw new Exception(erroCriarRoleClaims);
                }
            }

            if (!userClaims.Any(x => x.Type == claim.Type))
            {
                if (!(await _userManager.AddClaimAsync(adminUser, claim)).Succeeded)
                {
                    var erroCriarUserClaims = "Algo de errado, não está certo com as users claims!";
                    _logger.LogError("{message}", erroCriarUserClaims);
                    throw new Exception(erroCriarUserClaims);
                }
            }

        }

        await _userManager.AddToRoleAsync(adminUser, FinActionsRolesConsts.Administrador.GetDisplayName());
    }

}
