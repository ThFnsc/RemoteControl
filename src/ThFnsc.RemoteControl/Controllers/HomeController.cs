using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ThFnsc.RemoteControl.Controllers;

[Route("/")]
[ApiExplorerSettings(IgnoreApi = true)]
[AllowAnonymous]
public class HomeController : Controller
{
    [HttpGet]
    public IActionResult Index() => RedirectPermanent("Swagger");
}
