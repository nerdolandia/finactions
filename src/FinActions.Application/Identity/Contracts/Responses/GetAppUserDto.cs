using FinActions.Application.Base.Requests;

namespace FinActions.Application.Identity.Contracts.Requests;

public class GetAppUserDto : PagedResultRequestDto
{
    public string UserName { get; set; }
    public string Email { get; set; }
}
