using Microsoft.AspNetCore.Mvc;

namespace MinhaApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalesForecastController : ControllerBase
{
    [HttpPost("estimate")]
    public IActionResult Estimate([FromBody] SalesForecastRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var baseDemand = 120m;
        var temperatureBoost = request.TemperatureCelsius * 8m;
        var weekendBoost = request.IsWeekend ? 30m : 0m;
        var campaignBoost = request.MarketingCampaignActive ? 18m : 0m;

        var estimatedUnits = Math.Max(0m, baseDemand + temperatureBoost + weekendBoost + campaignBoost);

        return Ok(new SalesForecastResponse(
            EstimatedSalesUnits: Math.Round(estimatedUnits, 0),
            GeneratedAtUtc: DateTime.UtcNow,
            Assumptions: new[]
            {
                "Modelo demonstrativo para deploy e observabilidade.",
                "Nao utiliza machine learning real; serve como API de exemplo para App Service."
            }
        ));
    }
}

public record SalesForecastRequest(
    decimal TemperatureCelsius,
    bool IsWeekend,
    bool MarketingCampaignActive
);

public record SalesForecastResponse(
    decimal EstimatedSalesUnits,
    DateTime GeneratedAtUtc,
    string[] Assumptions
);
