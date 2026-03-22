using Microsoft.AspNetCore.Mvc;

namespace MinhaApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EchoController : ControllerBase
{
    [HttpPost]
    public IActionResult Post([FromBody] object payload)
    {
        return Ok(new
        {
            received = payload,
            receivedAtUtc = DateTime.UtcNow
        });
    }
}
