using Microsoft.AspNetCore.Mvc;

namespace MinhaApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StatusController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
        var version = typeof(Program).Assembly.GetName().Version?.ToString() ?? "unknown";

        return Ok(new
        {
            status = "API online",
            environment,
            version,
            serverTimeUtc = DateTime.UtcNow
        });
    }
}
