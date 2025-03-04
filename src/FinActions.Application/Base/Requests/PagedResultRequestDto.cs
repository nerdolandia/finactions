using System.ComponentModel.DataAnnotations;

namespace FinActions.Application.Base.Requests;

public abstract record PagedResultRequestDto
{
    [Range(0, int.MaxValue)]
    public int Skip { get; init; }
    public int Take { get; init; } = 20;
}
