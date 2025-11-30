using Microsoft.AspNetCore.Mvc;

namespace BirdIdentifierBackend.Controllers
{
    [ApiController]
    public class HeartbeatController : ControllerBase
    {
        private const string ServiceRunning = /*lang=json,strict*/ "{ \"health\":\"ok\" }";

        [HttpGet("heartbeat")]
        public IActionResult GetHeartbeat()
            => Content(ServiceRunning);
    }
}