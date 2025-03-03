using FinActions.Domain.Shared.Extensions;

namespace FinActions.Domain.Shared.Security;

public static class PermissionsConsts
{
    public const string Allow = "allow";
    public const string Deny = "deny";
    public static readonly ICollection<FinActionsPermissions> Permissions = EnumExtensions.GetEnumValues<FinActionsPermissions>();
}
