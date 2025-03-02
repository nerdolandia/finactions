namespace FinActions.Application.Base;

public record struct PagedResultDto<T>
{
    public int TotalCount { get; set; }
    public IReadOnlyList<T> Items { get; set; }
}
