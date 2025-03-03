using FinActions.Application.Base.Requests;
using FinActions.Application.Base.Responses;
using FinActions.Application.Identity.Contracts.Requests;
using FinActions.Application.Identity.Contracts.Responses;
using FinActions.Domain.Shared.DependencyInjection;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity.Data;

namespace FinActions.Application.Identity.Services;

public interface IIdentityService : IScopedDependency
{
    Task<Results<Ok<AccessTokenResponse>, ProblemHttpResult>> LoginAsync(LoginRequestDto login);
    Task<Results<Ok<PagedResultDto<AppUserDto>>, ProblemHttpResult>> GetListAsync(GetAppUserDto login);
    Task<Results<Ok<PermissionsDto>, ProblemHttpResult>> GetPermissionsAsync(Guid userId);
    Task<Results<Ok<AccessTokenResponse>, UnauthorizedHttpResult, ProblemHttpResult>> RefreshAsync(RefreshRequest refreshRequest);
}
