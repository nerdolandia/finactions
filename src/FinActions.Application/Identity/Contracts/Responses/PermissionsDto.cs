namespace FinActions.Application.Identity.Contracts.Responses;

public record struct PermissionsDto()
{
    public ICollection<string> Permissions { get; set; } = [];
}
