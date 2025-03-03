using FinActions.Application.Base;

namespace FinActions.Application.Identity.Contracts;

public class GetAppUserDto : PagedResultRequestDto
{
    public string UserName { get; set; }
    public string Email { get; set; }
}
