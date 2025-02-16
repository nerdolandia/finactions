using Microsoft.AspNetCore.Mvc;

namespace FinActions.Api.Host.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
public class HomeController : ControllerBase
{
    [Route("/")]
    public ActionResult Index()
    {
        return Redirect("~/swagger");
    }
}
