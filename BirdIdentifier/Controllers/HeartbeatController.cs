using Microsoft.AspNetCore.Mvc;

namespace BirdIdentifier.Controllers;

/**
 * <summary>Controller that has a simple endpoint to let the user know if the service is available</summary>
 */
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