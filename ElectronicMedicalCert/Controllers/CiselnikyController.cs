using ElectronicMedicalCert.Application.Features.Codebooks.Queries.GetCodebookItems;
using ElectronicMedicalCert.Application.Features.Codebooks.Queries.GetCodebooks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicMedicalCert.Controllers;

[ApiController]
[Authorize]
[Route("api/v2/ciselniky")]
public sealed class CiselnikyController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await mediator.Send(new GetCodebooksQuery(), ct);
        return Ok(result);
    }

    [HttpGet("{kod}/polozky")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetItems([FromRoute] string kod, CancellationToken ct)
    {
        var result = await mediator.Send(new GetCodebookItemsQuery(kod), ct);
        return Ok(result);
    }
}

