using System.ComponentModel.DataAnnotations;

namespace FinActions.Application.Base;

public abstract class PagedResultRequestDto
{
    [Range(0, int.MaxValue)]
    public int SkipCount { get; set; }
    public int MaxResultCount { get; set; }
}
