namespace FinActions.Application.Base.Responses;

public record struct PagedResultDto<T>
{
    public int TotalCount { get; set; }
    public IReadOnlyList<T> Items { get; set; }
}
