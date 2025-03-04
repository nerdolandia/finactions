namespace FinActions.Application.Base.Requests;

public abstract record UserResultRequestDto
{
    public Guid UserId { get; init; }
}
