using Microsoft.AspNetCore.Mvc;
using transactions.portal.services.Dtos;
using transactions.portal.services.Services;

namespace transactions.portal.services.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DisputesController : ControllerBase
{
    private readonly IDisputeService _disputeService;

    public DisputesController(IDisputeService disputeService)
    {
        _disputeService = disputeService;
    }

    [HttpGet]
    public async Task<ActionResult<List<DisputeListItemDto>>> GetDisputes(
        [FromQuery] string? status,
        [FromQuery] DateTime? fromDate,
        [FromQuery] DateTime? toDate)
    {
        var result = await _disputeService.GetDisputesAsync(status, fromDate, toDate);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DisputeDetailDto>> GetDisputeById(string id)
    {
        var result = await _disputeService.GetDisputeByIdAsync(id);
        return Ok(result);
    }
}

