using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using FinActions.Application.Base.Requests;
using FinActions.Application.Base.Responses;
using FinActions.Application.Identity.Contracts.Requests;
using FinActions.Application.Identity.Contracts.Responses;
using FinActions.Application.Identity.Services;
using FinActions.Domain.Identity;
using FinActions.Domain.Shared.Security;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;

namespace FinActions.Api.Host.Controllers.Identity;

[ApiController]
[Area("identity")]
[Authorize]
[Route("api/identity")]
public class IdentityController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IEmailSender<AppUser> _emailSender;
    private readonly LinkGenerator _linkGenerator;
    private static readonly EmailAddressAttribute _emailAddressAttribute = new();
    private readonly IServiceProvider _sp;
    private readonly IIdentityService _identityService;

    public IdentityController(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        IEmailSender<AppUser> emailSender,
        LinkGenerator linkGenerator,
        IServiceProvider sp,
        IIdentityService identityService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailSender = emailSender;
        _linkGenerator = linkGenerator;
        _sp = sp;
        _identityService = identityService;
    }

    [HttpPost("register")]
    [Authorize(nameof(FinActionsPermissions.UsuarioCriar))]
    public async Task<Results<Ok, ValidationProblem>> Register([FromBody] RegisterRequest registration)
    {
        if (!_userManager.SupportsUserEmail)
        {
            throw new NotSupportedException($"{nameof(IdentityController)} requires a user store with email support.");
        }

        var userStore = _sp.GetRequiredService<IUserStore<AppUser>>();
        var emailStore = (IUserEmailStore<AppUser>)userStore;
        var email = registration.Email;

        if (string.IsNullOrEmpty(email) || !_emailAddressAttribute.IsValid(email))
        {
            return CreateValidationProblem(IdentityResult.Failed(_userManager.ErrorDescriber.InvalidEmail(email)));
        }

        var user = new AppUser();
        await userStore.SetUserNameAsync(user, email, CancellationToken.None);
        await emailStore.SetEmailAsync(user, email, CancellationToken.None);
        var result = await _userManager.CreateAsync(user, registration.Password);

        if (!result.Succeeded)
        {
            return CreateValidationProblem(result);
        }

        await SendConfirmationEmailAsync(user, HttpContext, email);
        return TypedResults.Ok();
    }

    [HttpGet("user")]
    [Authorize(nameof(FinActionsPermissions.UsuarioConsultar))]
    public async Task<Results<Ok<PagedResultDto<AppUserDto>>, ProblemHttpResult>>
        GetListAsync([FromQuery] GetAppUserDto login)
        => await _identityService.GetListAsync(login);

    [HttpGet("user/{id:guid}/permissions")]
    [Authorize(nameof(FinActionsPermissions.UsuarioConsultar))]
    public async Task<Results<Ok<PermissionsDto>, ProblemHttpResult>> GetPermissionsAsync([FromRoute][Required] Guid id)
        => await _identityService.GetPermissionsAsync(id);

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<Results<Ok<AccessTokenResponse>, ProblemHttpResult>>
        LoginAsync([FromBody] LoginRequestDto login)
        => await _identityService.LoginAsync(login);

    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<Results<Ok<AccessTokenResponse>, UnauthorizedHttpResult, ProblemHttpResult>>
        Refresh([FromBody] RefreshRequest refreshRequest)
        => await _identityService.RefreshAsync(refreshRequest);

    [NonAction]
    [HttpPost("resendConfirmationEmail")]
    public async Task<Ok> ResendConfirmationEmail([FromBody] ResendConfirmationEmailRequest resendRequest)
    {
        if (await _userManager.FindByEmailAsync(resendRequest.Email) is not { } user)
        {
            return TypedResults.Ok();
        }

        await SendConfirmationEmailAsync(user, HttpContext, resendRequest.Email);
        return TypedResults.Ok();
    }

    [NonAction]
    [HttpPost("forgotPassword")]
    public async Task<Results<Ok, ValidationProblem>> ForgotPassword([FromBody] ForgotPasswordRequest resetRequest)
    {
        var user = await _userManager.FindByEmailAsync(resetRequest.Email);

        if (user is not null && await _userManager.IsEmailConfirmedAsync(user))
        {
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            await _emailSender.SendPasswordResetCodeAsync(user, resetRequest.Email, HtmlEncoder.Default.Encode(code));
        }

        // Don't reveal that the user does not exist or is not confirmed, so don't return a 200 if we would have
        // returned a 400 for an invalid code given a valid user email.
        return TypedResults.Ok();
    }

    [NonAction]
    [HttpPost("resetPassword")]
    public async Task<Results<Ok, ValidationProblem>> ResetPassword([FromBody] ResetPasswordRequest resetRequest)
    {
        var user = await _userManager.FindByEmailAsync(resetRequest.Email);

        if (user is null || !await _userManager.IsEmailConfirmedAsync(user))
        {
            // Don't reveal that the user does not exist or is not confirmed, so don't return a 200 if we would have
            // returned a 400 for an invalid code given a valid user email.
            return CreateValidationProblem(IdentityResult.Failed(_userManager.ErrorDescriber.InvalidToken()));
        }

        IdentityResult result;
        try
        {
            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(resetRequest.ResetCode));
            result = await _userManager.ResetPasswordAsync(user, code, resetRequest.NewPassword);
        }
        catch (FormatException)
        {
            result = IdentityResult.Failed(_userManager.ErrorDescriber.InvalidToken());
        }

        if (!result.Succeeded)
        {
            return CreateValidationProblem(result);
        }

        return TypedResults.Ok();
    }

    [NonAction]
    [HttpGet("confirmEmail")]
    public async Task<Results<ContentHttpResult, UnauthorizedHttpResult>> ConfirmEmail(
            [FromQuery] string userId,
            [FromQuery] string code,
            [FromQuery] string changedEmail)
    {
        if (await _userManager.FindByIdAsync(userId) is not { } user)
        {
            // We could respond with a 404 instead of a 401 like Identity UI, but that feels like unnecessary information.
            return TypedResults.Unauthorized();
        }

        try
        {
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        }
        catch (FormatException)
        {
            return TypedResults.Unauthorized();
        }

        IdentityResult result;

        if (string.IsNullOrEmpty(changedEmail))
        {
            result = await _userManager.ConfirmEmailAsync(user, code);
        }
        else
        {
            // As with Identity UI, email and user name are one and the same. So when we update the email,
            // we need to update the user name.
            result = await _userManager.ChangeEmailAsync(user, changedEmail, code);

            if (result.Succeeded)
            {
                result = await _userManager.SetUserNameAsync(user, changedEmail);
            }
        }

        if (!result.Succeeded)
        {
            return TypedResults.Unauthorized();
        }

        return TypedResults.Text("Thank you for confirming your email.");
    }

    [NonAction]
    [HttpPost("manage/2fa")]
    public async Task<Results<Ok<TwoFactorResponse>, ValidationProblem, NotFound>>
        TwoFactorAuthentication([FromBody] TwoFactorRequest tfaRequest)
    {
        var userManager = _signInManager.UserManager;
        if (await userManager.GetUserAsync(HttpContext.User) is not { } user)
        {
            return TypedResults.NotFound();
        }

        if (tfaRequest.Enable == true)
        {
            if (tfaRequest.ResetSharedKey)
            {
                return CreateValidationProblem("CannotResetSharedKeyAndEnable",
                    "Resetting the 2fa shared key must disable 2fa until a 2fa token based on the new shared key is validated.");
            }
            else if (string.IsNullOrEmpty(tfaRequest.TwoFactorCode))
            {
                return CreateValidationProblem("RequiresTwoFactor",
                    "No 2fa token was provided by the request. A valid 2fa token is required to enable 2fa.");
            }
            else if (!await userManager.VerifyTwoFactorTokenAsync(user, userManager.Options.Tokens.AuthenticatorTokenProvider, tfaRequest.TwoFactorCode))
            {
                return CreateValidationProblem("InvalidTwoFactorCode",
                    "The 2fa token provided by the request was invalid. A valid 2fa token is required to enable 2fa.");
            }

            await userManager.SetTwoFactorEnabledAsync(user, true);
        }
        else if (tfaRequest.Enable == false || tfaRequest.ResetSharedKey)
        {
            await userManager.SetTwoFactorEnabledAsync(user, false);
        }

        if (tfaRequest.ResetSharedKey)
        {
            await userManager.ResetAuthenticatorKeyAsync(user);
        }

        string[] recoveryCodes = null;
        if (tfaRequest.ResetRecoveryCodes || tfaRequest.Enable == true && await userManager.CountRecoveryCodesAsync(user) == 0)
        {
            var recoveryCodesEnumerable = await userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
            recoveryCodes = recoveryCodesEnumerable?.ToArray();
        }

        if (tfaRequest.ForgetMachine)
        {
            await _signInManager.ForgetTwoFactorClientAsync();
        }

        var key = await userManager.GetAuthenticatorKeyAsync(user);
        if (string.IsNullOrEmpty(key))
        {
            await userManager.ResetAuthenticatorKeyAsync(user);
            key = await userManager.GetAuthenticatorKeyAsync(user);

            if (string.IsNullOrEmpty(key))
            {
                throw new NotSupportedException("The user manager must produce an authenticator key after reset.");
            }
        }

        return TypedResults.Ok(new TwoFactorResponse
        {
            SharedKey = key,
            RecoveryCodes = recoveryCodes,
            RecoveryCodesLeft = recoveryCodes?.Length ?? await userManager.CountRecoveryCodesAsync(user),
            IsTwoFactorEnabled = await userManager.GetTwoFactorEnabledAsync(user),
            IsMachineRemembered = await _signInManager.IsTwoFactorClientRememberedAsync(user),
        });
    }

    [HttpGet("manage/info")]
    [Authorize(nameof(FinActionsPermissions.UsuarioCriar))]
    public async Task<Results<Ok<InfoResponse>, ValidationProblem, NotFound>> GetLoginInformation()
    {
        if (await _userManager.GetUserAsync(HttpContext.User) is not { } user)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(await CreateInfoResponseAsync(user, _userManager));
    }

    [NonAction]
    [HttpPost("manage/info")]
    public async Task<Results<Ok<InfoResponse>, ValidationProblem, NotFound>> PostLoginInformation([FromBody] InfoRequest infoRequest)
    {
        if (await _userManager.GetUserAsync(HttpContext.User) is not { } user)
        {
            return TypedResults.NotFound();
        }

        if (!string.IsNullOrEmpty(infoRequest.NewEmail) && !_emailAddressAttribute.IsValid(infoRequest.NewEmail))
        {
            return CreateValidationProblem(IdentityResult.Failed(_userManager.ErrorDescriber.InvalidEmail(infoRequest.NewEmail)));
        }

        if (!string.IsNullOrEmpty(infoRequest.NewPassword))
        {
            if (string.IsNullOrEmpty(infoRequest.OldPassword))
            {
                return CreateValidationProblem("OldPasswordRequired",
                    "The old password is required to set a new password. If the old password is forgotten, use /resetPassword.");
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, infoRequest.OldPassword, infoRequest.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                return CreateValidationProblem(changePasswordResult);
            }
        }

        if (!string.IsNullOrEmpty(infoRequest.NewEmail))
        {
            var email = await _userManager.GetEmailAsync(user);

            if (email != infoRequest.NewEmail)
            {
                await SendConfirmationEmailAsync(user, HttpContext, infoRequest.NewEmail, isChange: true);
            }
        }

        return TypedResults.Ok(await CreateInfoResponseAsync(user, _userManager));
    }

    private async Task SendConfirmationEmailAsync(AppUser user, HttpContext context, string email, bool isChange = false)
    {
        var code = isChange
            ? await _userManager.GenerateChangeEmailTokenAsync(user, email)
            : await _userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        var userId = await _userManager.GetUserIdAsync(user);
        var routeValues = new RouteValueDictionary()
        {
            ["userId"] = userId,
            ["code"] = code,
        };

        if (isChange)
        {
            // This is validated by the /confirmEmail endpoint on change.
            routeValues.Add("changedEmail", email);
        }

        var confirmEmailUrl = _linkGenerator.GetUriByName(context, "confirmEmail", routeValues)
            ?? throw new NotSupportedException($"Could not find endpoint named 'confirmEmail'.");

        await _emailSender.SendConfirmationLinkAsync(user, email, HtmlEncoder.Default.Encode(confirmEmailUrl));
    }

    private static ValidationProblem CreateValidationProblem(string errorCode, string errorDescription) =>
        TypedResults.ValidationProblem(new Dictionary<string, string[]> {
            { errorCode, [errorDescription] }
        });

    private static ValidationProblem CreateValidationProblem(IdentityResult result)
    {
        // We expect a single error code and description in the normal case.
        // This could be golfed with GroupBy and ToDictionary, but perf! :P
        Debug.Assert(!result.Succeeded);
        var errorDictionary = new Dictionary<string, string[]>(1);

        foreach (var error in result.Errors)
        {
            string[] newDescriptions;

            if (errorDictionary.TryGetValue(error.Code, out var descriptions))
            {
                newDescriptions = new string[descriptions.Length + 1];
                Array.Copy(descriptions, newDescriptions, descriptions.Length);
                newDescriptions[descriptions.Length] = error.Description;
            }
            else
            {
                newDescriptions = [error.Description];
            }

            errorDictionary[error.Code] = newDescriptions;
        }

        return TypedResults.ValidationProblem(errorDictionary);
    }

    private static async Task<InfoResponse> CreateInfoResponseAsync<TUser>(TUser user, UserManager<TUser> userManager)
        where TUser : class
    {
        return new()
        {
            Email = await userManager.GetEmailAsync(user) ?? throw new NotSupportedException("Users must have an email."),
            IsEmailConfirmed = await userManager.IsEmailConfirmedAsync(user),
        };
    }
}
