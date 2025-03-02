namespace FinActions.Application.Identity.Contracts;

public sealed class LoginRequestDto
{
    //
    // Summary:
    //     The user's name.
    public required string UserName { get; init; }
    //
    // Summary:
    //     The user's password.
    public required string Password { get; init; }
}
