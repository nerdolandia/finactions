using FinActions.Domain.Shared.Data.Seeds;
using FinActions.Domain.Shared.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace FinActions.Domain.Identity;

public class AppUserSeedContributor : IDataSeedContributor
{
    private const string AdminEmailDefaultValue = AppUserConsts.AdminEmailDefaultValue;
    private const string AdminUserNameDefaultValue = AppUserConsts.AdminUserNameDefaultValue;
    private const string AdminPasswordDefaultValue = AppUserConsts.AdminPasswordDefaultValue;

    private readonly UserManager<AppUser> _userManager;
    private readonly ILogger<AppUserSeedContributor> _logger;

    public AppUserSeedContributor(
        UserManager<AppUser> userManager,
        ILogger<AppUserSeedContributor> logger)
    {
        _userManager = userManager;
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
