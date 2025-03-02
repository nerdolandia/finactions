namespace FinActions.Application.Base;

public class PagedResultDto<T>
{
    public int TotalCount { get; set; }
    public IReadOnlyList<T> Items { get; set; }
}
