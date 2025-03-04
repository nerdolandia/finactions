using System.ComponentModel.DataAnnotations;

namespace FinActions.Application.Base.Requests;

public abstract class PagedResultRequestDto
{
    [Range(0, int.MaxValue)]
    public int Skip { get; set; }
    public int Take { get; set; } = 20;
}
