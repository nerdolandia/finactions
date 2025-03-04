using System.Security.Claims;

namespace FinActions.Domain.Shared.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal claimsPrincipal)
        => Guid.Parse(claimsPrincipal.Claims.FirstOrDefault(x => x?.Type == ClaimTypes.NameIdentifier)?.Value);
}
