using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FinActions.Application.Base;
using FinActions.Application.Identity.Contracts;
using FinActions.Domain.Identity;
using FinActions.Domain.Shared.Security;
using FinActions.Infrastructure.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FinActions.Application.Identity.Services;

public class IdentityService : IIdentityService
{
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly JwtOptions _jwtOptions;
    private readonly FinActionsDbContext _context;

    public IdentityService(
        UserManager<AppUser> userManager,
        IOptions<JwtOptions> jwtOptions,
        SignInManager<AppUser> signInManager,
        RoleManager<IdentityRole<Guid>> roleManager,
        FinActionsDbContext context)
    {
        _userManager = userManager;
        _jwtOptions = jwtOptions.Value;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _context = context;
    }


    public async Task<Results<Ok<AccessTokenResponse>, ProblemHttpResult>> LoginAsync(LoginRequestDto login)
    {
        var result = await _signInManager.PasswordSignInAsync(login.UserName, login.Password, false, lockoutOnFailure: true);

        if (result.Succeeded)
        {
            var response = await GerarAccessTokenResponse(login.UserName);

            return TypedResults.Ok(response);
        }

        return TypedResults.Problem(result.ToString(), statusCode: StatusCodes.Status401Unauthorized);
    }

    public async Task<Results<Ok<AccessTokenResponse>, UnauthorizedHttpResult, ProblemHttpResult>>
        RefreshAsync(RefreshRequest refreshRequest)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = _jwtOptions.SigningCredentials.Key,
            ValidateLifetime = false
        };

        var principal = new JwtSecurityTokenHandler()
                                .ValidateToken(refreshRequest.RefreshToken,
                                                tokenValidationParameters,
                                                out var securityToken);

        if (securityToken is not JwtSecurityToken || principal is null)
        {
            return TypedResults.Problem("Invalid access token/refresh token", statusCode: StatusCodes.Status401Unauthorized);
        }

        var user = await _userManager.FindByNameAsync(principal.Identity.Name);

        if (user is null)
        {
            return TypedResults.Problem("Invalid access token/refresh token", statusCode: StatusCodes.Status401Unauthorized);
        }

        var accessTokenResponse = await GerarAccessTokenResponse(user.UserName);

        return TypedResults.Ok(accessTokenResponse);
    }

    private async Task<AccessTokenResponse> GerarAccessTokenResponse(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);
        var accessTokenClaims = await ObterClaims(user, adicionarClaimsUsuario: false);
        var refreshTokenClaims = await ObterClaims(user, adicionarClaimsUsuario: false);

        var dataExpiracaoAccessToken = DateTime.Now.AddSeconds(_jwtOptions.AccessTokenExpiration);
        var dataExpiracaoRefreshToken = DateTime.Now.AddSeconds(_jwtOptions.RefreshTokenExpiration);

        var accessToken = GerarToken(accessTokenClaims, dataExpiracaoAccessToken);
        var refreshToken = GerarToken(refreshTokenClaims, dataExpiracaoRefreshToken);

        var value = new AccessTokenResponse()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresIn = dataExpiracaoAccessToken.Ticks
        };
        return value;
    }

    private async Task<IList<Claim>> ObterClaims(AppUser user, bool adicionarClaimsUsuario)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Typ, JwtBearerDefaults.AuthenticationScheme),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.AuthTime, DateTimeOffset.Now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer),
            new(JwtRegisteredClaimNames.Nbf, DateTime.Now.ToString()),
            new(JwtRegisteredClaimNames.Iat, DateTimeOffset.Now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer),
            new(JwtRegisteredClaimNames.PreferredUsername, user.UserName),
            new(JwtRegisteredClaimNames.Name, user.NormalizedUserName)
        };

        if (adicionarClaimsUsuario)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);

            var rolesNames = await _userManager.GetRolesAsync(user);
            foreach (var roleName in rolesNames)
            {
                claims.Add(new("role", roleName));
                var role = await _roleManager.FindByNameAsync(roleName);

                foreach (var roleClaim in await _roleManager.GetClaimsAsync(role))
                {
                    claims.Add(roleClaim);
                }
            }

            claims.AddRange(userClaims.DistinctBy(x => x.Type));
        }

        return claims;
    }

    private string GerarToken(IEnumerable<Claim> claims, DateTime dataExpiracao)
    {
        var jwt = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            notBefore: DateTime.Now,
            expires: dataExpiracao,
            signingCredentials: _jwtOptions.SigningCredentials);

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    public async Task<Results<Ok<PagedResultDto<AppUserDto>>, ProblemHttpResult>> GetListAsync(GetAppUserDto request)
    {
        var query = _context.Users.AsNoTracking();
        if (!string.IsNullOrEmpty(request.Email))
        {
            query = query.Where(x => x.Email.ToLower().Contains(request.Email.ToLower()));
        }

        if (!string.IsNullOrEmpty(request.UserName))
        {
            query = query.Where(x => x.UserName.ToLower().Contains(request.UserName.ToLower()));
        }

        query = query.OrderBy(x => x.UserName);

        var totalCount = await query.CountAsync();
        var entities = await query.Skip(request.SkipCount)
                               .Take(request.MaxResultCount > 100 ? 100 : request.MaxResultCount)
                               .ToListAsync();

        var dtos = entities.Select(x => new AppUserDto(x.UserName, x.Email)).ToList();

        var pagedResult = new PagedResultDto<AppUserDto>()
        {
            TotalCount = totalCount,
            Items = dtos
        };

        return TypedResults.Ok(pagedResult);
    }

    public async Task<Results<Ok<PermissionsDto>, ProblemHttpResult>> GetPermissionsAsync(Guid userId)
    {
        var claims = await _context.UserClaims.AsNoTracking()
                            .Where(x => x.UserId == userId)
                            .Select(x => x.ClaimType)
                            .ToHashSetAsync();
        
        return TypedResults.Ok(new PermissionsDto()
        {
            Permissions = claims
        });
    }
}
