namespace FinActions.Application.Identity.Contracts;

public record struct PermissionsDto()
{
    public ICollection<string> Permissions { get; set; } = [];
}
