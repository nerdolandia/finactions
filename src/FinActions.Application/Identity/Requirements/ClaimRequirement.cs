using Microsoft.AspNetCore.Authorization;

namespace FinActions.Application.Identity.Requirements;

public class ClaimRequirement : IAuthorizationRequirement
{
    public string PolicyName { get; }

    public ClaimRequirement(string policyName)
    {
        PolicyName = policyName;
    }
}
