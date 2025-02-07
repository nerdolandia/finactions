using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FinActions.Application.Identity.Contracts;
using FinActions.Domain.Identity;
using FinActions.Domain.Shared.Security;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FinActions.Application.Identity.Services;

public class IdentityService : IIdentityService
{
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly JwtOptions _jwtOptions;

    public IdentityService(
        UserManager<AppUser> userManager,
        IOptions<JwtOptions> jwtOptions,
        SignInManager<AppUser> signInManager)
    {
        _userManager = userManager;
        _jwtOptions = jwtOptions.Value;
        _signInManager = signInManager;
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
        Refresh(RefreshRequest refreshRequest)
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
        var accessTokenClaims = await ObterClaims(user, adicionarClaimsUsuario: true);
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
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Nbf, DateTime.Now.ToString()),
            new(JwtRegisteredClaimNames.Iat, DateTimeOffset.Now.ToUnixTimeSeconds().ToString())
        };

        if (adicionarClaimsUsuario)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            claims.AddRange(userClaims);

            foreach (var role in roles)
            {
                claims.Add(new("role", role));
            }
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
}
