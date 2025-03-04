namespace FinActions.Application.Base.Requests;

public abstract record UserPagedResultRequestDto : PagedResultRequestDto
{
    public Guid UserId { get; init; }
}
