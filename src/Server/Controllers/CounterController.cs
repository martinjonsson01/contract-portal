using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers;

[ApiController]
[Route("api/counter123")]
public class CounterController : Controller
{
    [HttpPost]
    public ActionResult<string> IncrementCounter()
    {
        return "hi";
    }
}
