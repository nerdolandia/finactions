using FinActions.Application.Identity.Contracts;
using FinActions.Domain.Shared.DependencyInjection;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity.Data;

namespace FinActions.Application.Identity.Services;

public interface IIdentityService : IScopedDependency
{
    Task<Results<Ok<AccessTokenResponse>, ProblemHttpResult>> LoginAsync(LoginRequestDto login);
    Task<Results<Ok<AccessTokenResponse>, UnauthorizedHttpResult, ProblemHttpResult>> Refresh(RefreshRequest refreshRequest);
}
