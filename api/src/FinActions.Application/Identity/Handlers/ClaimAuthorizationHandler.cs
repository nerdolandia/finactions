using FinActions.Application.Identity.Requirements;
using FinActions.Domain.Identity;
using FinActions.Domain.Shared.DependencyInjection;
using FinActions.Infrastructure.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FinActions.Application.Identity.Handlers;

public class ClaimAuthorizationHandler : AuthorizationHandler<ClaimRequirement>
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ClaimAuthorizationHandler> _logger;

    public ClaimAuthorizationHandler(
        IServiceProvider serviceProvider,
        ILogger<ClaimAuthorizationHandler> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    // TODO: Seria uma boa fazer um cache dessas permissões para não ficar consultando o banco toda hora
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ClaimRequirement requirement)
    {
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<FinActionsDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

        var user = await userManager.GetUserAsync(context.User);

        if (user is null)
        {
            _logger.LogDebug("Usuário não está autenticado");
            context.Fail();
            return;
        }

        _logger.LogDebug(
            "Verificando se o usuário {userName}, possui a permissão {policyName}",
            user.UserName,
            requirement.PolicyName);


        if (await dbContext.UserClaims
                    .AsNoTracking()
                    .AnyAsync(c => c.UserId == user.Id
                                && c.ClaimType == requirement.PolicyName))
        {
            _logger.LogDebug("Usuário {userName} tem a permissão necessária!", user.UserName);
            context.Succeed(requirement);
            return;
        }

        _logger.LogDebug("Usuário {userName} não possui a a permissão necessária! :C", user.UserName);
        context.Fail();
    }
}
