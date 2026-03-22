using Microsoft.AspNetCore.Mvc;
using TrainingTracker.Application.Interfaces;

namespace TrainingTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _service;

    public DashboardController(IDashboardService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult> GetSummary(CancellationToken cancellationToken)
    {
        var summary = await _service.GetSummaryAsync(cancellationToken);
        return Ok(summary);
    }
}
