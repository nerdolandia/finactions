using FinActions.Domain.Shared.Data.Seeds;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace FinActions.Domain.Identity;

public class AppUserSeedContributor : IDataSeedContributor
{
    private const string AdminEmailDefaultValue = "admin@finactions.com.br";
    private const string AdminUserNameDefaultValue = "admin";
    private const string AdminPasswordDefaultValue = "1q2w3E*";

    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly ILogger<AppUserSeedContributor> _logger;

    public AppUserSeedContributor(
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole<Guid>> roleManager,
        ILogger<AppUserSeedContributor> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
    }


    public async Task SeedDataAsync()
    {
        _logger.LogInformation("Criando usuário admin padrão");
        if (await _userManager.FindByNameAsync(AdminUserNameDefaultValue) is not null)
        {
            _logger.LogInformation("Usuário já existe na base, ignorando a criação do mesmo");
            return;
        }

        var adminUser = new AppUser()
        {
            Id = Guid.NewGuid(),
            Email = AdminEmailDefaultValue,
            UserName = AdminUserNameDefaultValue
        };

        var result = await _userManager.CreateAsync(adminUser, AdminPasswordDefaultValue);

        if(result.Succeeded)
        {
            _logger.LogInformation("Usuário criado com sucesso!");
        }
    }
}
