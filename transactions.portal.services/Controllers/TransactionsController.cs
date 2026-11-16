using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using transactions.portal.services.Dtos;
using transactions.portal.services.Services;

namespace transactions.portal.services.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly ITransactionService _transactionService;
    private readonly IDisputeService _disputeService;
    private readonly IValidator<CreateDisputeRequest> _validator;

    public TransactionsController(
        ITransactionService transactionService,
        IDisputeService disputeService,
        IValidator<CreateDisputeRequest> validator)
    {
        _transactionService = transactionService;
        _disputeService = disputeService;
        _validator = validator;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<TransactionListItemDto>>> GetTransactions(
        [FromQuery] string? search,
        [FromQuery] string? status,
        [FromQuery] DateTime? fromDate,
        [FromQuery] DateTime? toDate,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await _transactionService.GetTransactionsAsync(
            search,
            status,
            fromDate,
            toDate,
            page,
            pageSize);

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TransactionDetailDto>> GetTransactionById(string id)
    {
        var result = await _transactionService.GetTransactionByIdAsync(id);
        return Ok(result);
    }

    [HttpPost("{id}/disputes")]
    public async Task<ActionResult<DisputeDetailDto>> CreateDispute(
        string id,
        [FromBody] CreateDisputeRequest request)
    {
        await _validator.ValidateAndThrowAsync(request);
        var created = await _disputeService.CreateDisputeAsync(id, request);
        return CreatedAtAction(
            nameof(DisputesController.GetDisputeById),
            "Disputes",
            new { id = created.Id },
            created);
    }
}

