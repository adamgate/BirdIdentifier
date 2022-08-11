using Microsoft.AspNetCore.Mvc;

namespace BirdIdentifier.Controllers;

[ApiController]
[Route("heartbeat")]
public class HeartbeatController : ControllerBase
{
    /**
     * <summary>Used to determine if the service is reachable</summary>
     * <returns>Http Status Code</returns>
     */
    [HttpGet]
    public IActionResult Heartbeat()
    {
        return Content("{ \"health\":\"ok\" }");
    }
}